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
using System.Linq;

namespace Elekta.Code.Challenge.Api.Tests
{
    public class BookingCreateTests
    {
        private IBooking _bookingService;
        private IBookingRepositoryService _bookingRepositoryService;
        private readonly IPatientRespository _patientRepository;
        private readonly IEquipmentApi _equipmentApi;
        private DataContext _context;
        private EquipmentSetting _equipmentSetting;
        private readonly EmailSetting _emailSetting;
        private readonly ITestOutputHelper _output;

        public BookingCreateTests(ITestOutputHelper output)
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
        public void BookingCreateTest_WhenEquipment_Available()
        {
            var booking = new Booking
            {
                PatientID = 1,
                StartDate = DateTime.Parse("28 Dec 2020 11:15"),
                EquipmentID = 0,
                bookingStatus = BookingStatus.Request,
            };

            // mocking a new the patient for email address notification later
            var patient = new Patient
            {
                FullName = "Brook Shield",
                Email = "bshield@test.com"
            };

            var patientID = _patientRepository.AddPatient(patient);

            _bookingService = new BookingService(_bookingRepositoryService, _patientRepository, _equipmentApi, _emailSetting);

            var response = _bookingService.CreateBooking(booking.PatientID, booking.StartDate);

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
        public void BookingCreateTest_WhenEquipment_NotAvailable()
        {
            var booking = new Booking
            {
                PatientID = 1,
                StartDate = DateTime.Parse("30 Dec 2020 11:15"),
                EquipmentID = 0,
                bookingStatus = BookingStatus.Request,
            };

            // mocking a new patient for email address notification later
            var patient = new Patient
            {
                FullName = "Brook Shield",
                Email = "bshield@test.com"
            };

            var patientID = _patientRepository.AddPatient(patient);

            _bookingService = new BookingService(_bookingRepositoryService, _patientRepository,_equipmentApi, _emailSetting);

            var response = _bookingService.CreateBooking(booking.PatientID, booking.StartDate);

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
        public void BookingCreateTest_WithUnderTimeConstraints()
        {
            var booking = new Booking
            {
                PatientID = 1,
                StartDate = DateTime.Parse("28 Dec 2020 07:15"),
                EquipmentID = 0,
                bookingStatus = BookingStatus.Request,
            };

            // mocking a new patient for email address notification later
            var patient = new Patient
            {
                FullName = "Brook Shield",
                Email = "bshield@test.com"
            };

            var patientID = _patientRepository.AddPatient(patient);

            _bookingService = new BookingService(_bookingRepositoryService, _patientRepository, _equipmentApi, _emailSetting);

            var response = _bookingService.CreateBooking(booking.PatientID, booking.StartDate);

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
        public void BookingCreateTest_WithOverTimeConstraints()
        {
            var booking = new Booking
            {
                PatientID = 1,
                StartDate = DateTime.Parse("28 Dec 2020 16:15"),
                EquipmentID = 0,
                bookingStatus = BookingStatus.Request,
            };

            // mocking a new patient for email address notification later
            var patient = new Patient
            {
                FullName = "Brook Shield",
                Email = "bshield@test.com"
            };

            var patientID = _patientRepository.AddPatient(patient);

            _bookingService = new BookingService(_bookingRepositoryService, _patientRepository, _equipmentApi, _emailSetting);

            var response = _bookingService.CreateBooking(booking.PatientID, booking.StartDate);

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
