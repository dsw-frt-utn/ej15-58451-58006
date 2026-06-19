namespace Dsw2026Ej15.Api.Models
{
    public record DoctorModel
    {
        //este va a ser el record que represente la estructura que quiero pedir
        //me permite agrupar la estructura de los distintos records
        public record Request(string Name, string LicenseNumber, Guid SpecialityId);

        public record Response(Guid Id, string Name, string LicenseNumber, string SpecialityName);

    }
}
