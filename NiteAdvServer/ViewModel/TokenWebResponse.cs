using System;

namespace NiteAdvServer.ViewModel
{
    public class TokenWebResponse
    {
        public TokenWebResponse()
        {
        }
        public TokenWebResponse(string errorMessage, string operation = "")
        {
            Operation = operation;
            Error = errorMessage;
            Data = null;
        }
        public TokenWebResponse(object data, string operation = "")
        {
            Operation = operation;
            Error = String.Empty;
            Data = data;
        }
        public string Operation { get; set; }
        public string Error { get; set; }
        public object Data { get; set; }
    }
}
