using System.IO;
using System.Xml.Serialization;

namespace Fluid.Core.Utils.Serialization
{
    public static class Xml
    {
        /// <summary>
        ///     Записывает класс в файл.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="objectToWrite"></param>
        /// <param name="append"></param>
        public static void WriteToFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                writer?.Close();
            }
        }

        /// <summary>
        ///     Читает класс из файла.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T ReadFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T) serializer.Deserialize(reader);
            }
            finally
            {
                reader?.Close();
            }
        }
    }
}