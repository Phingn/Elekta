using Elekta.Code.Challenge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Interfaces
{
    public interface IBooking
    {
        Task<ClientResponse> CreateBooking(int patientID, DateTime appointmentDate);
        Task<ClientResponse> CancelBooking(int PatientID, DateTime AppointmentDate);
        Task<ClientResponse> UpdateBooking(int patientID, DateTime currentAppointmentDate, DateTime newAppointmentDate);
        Task<List<Booking>> GetTodayBooking();
    }
}
