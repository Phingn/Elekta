using System;

namespace Elekta.Code.Challenge.Api.External.Api
{
    public interface IEquipmentApi
    {
        int GetAvailableEquipment(DateTime requestDate);
        int GetAvailableEquipment(DateTime requestDate, DateTime newRequestDate);
        bool BookingRequestConfirm(int equipmentID, DateTime requestDate);
        bool BookingCancelConfirm(int equipmentID, DateTime requestDate);
    }
}