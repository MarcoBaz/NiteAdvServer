using NiteAdvServerCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Generic
{
    public class CompanyResponse
    {
        public CompanyResponse()
        {
            CompanyList = new List<Company>();
        }
        public List<Company> CompanyList { get; set; }
        public int ItemsCount { get; set; }
    }
}
