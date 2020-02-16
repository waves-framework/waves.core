using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Fluid.Core.Utils.Serialization
{
    public static class Binary
    {
        /// <summary>
        ///     Записывает класс в файл
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="objectToWrite"></param>
        /// <param name="append"></param>
        public static void WriteToFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create);
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }

        /// <summary>
        ///     Читает класс из файла
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T ReadFile<T>(string filePath)
        {
            using Stream stream = File.Open(filePath, FileMode.Open);
            var binaryFormatter = new BinaryFormatter();
            return (T) binaryFormatter.Deserialize(stream);
        }
    }
}