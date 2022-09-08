using NiteAdvServerCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Generic
{
    public class UserResponse
    {
        public UserResponse()
        {
            UserList = new List<User>();
        }
       public List<User> UserList { get; set; }
       public int ItemsCount { get; set; }
    }
}
