using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FestivalDataApi.Utilities.Helpers
{
    public static class JsonSerialisation
    {
        /// <summary>
        /// Converts the data source of <see cref="TSource"/> type into Json
        /// </summary>
        /// <param name="typedData">Data source of <see cref="IEnumerable{TSource}"/> type</param>
        /// <returns>Returns a Json string</returns>
        public static async Task<string> ToJson<TSource>(this IEnumerable<TSource> typedData)
        {
            var jsonContent = await Task.Run(() =>
            {
                if (typedData == null)
                {
                    return string.Empty;
                }

                var content = JsonSerializer.Serialize(typedData);

                return content;
            });

            return jsonContent;
        }

        /// <summary>
        /// Generic method to get typed data from a Json data file
        /// </summary>
        /// <typeparam name="TSource">Deserialised typed to be used</typeparam>
        /// <returns>Returns an <see cref="IEnumerable{TSource}"/></returns>
        public static async Task<IEnumerable<TSource>> FromJson<TSource>(this string rawData)
        {
            var typedContent = await Task.Run(() =>
            {
                if (!IsJson(rawData))
                {
                    throw new JsonException("Invalid Json content in file");
                }

                var options = new JsonSerializerOptions();
                options.PropertyNamingPolicy = null;
                options.PropertyNameCaseInsensitive = true;
                options.IgnoreNullValues = true;
                options.Converters.Add(new JsonStringEnumConverter());

                // Deserialise the JSON content from the string data into an enumerable list.
                var content = JsonSerializer.Deserialize<IEnumerable<TSource>>(rawData, options);

                return content;
            });

            return typedContent;
        }

        /// <summary>
        /// This will check if a string is Json formatted string
        /// </summary>
        /// <param name="input">String to validate</param>
        /// <returns></returns>
        private static bool IsJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            input = input.Trim();

            bool IsWellFormed()
            {
                try
                {
                    JToken.Parse(input);
                }
                catch
                {
                    return false;
                }

                return true;
            }

            return ((input.StartsWith("{") && input.EndsWith("}"))
                    || (input.StartsWith("[") && input.EndsWith("]")))
                   && IsWellFormed();
        }
    }
}
