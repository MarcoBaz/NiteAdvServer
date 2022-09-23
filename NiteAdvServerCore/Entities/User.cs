using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gremlin.Net.Process.Traversal;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Layouts;
using NiteAdvServerCore.Entities;
using NiteAdvServerCore.Managers;
using NiteAdvServerCore.Util;
using Org.BouncyCastle.Crypto;
using static System.Net.WebRequestMethods;
using Twilio.TwiML.Messaging;
using Twilio.TwiML.Voice;

namespace NiteAdvServerCore.Entities
{
    public class User:VertexEntity
    {
        public User() : base("", "user")
        { }
        public User(string id) : base(id, "user")
        {
        }
       
        public string? Name { get; set; }
        public string? Surename { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public double BirthDate { get; set; }
        public bool Disabled { get; set; }
        public bool UserActivated { get; set; }
        public double RegistrationDate { get; set; }
        public string? UserImageLink { get; set; }
        public string? PhoneNumber { get; set; }
        public bool FromFacebook { get; set; }
        public bool IsCompany { get; set; }
        public long IdNation { get; set; }
        [Ignore]
        public bool IsAdmin { get => ServerUtil.IsUserAdmin(Email); }
        //public DateTime BirthDateDate { get => ServerUtil.GetDateTimeFromUnixFormat(this.BirthDate); }
        //public UserProperties properties { get; set; }
    }
}


