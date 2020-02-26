using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public class AddressHelper
    {
        public static bool CryptoAddressValidator(string address, string currency)
        {
            try
            {
                // need ?
                var obj = new
                {
                    Address = address,
                    Currency = currency
                };
                var mJson = JsonConvert.SerializeObject(obj);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://35.237.138.131:5000/api/checkaddress");
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    streamWriter.Write(mJson);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    return Convert.ToBoolean(result);
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
