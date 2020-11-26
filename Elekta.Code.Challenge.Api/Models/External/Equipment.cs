using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Models.External
{
    public class Equipment
    {
        public int EquipmentID { get; set; }
        public bool isAvailable { get; set; }
        public DateTime Date { get; set; }
    }
}
