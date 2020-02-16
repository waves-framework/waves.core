using System.IO;
using Newtonsoft.Json;

namespace Fluid.Core.Utils.Serialization
{
    public static class Json 
    {
        /// <summary>
        ///     Записывает класс в файл.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="objectToWrite"></param>
        /// <param name="append"></param>
        public static void WriteToFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            var json = JsonConvert.SerializeObject(
                objectToWrite,
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented
                });

            File.WriteAllText(filePath, json);
        }

        /// <summary>
        ///     Читает класс из файла.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T ReadFile<T>(string filePath)
        {
            var json = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            });
        }
    }
}
