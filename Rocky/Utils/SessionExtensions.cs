using Microsoft.AspNetCore.Http;
using Rocky.Utils;
using System.Text.Json;

namespace Rocky.Utils
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.Get(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}