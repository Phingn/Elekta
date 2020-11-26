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
    public class BookingSelectTests
    {
        private IBooking _bookingService;

        private readonly IPatientRespository _patientRepository;
        private readonly IEquipmentApi _equipmentApi;
        private EquipmentSetting _equipmentSetting;
        private readonly EmailSetting _emailSetting;
        private readonly ITestOutputHelper _output;
        private readonly DataContext _context;
        private IBookingRepositoryService _bookingRepositoryService;

        public BookingSelectTests(ITestOutputHelper output)
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

        /// <summary>
        /// BookingSelectTests_GetAll_Bookings_for_Today *****************************************************/
        /// **************************************************************************************************
        /// Note: It is difficult to test getting all bookings/appointments for today
        /// as new booking is required at least 2 wks booking in advance. Hence can't run the code automatically
        /// to create bookings today enable to display.
        /// Tweaking: During debug mode put a break just before the new booking is saved then change the booking EndDate to today date.
        /// The class and method reference in the main API project are:  Elekta.Code.Challenge.Api.Repository and CreateBooking(Booking booking)
        /// </summary>
        [Fact]
        public void BookingSelectTests_GetAll_Bookings_for_Today()
        {
            // mocking a new patient 
            var patient = new Patient
            {
                FullName = "Brook Shield",
                Email = "bshield@test.com"
            };

            var patientID = _patientRepository.AddPatient(patient);

            // mocking up a new booking
            var booking = new Booking
            {
                PatientID = patientID,
                StartDate = DateTime.Parse("28 Dec 2020 10:15"),
                EquipmentID = 0,
                bookingStatus = BookingStatus.Live
            };

            _bookingService = new BookingService(_bookingRepositoryService, _patientRepository, _equipmentApi, _emailSetting);

            var response = _bookingService.CreateBooking(booking.PatientID, booking.StartDate);

            // tweaking appointment date to make a valid case for today selection
            var newAppointmentDate = DateTime.Now.Date;

            var bookings = _bookingService.GetTodayBooking();

            // displaying booking
            if (bookings != null)
            {
                foreach (var bk in bookings.Result)
                {
                    _output.WriteLine($"{bk.BookingID}: {bk.bookingStatus}:{bk.EquipmentID}: {bk.PatientID} {bk.StartDate}: {bk.EndDate} ");
                }
            }

            Assert.True(true);
        }
    }
}
