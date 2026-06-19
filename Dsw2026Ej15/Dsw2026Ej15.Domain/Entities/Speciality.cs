using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2026Ej15.Domain.Entities
{
    public class Speciality : BaseEntity
    {
        public string Name { get; init; } 

        public string Description { get; init; }

        public Speciality(string name, string description, Guid? id = null): base(id)
        {
            Name = name;
            Description = description;
        }
    }
}
