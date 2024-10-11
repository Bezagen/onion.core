using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using onion.core.src.Models.Forge;

namespace onion.core.src.Handlers
{
    public class TOMLParser
    {
        // key = "value"
        // intKey = value
        // #comment
        // key = ''' value1 value2 ''' // Mulit-line string
        // key = true/false
        // array = [ "value1", "value2" ]
        //
        // [table]
        // key1 = value
        // key2 = value
        //

        public Dictionary<string, object> GetValuesFromFile(string tomlText)
        {
            string[] tomlLines = tomlText.Split('\n');
            Dictionary<string, object> tomlResult = new();

            return tomlResult;
        }

        public Dictionary<string, List<Models.Forge.KeyValuePair>> GetTables(string fileText)
        {
            if (fileText.Length <= 2)
                throw new ArgumentException("Text not found");
            
            Dictionary<string, List<Models.Forge.KeyValuePair>> tablesDictionary = new();

            string[] lines = fileText.Split("\n");

            // Num Table
            // Value Lines numbers
            Dictionary<int, List<int>> foundedTables = new();

            // Search Tables
            bool tableFound = false;
            int currentTableNumber = 1;
            
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("[[mods]]"))
                {
                    lines[i] = " ";
                    Debug.WriteLine(lines[i]);
                }
                switch (tableFound)
                {
                    case true:
                        if (lines[i].StartsWith("[["))
                        {
                            currentTableNumber++;
                            foundedTables.Add(currentTableNumber, new List<int> { i });
                            continue;
                        }
                        else if (lines[i].Length <= 2)
                        {
                            tableFound = false;
                            continue;
                        }

                        if (!foundedTables.ContainsKey(currentTableNumber))
                            foundedTables.Add(currentTableNumber, new List<int> { i });
                        else
                            foundedTables[currentTableNumber].Add(i);

                        break;

                    case false:
                        if (lines[i].StartsWith("[["))
                        {
                            tableFound = true;
                        }
                        break;
                }
            }

            // GetValues
            for (int i = 0; i < foundedTables.Count; i++)
            {
                Debug.WriteLine(foundedTables.ElementAt(i).Value.Count + " + " + i);
                for (int j = 0; j < foundedTables.ElementAt(i).Value.Count; j++)
                {
                    var keyValuePair = GetKeyValuePair(lines[foundedTables.ElementAt(i).Value[j]]);
                
                    if (keyValuePair != null)
                    {
                        if (!tablesDictionary.ContainsKey("Dependence" + i))
                            tablesDictionary.Add("Dependence" + i, new List<Models.Forge.KeyValuePair> { keyValuePair });
                        else
                            tablesDictionary["Dependence" + i].Add(keyValuePair);
                    }
                }
            }

            return tablesDictionary;
        }

        public List<Models.Forge.KeyValuePair> GetKeyValuePairs(string line)
        {
            List<Models.Forge.KeyValuePair> keyValuePairs = new();

            if (line.Contains('='))
            {
                var keyValue = line.Split('=', 2);
                
                string key = keyValue[0];
                string value = keyValue[1];

                value.Replace('\"', ' ');

                Models.Forge.KeyValuePair keyValuePair = new(key, value);

                keyValuePairs.Add(keyValuePair);
            }

            return keyValuePairs;
        }

        public Models.Forge.KeyValuePair? GetKeyValuePair(string line)
        {
            Models.Forge.KeyValuePair keyValuePair;
            
            if (line.Contains('='))
            {
                var keyValue = line.Split('=', 2);

                string key = keyValue[0];
                string value = keyValue[1];

                value.Replace('\"', ' ');

                keyValuePair = new(key, value);

                return keyValuePair;
            }

            return null;
        }
    }
}