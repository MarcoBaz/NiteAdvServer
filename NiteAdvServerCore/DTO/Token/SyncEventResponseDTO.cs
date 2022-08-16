using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.DTO.Token
{
    public  class SyncEventResponseDTO
    {
        public SyncEventResponseDTO()
        {
            EventDTOList = new List<EventDTO>();
            ActionError = String.Empty;
        }
        public List<EventDTO> EventDTOList { get; set; }
        public int ActionResult { get; set; }
        public string ActionError { get; set; }
       
    }
}
