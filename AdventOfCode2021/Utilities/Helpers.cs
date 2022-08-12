﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public static class Helpers
    {
        public static string[] GetInput(string fileName)
        {
            var path = Path.Combine(Config.Input, fileName);
           
            return File.ReadLines(path).ToArray();
        }

        public static object Deserialize(string json)
        {
            return ToObject(JToken.Parse(json));
        }

        public static object ToObject(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token
                        .Children<JProperty>()
                        .ToDictionary(prop => prop.Name, prop => ToObject(prop.Value));

                case JTokenType.Array:
                    return token.Select(ToObject).ToList();

                default:
                    return ((JValue)token).Value;
            }
        }
    }
}
