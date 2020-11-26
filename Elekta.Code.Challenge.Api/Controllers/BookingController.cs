using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elekta.Code.Challenge.Api.Interfaces;
using Elekta.Code.Challenge.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Elekta.Code.Challenge.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BookingController : ControllerBase
    {
        private readonly IBooking _booking;

        public BookingController(IBooking booking)
        {
            _booking = booking;
        }

        [HttpGet("search")]
        public Task<List<Booking>> Get()
        {
            return _booking.GetTodayBooking();
        }

        [HttpPost("create")]
        public Task<ClientResponse> CreateNewBooking([FromBody] int patientId, DateTime appointmentDate)
        {
            return _booking.CreateBooking(patientId, appointmentDate);
        }

        [HttpPost("cancel")]
        public Task<ClientResponse> CancelBooking([FromBody] int patientId, DateTime appointmentDate)
        {
            return _booking.CancelBooking(patientId, appointmentDate);
        }

        [HttpPost("update")]
        public Task<ClientResponse> UpdateBooking([FromBody] int patientId, DateTime currentAppointmentDate, DateTime newAppointmentDate)
        {
            return _booking.CancelBooking(patientId, currentAppointmentDate);
        }

    }
}
