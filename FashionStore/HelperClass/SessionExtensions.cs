using System.Text.Json;

namespace FashionStore.HelperClass
{
    //public static class SessionExtensions
    //{
    //    public static void SetObjectAsJson(this ISession session, string key, object value)
    //    {
    //        session.SetString(key, JsonSerializer.Serialize(value));
    //    }

    //    public static T GetObjectFromJson<T>(this ISession session, string key)
    //    {
    //        var value = session.GetString(key);
    //        return value == null ? default :
    //            JsonSerializer.Deserialize<T>(value);
    //    }
    //    public static void SetInt32(this ISession session, string key, int value)
    //    {
    //        session.SetString(key, value.ToString());
    //    }

    //    public static int? GetInt32(this ISession session, string key)
    //    {
    //        var value = session.GetString(key);
    //        if (int.TryParse(value, out var result))
    //        {
    //            return result;
    //        }
    //        return null;
    //    }
    //}


    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static void SetInt32(this ISession session, string key, int value)
        {
            session.SetString(key, value.ToString());
        }

        public static int? GetInt32(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (int.TryParse(value, out var result))
            {
                return result;
            }
            return null;
        }
    }

}


