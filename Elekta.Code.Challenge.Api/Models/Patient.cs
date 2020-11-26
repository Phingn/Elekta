using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Models
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
