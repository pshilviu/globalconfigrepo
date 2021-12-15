using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace ORG.HttpClient.UnitTests.Extensions
{
    public class TestJson
    {
        public string? JsonString { get; set; }

        public int JsonNumber { get; set; }

        public JsonObject? JsonObject { get; set; }

        public DateTime JsonSerializedDate { get; set; }

        public object? JsonNullObject { get; set; }

        public double JsonNumberWithDp { get; set; }

        public bool JsonBool { get; set; }

        public List<string>? JsonArray { get; set; }

        public static TestJson ExampleTestJson()
        {
            return new()
            {
                JsonString = "Tom Cruise",
                JsonNumber = 56,
                JsonObject = new JsonObject { JsonObjectName = "Cool Object" },
                JsonSerializedDate = DateTime.Parse("2019-07-26T22:59:57.0000000+01:00", CultureInfo.InvariantCulture),
                JsonNullObject = null,
                JsonNumberWithDp = 67.5,
                JsonBool = true,
                JsonArray = new List<string> { "arrayItem1", "arrayItem2", "arrayItem3" }
            };
        }

        public static string LoadTestJson()
        {
            var json = File.ReadAllText("Extensions/TestJson.json");

            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            var jsonElement = JsonSerializer.Deserialize<TestJson>(json, options);

            return JsonSerializer.Serialize(jsonElement, options);
        }
    }
}