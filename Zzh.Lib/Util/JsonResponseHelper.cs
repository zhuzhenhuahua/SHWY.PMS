using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Zzh.Lib.Util
{
   public class JsonResponseHelper
    {
        public static HttpResponseMessage HttpRMtoJson(object obj, HttpStatusCode statusCode, ECustomStatus customStatus)
        {
            string str;
            ResponseJsonMessage rjm = new ResponseJsonMessage(customStatus.ToString(), obj);
            str = JsonConvert.SerializeObject(rjm);
            HttpResponseMessage result = new HttpResponseMessage() { StatusCode = statusCode, Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
        [Serializable]
        public class ResponseJsonMessage
        {
            public string CustomStatus = "";
            public object Message = null;
            public ResponseJsonMessage(string customStatus, object message)
            {
                CustomStatus = customStatus;
                Message = message;
            }
        }
        public enum ECustomStatus
        {
            InvalidArguments,
            Forbidden,
            Inactive,
            Success,
            WrongPassowrd,
            NotFound,
            AccountExist,
            Fail,
            ErrorValidationCode,
            NoValidationCode,
        }
    }
}
