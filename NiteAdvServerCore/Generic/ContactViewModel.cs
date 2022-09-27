using System;
namespace NiteAdvServerCore.Generic
{
    public class ContactViewModel
    {
        public ContactViewModel()
        {
        }
        public string Email { get; set; }
        public string Text { get; set; }
        public bool IsOwner { get; set; }
    }
}

