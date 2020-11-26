using Elekta.Code.Challenge.Api.External.Api;
using Elekta.Code.Challenge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Helper
{
    public static class ValidationHelper
    {
        public static bool ValidateNewBooking(int PatientID, DateTime appointmentDate, ref List<ErrorDetails> errorDetails)
        {
            ErrorDetails error = new ErrorDetails();

            // checking for appointment date if not less than 2 weeks from now
            if ((appointmentDate - DateTime.Now).TotalDays < 14 )
            {
                errorDetails.Add(ErrorHelper.BuildError("Appointments can only be made 2 weeks later at most."));
            }

            // checking for time out side 8:00 and 16:00 range

            TimeSpan startTime = new TimeSpan(appointmentDate.Hour, appointmentDate.Minute, appointmentDate.Second);
            TimeSpan lowerRangeTime = new TimeSpan(8, 0,0);
            TimeSpan upperRangeTime = new TimeSpan(16, 0, 0);

            if (startTime.CompareTo(lowerRangeTime) < 0  || startTime.CompareTo(upperRangeTime) > 0)
            {
                errorDetails.Add(ErrorHelper.BuildError("Appointments can only be made between 08:00 and 16:00"));
            }

            if (errorDetails.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool ValidateCancelBooking(DateTime appointmentDate, ref List<ErrorDetails> errorDetails)
        {

            if ((appointmentDate - DateTime.Now).TotalDays < 3)
            {
                errorDetails.Add(ErrorHelper.BuildError("Sorry appointment can only be cancelled up to 3 days before the appointment date."));
                return false;
            }

            return true;
        }

        public static bool ValidateUpdateBooking(DateTime currentAppointmentDate, DateTime newAppointmentDate, ref List<ErrorDetails> errorDetails)
        {

            // checking for appointment date if not less than 2 days from now then allow to change
            if ((currentAppointmentDate - DateTime.Now).TotalDays < 2)
            {
                errorDetails.Add(ErrorHelper.BuildError("Appointment can only be changed up to 2 days before the appointment date."));
            }

            // checking for appointment date if not less than 2 weeks from now
            if ((newAppointmentDate - DateTime.Now).TotalDays < 14)
            {
                errorDetails.Add(ErrorHelper.BuildError("Appointments can only be made 2 weeks later at most"));
            }

            if (errorDetails.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
