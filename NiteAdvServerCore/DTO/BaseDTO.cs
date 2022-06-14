using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.DTO
{
    public class EntityDTO : BaseDTO
    {
        // public int Id { get; set; }
        public int OriginalId { get; set; }
    }

    public class BaseDTO
    {
        public System.DateTime LastSyncDate { get; set; }
        public int ActionResult { get; set; }
        public string ActionError { get; set; }
    }

    public class PagingDTO<T>
    {
        public int Page { get; set; }
        public int PageCount { get; set; }
        public T Data { get; set; }

        public string BuildJsonResponsePaginated(T objectToConvert, int page, int pageCount)
        {
            try
            {

                Page = page;
                PageCount = pageCount;
                Data = objectToConvert;
                return JsonConvert.SerializeObject(this);
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

    }
}
