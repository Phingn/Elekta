using Elekta.Code.Challenge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Interfaces
{
    public interface IBookingRepositoryService
    {
        int CreateBooking(Booking booking);
        void UpdateBooking(Booking booking);
        List<Booking> GetTodayBookings();
        Booking GetExistingBooking(int patientId, DateTime appointmentDate);
        List<Booking> GetAllBooking();
    }
}
