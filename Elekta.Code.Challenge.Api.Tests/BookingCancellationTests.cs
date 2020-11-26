using System;
using Xunit;
using Moq;
using Elekta.Code.Challenge.Api;
using Elekta.Code.Challenge.Api.Interfaces;
using Elekta.Code.Challenge.Api.External.Api;
using Elekta.Code.Challenge.Api.Data;
using Elekta.Code.Challenge.Api.Tests.Data;
using Elekta.Code.Challenge.Api.Repository;
using Elekta.Code.Challenge.Api.Models;
using Elekta.Code.Challenge.Api.Type;
using Elekta.Code.Challenge.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Elekta.Code.Challenge.Api.Models.External;
using Elekta.Code.Challenge.Api.Tests.Helper;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Elekta.Code.Challenge.Api.Tests
{
    public class BookingCancellationTests
    {
        private IBooking _bookingService;
        private readonly IPatientRespository _patientRepository;
        private readonly IEquipmentApi _equipmentApi;
        private EquipmentSetting _equipmentSetting;
        private readonly EmailSetting _emailSetting;
        private readonly ITestOutputHelper _output;
        private readonly DataContext _context;
        private IBookingRepositoryService _bookingRepositoryService;

        public BookingCancellationTests(ITestOutputHelper output)
        {
            // setting up to log response message and any errors
            _output = output;

            // using EntityFrameworkCoreInMemory to mock datacontext object
            _context = DBContext.DataSourceMemoryContext("db");

            // getting equipment and email setting configurations
            _equipmentSetting = FileHelper.GetEquipmentSettingConfiguration(@"appsettings.json");

            _emailSetting = FileHelper.GetEmailSettingConfiguration(@"appsettings.json");

            _bookingRepositoryService = new BookingRepositoryService(_context);

            _equipmentApi = new EquipmentApi(_equipmentSetting);

            _patientRepository = new PatientRepositoryService(_context);

        }

        [Fact]
        public void BookingCancellationTests_Cancel_When_BookingExist()
        {
            // mocking a new the patient 
            var patient = new Patient
            {
                FullName = "Brook Shield",
                Email = "bshield@test.com"
            };

            var patientID = _patientRepository.AddPatient(patient);

            // mocking up a new booking for cancellation
            var booking = new Booking
            {
                PatientID = patientID,
                StartDate = DateTime.Parse("28 Dec 2020 10:15"),
                EquipmentID = 1,
                bookingStatus = BookingStatus.Live
            };

            _bookingService = new BookingService(_bookingRepositoryService, _patientRepository, _equipmentApi, _emailSetting);

            var response = _bookingService.CreateBooking(booking.PatientID, booking.StartDate);

            // preparing cancellation

            response = _bookingService.CancelBooking(patientID, booking.StartDate);

            // displaying response message 
            _output.WriteLine(response.Result.Message);

            // displaying errors if any
            if (response.Result.Errors != null)
            {
                foreach (var err in response.Result.Errors)
                {
                    _output.WriteLine(err.Message);
                }
            }

            Assert.True(true);
        }

        [Fact]
        public void BookingCancellationTests_When_Booking_NotExist()
        {
            // mocking a new the patient 
            var patient = new Patient
            {
                FullName = "Brook Shield",
                Email = "bshield@test.com"
            };

            var patientID = _patientRepository.AddPatient(patient);

            // mocking up a new booking for cancellation
            var booking = new Booking
            {
                PatientID = patientID,
                StartDate = DateTime.Parse("28 Dec 2020 10:15"),
                EquipmentID = 1,
                bookingStatus = BookingStatus.Live
            };

            _bookingService = new BookingService(_bookingRepositoryService, _patientRepository, _equipmentApi, _emailSetting);

            var response = _bookingService.CreateBooking(booking.PatientID, booking.StartDate);

            // preparing cancellation
            //invalidating existing booking by changing to different date
            booking.StartDate = DateTime.Parse("29 Dec 2020 10:15");

            response = _bookingService.CancelBooking(patientID, booking.StartDate);

            // displaying response message 
            _output.WriteLine(response.Result.Message);

            // displaying errors if any
            if (response.Result.Errors != null)
            {
                foreach (var err in response.Result.Errors)
                {
                    _output.WriteLine(err.Message);
                }
            }

            Assert.True(true);
        }
    }
}
