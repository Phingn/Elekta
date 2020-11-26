using Elekta.Code.Challenge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Interfaces
{
    public interface IPatientRespository
    {
        string GetPatientEmail(int patientID);
        int AddPatient(Patient patient);
    }
}
