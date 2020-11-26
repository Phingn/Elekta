using Elekta.Code.Challenge.Api.Helper;
using Elekta.Code.Challenge.Api.Models;
using Elekta.Code.Challenge.Api.Models.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.External.Api
{
    public class EquipmentApi : IEquipmentApi
    {
        private readonly EquipmentSetting _equipmentSetting;
        //private readonly HttpClient _client;
        public EquipmentApi(EquipmentSetting equipmentSetting)
        {
            _equipmentSetting = equipmentSetting;
        }

        public int GetAvailableEquipment(DateTime requestDate)
        {
            if (_equipmentSetting.Environment == "Production")
            {
                using (var _client = new HttpClient())
                {
                    //calling external equipment availability api and getting availability status
                    _client.BaseAddress = new Uri(_equipmentSetting.Uri);
                    _client.DefaultRequestHeaders.Accept.Clear();

                    //TODO: Making a request to return availability status??

                }
            }
            else
            {
                var equipment = JsonFileHelper.GetEquipmentAvailability(_equipmentSetting.DataFile);

                var equipmentId = equipment
                    .Where(e => e.isAvailable == true
                            && e.Date >= requestDate)
                    .Select(x => x.EquipmentID)
                    .FirstOrDefault();

                return equipmentId;
            }

            return 0;
        }

        public bool BookingRequestConfirm(int equipmentID, DateTime requestDate)
        {
            // Confirming Equipment Availablility System that that slot had taken.
            //TODO: Calling Equipment Availability API

            return true;
        }

        public bool BookingCancelConfirm(int equipmentID, DateTime requestDate)
        {
            // Request cancellation from Equipment Availablility System
            //TODO: Calling Equipment Availability API

            return true;
        }

        /// <summary>
        /// GetAvailableEquipment - 
        /// Requesting both to cancel the current booking date
        /// and provide New availability of EquipmentID for the new request date
        /// </summary>
        /// <param name="requestDate"></param>
        /// <param name="newRequestDate"></param>
        /// <returns></returns>
        public int GetAvailableEquipment(DateTime requestDate, DateTime newRequestDate)
        {
            if (_equipmentSetting.Environment == "Production")
            {
                using (var _client = new HttpClient())
                {
                    //calling external equipment availability api and getting availability status
                    _client.BaseAddress = new Uri(_equipmentSetting.Uri);
                    _client.DefaultRequestHeaders.Accept.Clear();

                    //TODO: Making a request to return availability status??

                }
            }
            else
            {
                // cancelling of existing appointment
                // providing new availability for the new request date

                var equipment = JsonFileHelper.GetEquipmentAvailability(_equipmentSetting.DataFile);

                var equipmentId = equipment
                    .Where(e => e.isAvailable == true
                            && e.Date >= newRequestDate)
                    .Select(x => x.EquipmentID)
                    .FirstOrDefault();

                return equipmentId;
            }

            return 0;
        }
    }
}
