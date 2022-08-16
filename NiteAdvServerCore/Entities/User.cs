using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Entities
{
    public class User:VertexEntity
    {
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double BirthDate { get; set; }
        public bool Disabled { get; set; }
        public bool UserActivated { get; set; }
        public double RegistrationDate { get; set; }
        public string UserImageLink { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsCompany { get; set; }
        public long IdNation { get; set; }
        public override string label { get => "user"; }
    }
}
