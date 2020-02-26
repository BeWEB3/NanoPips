using System.Web;

namespace Exchange.Common
{
   public static class SessionItems
    {
        public static object Get(SessionKey key)
        {
           return HttpContext.Current.Session[key.ToString()];
        }
        public static void Add(SessionKey key, object value)
        {
            HttpContext.Current.Session.Add(key.ToString(), value);
        }
        public static void RemoveAll()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.RemoveAll();
        }
        public static void Remove(SessionKey key)
        {
            HttpContext.Current.Session.Remove(key.ToString());
        }

    }
}
