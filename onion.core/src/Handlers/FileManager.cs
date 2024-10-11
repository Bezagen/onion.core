using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using SharpCompress.Readers;
using SharpCompress;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives;

namespace onion.core.src.Handlers
{
    //              (c) Alexandr Yaz
    //      Класс логики взаимодействия с файлами

    public class FileManager
    {
        #region GetMinecraftModsFolderPath
        /// <summary>
        /// Получает путь до папки mods
        /// </summary>
        /// <returns>Возвращает путь до папки mods</returns>
        /// <exception cref="FileNotFoundException">Возникает если папка mods не была найдена</exception>
#endregion
        public string GetMinecraftModsFolderPath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string modsFolderPath = Path.Combine(appDataPath, ".minecraft", "mods");

            if (!Directory.Exists(modsFolderPath))
            {
                throw new FileNotFoundException("Папка mods не существует");
            }

            return modsFolderPath;
        }

        #region SearchFiles
        /// <summary>
        /// Ищет файлы формата .jar в папке mods
        /// </summary>
        /// <param name="modsFolerPath">Путь до папки mods</param>
        /// <returns>Возвращает коллекцию List string</returns>
#endregion
        public List<string> SearchFiles(string modsFolerPath)
        {
            List<string> modsPaths = Directory.GetFiles(modsFolerPath + @"\", "*.jar").ToList();

            return modsPaths;
        }

        #region GetModProperties
        /// <summary>
        /// Получает параметры модификации
        /// </summary>
        /// <param name="modPath">Путь до модификации</param>
        /// <returns>Возвращает словарь параметров модификации</returns>
        #endregion
        public Dictionary<string, List<Models.Forge.KeyValuePair>> GetModProperties(string modPath)
        {
            string content = "";
            Dictionary<string, List<Models.Forge.KeyValuePair>> result = new();

            switch (DetermineLoaderType(modPath))
            {
                case "forge":
                    using (var archive = ArchiveFactory.Open(modPath))
                    {
                        var entry = archive.Entries.FirstOrDefault(e => e.Key == @"META-INF/mods.toml");
                        if (entry != null)
                        {
                            using (var stream = entry.OpenEntryStream())
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    content = reader.ReadToEnd();
                                    TOMLParser parser = new();
                                    
                                    // Get result

                                }
                            }
                        }
                    }
                    break;
                case "fabric":

                    break;
                default:
                    break;
            }

            

            return result;
            //TOMLParser parser = new TOMLParser();
            //return parser.GetTables(content);
        }

        #region
        /// <summary>
        /// Определяет загрузичк модификации
        /// </summary>
        /// <param name="modPath">Путь до модификации</param>
        /// <returns>Возвращает название загрузчика</returns>
        #endregion
        public string DetermineLoaderType(string modPath)
        {
            using (var archive = ArchiveFactory.Open(modPath))
            {
                var mod = archive.Entries.FirstOrDefault(e => e.Key == @"META-INF/mods.toml");
                
                if (mod != null)
                    return "forge";
                
                mod = archive.Entries.FirstOrDefault(e => e.Key == @"fabric.mod.json");

                if (mod != null)
                    return "fabric";
            }
            return "unknown";
        }
    }


    /*
     * 
        ⡏⠉⠉⠉⠉⠉⠉⠋⠉⠉⠉⠉⠉⠉⠋⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠙⠉⠉⠉⠹
        ⡇⢸⣿⡟⠛⢿⣷⠀⢸⣿⡟⠛⢿⣷⡄⢸⣿⡇⠀⢸⣿⡇⢸⣿⡇⠀⢸⣿⡇⠀
        ⡇⢸⣿⣧⣤⣾⠿⠀⢸⣿⣇⣀⣸⡿⠃⢸⣿⡇⠀⢸⣿⡇⢸⣿⣇⣀⣸⣿⡇⠀
        ⡇⢸⣿⡏⠉⢹⣿⡆⢸⣿⡟⠛⢻⣷⡄⢸⣿⡇⠀⢸⣿⡇⢸⣿⡏⠉⢹⣿⡇⠀
        ⡇⢸⣿⣧⣤⣼⡿⠃⢸⣿⡇⠀⢸⣿⡇⠸⣿⣧⣤⣼⡿⠁⢸⣿⡇⠀⢸⣿⡇⠀
        ⣇⣀⣀⣀⣀⣀⣀⣄⣀⣀⣀⣀⣀⣀⣀⣠⣀⡈⠉⣁⣀⣄⣀⣀⣀⣠⣀⣀⣀⣰
        ⣇⣿⠘⣿⣿⣿⡿⡿⣟⣟⢟⢟⢝⠵⡝⣿⡿⢂⣼⣿⣷⣌⠩⡫⡻⣝⠹⢿⣿⣷
        ⡆⣿⣆⠱⣝⡵⣝⢅⠙⣿⢕⢕⢕⢕⢝⣥⢒⠅⣿⣿⣿⡿⣳⣌⠪⡪⣡⢑⢝⣇
        ⡆⣿⣿⣦⠹⣳⣳⣕⢅⠈⢗⢕⢕⢕⢕⢕⢈⢆⠟⠋⠉⠁⠉⠉⠁⠈⠼⢐⢕⢽
        ⡗⢰⣶⣶⣦⣝⢝⢕⢕⠅⡆⢕⢕⢕⢕⢕⣴⠏⣠⡶⠛⡉⡉⡛⢶⣦⡀⠐⣕⢕
        ⡝⡄⢻⢟⣿⣿⣷⣕⣕⣅⣿⣔⣕⣵⣵⣿⣿⢠⣿⢠⣮⡈⣌⠨⠅⠹⣷⡀⢱⢕
        ⡝⡵⠟⠈⢀⣀⣀⡀⠉⢿⣿⣿⣿⣿⣿⣿⣿⣼⣿⢈⡋⠴⢿⡟⣡⡇⣿⡇⡀⢕
        ⡝⠁⣠⣾⠟⡉⡉⡉⠻⣦⣻⣿⣿⣿⣿⣿⣿⣿⣿⣧⠸⣿⣦⣥⣿⡇⡿⣰⢗⢄
        ⠁⢰⣿⡏⣴⣌⠈⣌⠡⠈⢻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣬⣉⣉⣁⣄⢖⢕⢕⢕
        ⡀⢻⣿⡇⢙⠁⠴⢿⡟⣡⡆⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣵⣵⣿
        ⡻⣄⣻⣿⣌⠘⢿⣷⣥⣿⠇⣿⣿⣿⣿⣿⣿⠛⠻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
        ⣷⢄⠻⣿⣟⠿⠦⠍⠉⣡⣾⣿⣿⣿⣿⣿⣿⢸⣿⣦⠙⣿⣿⣿⣿⣿⣿⣿⣿⠟
        ⡕⡑⣑⣈⣻⢗⢟⢞⢝⣻⣿⣿⣿⣿⣿⣿⣿⠸⣿⠿⠃⣿⣿⣿⣿⣿⣿⡿⠁⣠
        ⡝⡵⡈⢟⢕⢕⢕⢕⣵⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣶⣿⣿⣿⣿⣿⠿⠋⣀⣈⠙
        ⡝⡵⡕⡀⠑⠳⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠛⢉⡠⡲⡫⡪⡪⡣
     *
     */
}
