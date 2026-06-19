using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dsw2026Ej15.Data
{
    // Implementación en memoria de la persistencia.
    // Mantiene los datos mientras la aplicación se encuentra en ejecución.
    public class PersistenceInMemory : IPersistence
    {
        private List<Speciality> _specialities = [];
        private List<Doctor> _doctors = [];

        public PersistenceInMemory()
        {
            LoadSpecialities();
        }

        public Doctor? GetActiveDoctorById(Guid id)
        {
            return _doctors.SingleOrDefault(d => d.Id == id && d.IsActive);
        }

        public Speciality? GetSpecialityById(Guid id)
        {
            return _specialities.SingleOrDefault(e => e.Id == id);
        }

        public IReadOnlyCollection<Doctor> GetActiveDoctors()
        {
            return _doctors.Where(d => d.IsActive).ToList().AsReadOnly();
        }

        // Carga inicial de especialidades desde el archivo JSON.
        public void LoadSpecialities()
        {
            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Sources", "specialities.json");
                var json = File.ReadAllText(jsonPath);
                var specialities = JsonSerializer.Deserialize<List<SpecialityDto>>(json,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? [];
                _specialities = [.. specialities.Select(s =>  new Speciality(s.Name, s.Description, s.Id))];
            }
            catch (Exception)
            {

            }
        }

        public void SaveDoctor(Doctor doctor)
        {
            _doctors.Add(doctor);
        }

     

    }
}
