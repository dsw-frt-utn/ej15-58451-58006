using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controllers
{
    // Controlador encargado de exponer los endpoints relacionados con médicos.
    // Recibe las solicitudes HTTP y delega la gestión de datos a la abstracción IPersistence.
    public class DoctorsController : AppController
    {
        private readonly IPersistence _persistence;

        // Inyectamos IPersistence.
        // Esto evita acoplar el controlador a una implementación concreta como PersistenceInMemory.
        public DoctorsController(IPersistence persistence) 
        { 
            _persistence = persistence;
        }


        // Valida los datos requeridos antes de crear el médico.
        // Si una validación falla, se lanza ValidationException.
        // El middleware global se encarga de transformar esa excepción en un 400 Bad Request.
        [HttpPost ("doctors")]
        public async Task<IActionResult> CreateDoctor(DoctorModel.Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ValidationException("Se requiere el nombre del medico");
            }

            if (string.IsNullOrWhiteSpace(request.LicenseNumber))
            {
                throw new ValidationException("Se requiere la matricula del medico");
            }

            var speciality = _persistence.GetSpecialityById(request.SpecialityId);
            
            if (speciality is null)
            {
                throw new ValidationException("La especialidad no existe");
            }

            var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
            _persistence.SaveDoctor(doctor);

            return Created("", null);
        }

        [HttpGet("doctors")]
        public IActionResult GetDoctors()
        {
            var doctors = _persistence.GetActiveDoctors()
                                      .Select(doctor => new DoctorModel.Response(   doctor.Id,
                                                                                    doctor.Name,
                                                                                    doctor.LicenseNumber,
                                                                                    doctor.Speciality.Name
                                                                                                            ));

            return Ok(doctors);
        }

        [HttpGet("doctors/{id:guid}")]
        public IActionResult GetDoctorById(Guid id)
        {
            var doctor = _persistence.GetActiveDoctorById(id);

            if (doctor is null)
            {
                return NotFound();
            }

            var response = new DoctorModel.Response(
                doctor.Id,
                doctor.Name,
                doctor.LicenseNumber,
                doctor.Speciality.Name
            );

            return Ok(response);
        }

        [HttpDelete("doctors/{id:guid}")]
        public IActionResult DeleteDoctor(Guid id)
        {
            var doctor = _persistence.GetActiveDoctorById(id);

            if (doctor is null)
            {
                return NotFound();
            }

            doctor.Deactivate();

            return NoContent();
        }

    }
}
