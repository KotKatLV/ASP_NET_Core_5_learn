using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Rocky.Models
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.Set(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.Get(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}