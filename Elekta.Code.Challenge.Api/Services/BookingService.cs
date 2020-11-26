using Elekta.Code.Challenge.Api.Data;
using Elekta.Code.Challenge.Api.External.Api;
using Elekta.Code.Challenge.Api.Helper;
using Elekta.Code.Challenge.Api.Interfaces;
using Elekta.Code.Challenge.Api.Models;
using Elekta.Code.Challenge.Api.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Services
{
    public class BookingService : IBooking
    {
        private readonly IBookingRepositoryService _bookingRepositoryService;
        private readonly IPatientRespository _patientRepository;
        private readonly IEquipmentApi _equipmentApi;
        private readonly EmailSetting _emailSetting;
        private List<ErrorDetails> _errorDetails = new List<ErrorDetails>();

        public BookingService(
                IBookingRepositoryService bookingRespositoryService, 
                IPatientRespository patientRepository, 
                IEquipmentApi equipmentApi,
                EmailSetting emailSetting)
        {
            _bookingRepositoryService = bookingRespositoryService;
            _patientRepository = patientRepository;
            _equipmentApi = equipmentApi;
            _emailSetting = emailSetting;
        }

        public async Task<ClientResponse> CreateBooking(int patientID, DateTime appointmentDate)
        {

            var equipmentId = _equipmentApi.GetAvailableEquipment(appointmentDate);
            if (equipmentId == 0)
            {
                var error = new ErrorDetails
                {
                    StatusCode = 0,
                    Message = "Equipment is not available for the requested date and time"
                };
                _errorDetails.Add(error);

                return new ClientResponse
                {
                    Message = "Your request was unsuccessfully completed",
                    Errors = _errorDetails
                };
            }

            if (!ValidationHelper.ValidateNewBooking(patientID, appointmentDate, ref _errorDetails))
            {
                var clientReponse = new  ClientResponse
                {
                    Message = "Your request was unsuccessfully completed",
                    Errors = _errorDetails
                };

                return clientReponse;
            }

            var booking = new Booking
            {
                bookingStatus = BookingStatus.Live,
                PatientID = patientID,
                EquipmentID = equipmentId,
                StartDate = appointmentDate,
                //set the appointment EndTime is 1hr to StartTime 
                EndDate = appointmentDate.AddHours(1),
                CreatedDate = DateTime.Now
            };

            var bookingID = _bookingRepositoryService.CreateBooking(booking);

            // checking if booking created successfully then do the following actions
            // 1. Confirming Equipment Availablility System that slot had taken.
            // 2. Get patient's email and fake send out notification 

            if (bookingID > 0 )
            {
                // Updating Availablility System that slot had taken.

                _equipmentApi.BookingRequestConfirm(equipmentId, appointmentDate);

                var email = _patientRepository.GetPatientEmail(patientID);

                EmailHelper.SendEmail("Booking Created", email, "some body text", _emailSetting);

            }

            return new ClientResponse
            {
                Message = "Your request has completed successfully",
                Errors = null
            };
        }
        public async Task<ClientResponse> CancelBooking(int patientID, DateTime appointmentDate)
        {
            var booking = _bookingRepositoryService.GetExistingBooking(patientID, appointmentDate);

            if (booking == null)
            {
                _errorDetails.Add(ErrorHelper.BuildError("Sorry the appointment request does not exist"));

                return new ClientResponse
                {
                    Message = "Your request was unsuccessfully completed",
                    Errors = _errorDetails
                };
            }

            if (!ValidationHelper.ValidateCancelBooking(booking.EndDate?? DateTime.MaxValue, ref _errorDetails))
            {
                return new ClientResponse
                {
                    Message = "Your request was unsuccessfully completed",
                    Errors = _errorDetails
                };
            }

            // requesting cancellation from Equipment Availability System
            _equipmentApi.BookingRequestConfirm(booking.EquipmentID, booking.EndDate ?? DateTime.MaxValue);

            // preparing cancel data and cancel booking
            booking.bookingStatus = BookingStatus.Cancel;
            booking.DeletedDate = DateTime.Now;
            booking.isDeleted = true;

            _bookingRepositoryService.UpdateBooking(booking);

            return new ClientResponse
            {
                Message = "Your request was successfully completed",
                Errors = null
            };
        }

        public async Task<ClientResponse> UpdateBooking(int patientID, DateTime currentAppointmentDate, DateTime newAppointmentDate)
        {
            var booking = _bookingRepositoryService.GetExistingBooking(patientID, currentAppointmentDate);

            if (booking == null)
            {
                _errorDetails.Add(ErrorHelper.BuildError("Sorry the appointment request does not exist"));

                return new ClientResponse
                {
                    Message = "Your request was unsuccessfully completed",
                    Errors = _errorDetails
                };
            }

            // validating booking dates variant
            if (!ValidationHelper.ValidateUpdateBooking(booking.EndDate ?? DateTime.MaxValue, newAppointmentDate, ref _errorDetails))
            {
                return new ClientResponse
                {
                    Message = "Your request was unsuccessfully completed",
                    Errors = _errorDetails
                };
            }

            //requesting 'Equipment Available System' both cancel existing equipment and providing new availability

            var equipmentId = _equipmentApi.GetAvailableEquipment(currentAppointmentDate, newAppointmentDate);
            if (equipmentId == 0)
            {
                _errorDetails.Add(ErrorHelper.BuildError("Equipment is not available for the requested date and time."));
                return new ClientResponse
                {
                    Message = "Your request was unsuccessfully completed",
                    Errors = _errorDetails
                };
            }

            // preparing update data and update booking
            booking.EquipmentID = equipmentId;
            booking.StartDate  = newAppointmentDate;
            booking.EndDate = newAppointmentDate.AddHours(1);
            booking.UpdatedDate  = DateTime.Now;

            _bookingRepositoryService.UpdateBooking(booking);

            // Reconfirming new request to Equipment Availability System
            _equipmentApi.BookingRequestConfirm(booking.EquipmentID, booking.EndDate ?? DateTime.MaxValue);

            return new ClientResponse
            {
                Message = "Your request was successfully completed",
                Errors = null
            };
        }

        public async Task<List<Booking>> GetTodayBooking()
        {
            return _bookingRepositoryService.GetTodayBookings();
        }

    }
}
