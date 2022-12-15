using EnpassJSONViewer.Utils;
using System;
using System.Text.Json;

namespace EnpassJSONViewer.Extensions
{
    static class JsonElementExtensions
    {
        public static string GetString(this JsonElement element, string propertyName, string def = null)
        {
            if (string.IsNullOrEmpty(propertyName) || !element.TryGetProperty(propertyName, out JsonElement found))
                return def;
            return found.GetString();
        }

        public static Guid GetGuid(this JsonElement element, string propertyName, Guid? def = null)
        {
            if (!string.IsNullOrEmpty(propertyName) &&
                element.TryGetProperty(propertyName, out JsonElement found) &&
                found.TryGetGuid(out Guid value))
                return value;
            return def ?? Guid.Empty;
        }

        public static int GetInt32(this JsonElement element, string propertyName, int def = 0)
        {
            if (!string.IsNullOrEmpty(propertyName) &&
                element.TryGetProperty(propertyName, out JsonElement found) &&
                found.TryGetInt32(out int value))
                return value;
            return def;
        }

        public static uint GetUInt32(this JsonElement element, string propertyName, uint def = 0)
        {
            if (!string.IsNullOrEmpty(propertyName) &&
                element.TryGetProperty(propertyName, out JsonElement found) &&
                found.TryGetUInt32(out uint value))
                return value;
            return def;
        }

        public static DateTime GetUnixTimestamp(this JsonElement element, string propertyName, DateTime? def = null)
        {
            if (!string.IsNullOrEmpty(propertyName) &&
                element.TryGetProperty(propertyName, out JsonElement found) &&
                found.TryGetInt64(out long value))
                return DateTimeUtils.GetUnixTimeStamp(value);
            return def ?? DateTime.MinValue;
        }
    }
}
