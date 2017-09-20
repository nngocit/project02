using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebComment.Commons
{
    public class JsonHelper
    {
        public static T DeserializeObject<T>(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex)
            {
                return default(T);
            }

        }


        public static object DeserializeObject(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject(jsonString);
            }
            catch (Exception ex)
            {
                return default(object);
            }

        }

        public static string SerializeObject(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
