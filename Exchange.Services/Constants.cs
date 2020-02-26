using System.Net;

namespace Exchange.Services
{
   internal class Constants
    {
        public static string TwoFactorSecret
        {
            get
            {
                return "858100416504-afcr1kb9jsqi6nu8aql2mr34l1ts0e34.apps.googleusercontent.com";
            }
        }

        public static string GetAPIResponse(string url)
        {
            return new WebClient().DownloadString("https://api.bittrex.com/v3/" + url);
        }

        public static string GetAPIResponse2(string url)
        {
            return new WebClient().DownloadString(url);
        }

        public static string GetAPIResponseV1(string url)
        {
            return new WebClient().DownloadString("https://bittrex.com/api/v1.1/public/" + url);
        }
        //public static string BApiKey = "e19e7fb8ed8b4fcc9c9804c67075c80b";
        //public static string BSecretKey = "c8904281316b415e82dcea0737cc7562";
        /// <summary>
        /// New Keys
        /// </summary>
        public static string BApiKey = "";
        public static string BSecretKey = "";
        public static string Domain
        {
            get
            {
                //local
                //return "http://localhost:21217";
                //live
                return "https://nanopips.com";
            }
        }
        public static string AdminDomain
        {
            get
            {
                //local
                //return "http://localhost:30779";
                //live
                return "https://nanopips.com";
            }
        }
    }
}
