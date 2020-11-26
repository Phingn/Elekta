using Elekta.Code.Challenge.Api.Data;
using Elekta.Code.Challenge.Api.Interfaces;
using Elekta.Code.Challenge.Api.Models;
using Elekta.Code.Challenge.Api.Type;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Repository
{
    public class BookingRepositoryService : IBookingRepositoryService
    {
        private readonly DataContext _context;
        public BookingRepositoryService(DataContext context)
        {
            _context = context;
        }
        public int CreateBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return booking.BookingID ?? 0;
        }

        public void UpdateBooking(Booking booking)
        {
            _context.Entry(booking).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public List<Booking> GetTodayBookings()
        {
            var booking = _context.Bookings.ToList();

            return  _context.Bookings
                    .Where(b => b.EndDate >= DateTime.Today
                        && b.EndDate <= DateTime.Today.AddDays(1).AddTicks(-1))
                    .ToList();
        }

        public Booking GetExistingBooking(int patientId, DateTime appointmentDate)
        {

            return _context.Bookings
                .Where(b => b.PatientID == patientId
                            && b.StartDate <= appointmentDate
                            && b.EndDate >= appointmentDate
                    )
                    .FirstOrDefault();

        }

        public List<Booking> GetAllBooking()
        {
            return _context.Bookings.ToList();
        }
    }
}
