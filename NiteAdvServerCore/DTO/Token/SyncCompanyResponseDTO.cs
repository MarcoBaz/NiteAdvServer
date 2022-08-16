using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.DTO.Token
{
    public class SyncCompanyResponseDTO
    {
        public SyncCompanyResponseDTO()
        {
            CompanyDTOList = new List<CompanyDTO>();
            ActionError = String.Empty;
        }
        public List<CompanyDTO> CompanyDTOList { get; set; }
        public int ActionResult { get; set; }
        public string ActionError { get; set; }
    }
}
