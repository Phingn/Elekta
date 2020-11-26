using Elekta.Code.Challenge.Api.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Models
{
    public class Booking
    {
        public Booking()
        {
            CreatedDate = DateTime.UtcNow;
            EndDate = StartDate.AddHours(1);
        }
        public int? BookingID { get; set; }
        public BookingStatus bookingStatus { get; set; }
        public int PatientID { get; set; }
        public int EquipmentID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool isDeleted  { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
