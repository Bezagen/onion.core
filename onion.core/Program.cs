using System;
using System.Reflection;
using System.Text;
using onion.core.Handlers;
using onion.core.Models.Forge;

namespace onion.core
{
    internal class Program
    {
        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            FileManager fileManager = new FileManager();
            string path = fileManager.GetMinecraftModsFolderPath();

            Console.WriteLine("Mods in folder\n\n//////////\n");

            foreach (var mod in fileManager.SearchFiles(path))
            {
                Console.WriteLine($"{Path.GetFileName(mod)}");
            }

            List<string> temp = fileManager.SearchFiles(path);
            Console.WriteLine(temp[0]);
            
            //Console.WriteLine($"{fileManager.DetermineLoaderType(temp[0])}");

            var result = fileManager.GetModProperties(temp[0]);

            foreach (var property in result)
            {
                Console.WriteLine($"{property.Key} = {property.Value}");
            }
            //for (int i = 0; i < result.Count; i++)
            //{
            //    Console.WriteLine($"{result.ElementAt(i)}");
            //    for (int j = 0; j < result.ElementAt(i).Value.Count; j++)
            //    {
            //        Console.WriteLine($"{result.ElementAt(i).Value[j].Value}");
            //    }
            //}
        }
    }
}
