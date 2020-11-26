using Elekta.Code.Challenge.Api.Models.External;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Helper
{
    public static class JsonFileHelper
    {
        public static List<Equipment> GetEquipmentAvailability(string dataFile)
        {
            string subdirectory = $@"Data\External\";
            string parentPath = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\{subdirectory}";

            List<Equipment> equipment = new List<Equipment>();
            using (StreamReader r = new StreamReader($"{parentPath}{dataFile}"))
            {
                string json = r.ReadToEnd();
                equipment = JsonConvert.DeserializeObject<List<Equipment>>(json);
            }

            return equipment;
        }
    }
}
