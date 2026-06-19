using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controllers
{
    //Este "Controller" para una web api no es la mejor alternativa
    //xq se usa mas para otro tipos de aplicacions (blazor, razor, para estos sirve x los metodos de controler)
    //public class DoctorsController : Controller

    public class DoctorsController : AppController
    {
        //Intentemos hacer los endpointes asincronos que devuelvan un IActionRsulet
        //a
        //La api debe devolver implicitamente un codigo de estado
        //pido -> no body
        //envio -> body
        private readonly IPersistence _persistence;
        
        public DoctorsController(IPersistence persistence) 
        { 
            _persistence = persistence;
        }

        [HttpPost ("doctors")]
        public async Task<IActionResult> CreateDoctor(DoctorModel.Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.LicenseNumber))
            {
                return BadRequest("El nombre y la matricula son requeridos.");
            }

            var speciality = _persistence.GetSpecialityById(request.SpecialityId);
            if (speciality is null)
            {
                return BadRequest("Especialidad no existe");
            }

            var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
            _persistence.SaveDoctor(doctor);

            return Created();
        }

    }
}
