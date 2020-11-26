using Elekta.Code.Challenge.Api.Data;
using Elekta.Code.Challenge.Api.Interfaces;
using Elekta.Code.Challenge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elekta.Code.Challenge.Api.Repository
{
    public class PatientRepositoryService : IPatientRespository
    {
        private readonly DataContext _context;
        public PatientRepositoryService(DataContext context)
        {
            _context = context;
        }

        public string GetPatientEmail(int patientID)
        {
            return _context.Patients
                .Where(p => p.PatientID == patientID)
                .Select(x => x.Email)
                .FirstOrDefault();
        }

        public int AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();

            return patient.PatientID;
        }
    }
}
