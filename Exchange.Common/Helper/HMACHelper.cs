using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common.Helper
{
    public class HMACHelper
    {

        /// <summary>
        /// This is for to create the HMACSHA256 Hash Message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string GenerateHMACSHA256(string message, string secret = "")
        {
            try
            {
                secret = secret ?? "";

                var encoding = new System.Text.ASCIIEncoding();

                byte[] keyBytes = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);

                using (var hmacsha256 = new HMACSHA256())
                {
                    if (!string.IsNullOrWhiteSpace(secret))
                    {
                        hmacsha256.Key = keyBytes;
                    }

                    byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);

                    string hashString = Convert.ToBase64String(hashMessage);

                    string bitString = BitConverter.ToString(hashMessage).Replace("-", "");

                    return bitString;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("GenerateHMACSHA256 : " + ex.Message);

                throw;
            }
        }


        /// <summary>
        /// This is for to create the HMACSHA256 Hash Message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string GenerateHMACSHA1(string message, string secret)
        {
            try
            {
                secret = secret ?? "";

                var encoding = new System.Text.ASCIIEncoding();

                byte[] keyBytes = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);

                using (var hmacsha1 = new HMACSHA1(keyBytes))
                {
                    byte[] hashMessage = hmacsha1.ComputeHash(messageBytes);
                    string hashString = Convert.ToBase64String(hashMessage);
                    return hashString;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GenerateHMACSHA1 : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string GenerateHMACSHA384(string message, string secret)
        {
            try
            {
                secret = secret ?? "";

                var encoding = new System.Text.ASCIIEncoding();

                byte[] keyBytes = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);

                using (var hmacsha384 = new HMACSHA384(keyBytes))
                {
                    byte[] hashMessage = hmacsha384.ComputeHash(messageBytes);
                    string hashString = Convert.ToBase64String(hashMessage);
                    return hashString;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GenerateHMACSHA384 : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string GenerateHMACSHA512(string message, string secret)
        {
            try
            {
                secret = secret ?? "";

                var encoding = new System.Text.ASCIIEncoding();

                byte[] keyBytes = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);

                using (var hmacsha512 = new HMACSHA512(keyBytes))
                {
                    byte[] hashMessage = hmacsha512.ComputeHash(messageBytes);
                    string hashString = Convert.ToBase64String(hashMessage);
                    return hashString;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("GenerateHMACSHA512 : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string GenerateHMACMD5(string message, string secret)
        {
            try
            {
                secret = secret ?? "";

                var encoding = new System.Text.ASCIIEncoding();

                byte[] keyBytes = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);

                using (var hmacmd5 = new HMACMD5(keyBytes))
                {
                    byte[] hashMessage = hmacmd5.ComputeHash(messageBytes);
                    string hashString = Convert.ToBase64String(hashMessage);
                    return hashString;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GenerateHMACMD5 : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string GenerateHMACRIPEMD160(string message, string secret)
        {
            try
            {
                secret = secret ?? "";

                var encoding = new System.Text.ASCIIEncoding();

                byte[] keyBytes = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);

                using (var hmacripemd160 = new HMACRIPEMD160(keyBytes))
                {
                    var hashMessage = hmacripemd160.ComputeHash(messageBytes);
                    string hashString = Convert.ToBase64String(hashMessage);
                    return hashString;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("GenerateHMACRIPEMD160 : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <param name="compareToHashString"></param>
        /// <returns></returns>
        public static bool IsHMACSHA1Valid(string message, string secret, string compareToHashString)
        {
            try
            {
                secret = secret ?? "";

                var encoding = new System.Text.ASCIIEncoding();

                byte[] keyBytes = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);

                using (var hmacsha1 = new HMACSHA1(keyBytes))
                {
                    byte[] hashMessage = hmacsha1.ComputeHash(messageBytes);
                    string hashString = Convert.ToBase64String(hashMessage).ToLower();
                    bool isValid = hashString == compareToHashString.ToLower();

                    return isValid;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("IsHMACSHA1Valid : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <param name="compareToHashString"></param>
        /// <returns></returns>
        public static bool IsHMACSHA256Valid(string message, string secret, string compareToHashString)
        {
            try
            {
                secret = secret ?? "";

                Encoding encoding = new System.Text.ASCIIEncoding();

                byte[] keyBytes = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);

                using (var hmacsha256 = new HMACSHA256(keyBytes))
                {
                    byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
                    string hashString = Convert.ToBase64String(hashMessage).ToLower();
                    string dd = BitConverter.ToString(hashMessage).Replace("-", "");
                    bool isValid = hashString == dd.ToLower();

                    return isValid;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("IsHMACSHA256Valid : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <param name="compareToHashString"></param>
        /// <returns></returns>
        public static bool IsHMACSHA384Valid(string message, string secret, string compareToHashString)
        {
            try
            {
                secret = secret ?? "";

                var encoding = new System.Text.ASCIIEncoding();

                byte[] keyBytes = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);

                using (var hmacsha384 = new HMACSHA384(keyBytes))
                {
                    byte[] hashMessage = hmacsha384.ComputeHash(messageBytes);
                    string hashString = Convert.ToBase64String(hashMessage).ToLower();
                    bool isValid = hashString == compareToHashString.ToLower();

                    return isValid;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("IsHMACSHA384Valid : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <param name="compareToHashString"></param>
        /// <returns></returns>
        public static bool IsHMACSHA512Valid(string message, string secret, string compareToHashString)
        {
            try
            {
                secret = secret ?? "";

                var encoding = new System.Text.ASCIIEncoding();

                byte[] keyBytes = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);

                using (var hmacsha512 = new HMACSHA512(keyBytes))
                {
                    byte[] hashMessage = hmacsha512.ComputeHash(messageBytes);
                    string hashString = Convert.ToBase64String(hashMessage).ToLower();
                    bool isValid = hashString == compareToHashString.ToLower();

                    return isValid;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("IsHMACSHA512Valid : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <param name="compareToHashString"></param>
        /// <returns></returns>
        public static bool IsHMACRIPEMD160Valid(string message, string secret, string compareToHashString)
        {
            try
            {
                secret = secret ?? "";

                var encoding = new System.Text.ASCIIEncoding();

                byte[] keyBytes = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);

                using (var hmacripemd160 = new HMACRIPEMD160(keyBytes))
                {
                    byte[] hashMessage = hmacripemd160.ComputeHash(messageBytes);
                    string hashString = Convert.ToBase64String(hashMessage).ToLower();
                    bool isValid = hashString == compareToHashString.ToLower();

                    return isValid;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("IsHMACRIPEMD160Valid : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <param name="compareToHashString"></param>
        /// <returns></returns>
        public static bool IsHMACMD5Valid(string message, string secret, string compareToHashString)
        {
            try
            {
                secret = secret ?? "";

                var encoding = new System.Text.ASCIIEncoding();

                byte[] keyBytes = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(message);

                using (var hmacmd5 = new HMACMD5(keyBytes))
                {
                    byte[] hashMessage = hmacmd5.ComputeHash(messageBytes);
                    string hashString = Convert.ToBase64String(hashMessage).ToLower();
                    bool isValid = hashString == compareToHashString.ToLower();

                    return isValid;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("IsHMACMD5Valid : " + ex.Message);
                throw;
            }
        }

    }
}
