using Newtonsoft.Json;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GDMLib
{
    public static class Serializer<T>
    {
        /// <summary>
        /// Converts an object to a serialized XML string
        /// </summary>
        /// <param name="obj">Any Object</param>
        /// <returns>Returns the XML string that it serialized from the object</returns>
        public static string ObjectToXMLString(T obj)
        {
            var stringwriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stringwriter, obj);
            return stringwriter.ToString();
        }

        /// <summary>
        /// Converts an XML string into an object
        /// </summary>
        /// <param name="xmlText">XML string that you want converted</param>
        /// <returns>Returns the Object that it constructed out of the XML string</returns>
        public static T XMLStringToObject(string xmlText)
        {
            var stringReader = new StringReader(xmlText);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader);
        }

        /// <summary>
        /// Writes an object to a file using XML Serialization
        /// </summary>
        /// <param name="obj">Object that you want serialized</param>
        /// <param name="path">Path to save the file</param>
        public static void WriteToXMLFile(T obj, string path)
        {
            XmlSerializer writer = new XmlSerializer(typeof(T));
            FileStream file = File.Create(path);
            writer.Serialize(file, obj);
            file.Close();
        }

        /// <summary>
        /// Loads a file and converts it into an object
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>The object that it constructed from the XML file</returns>
        public static T LoadFromXMLFile(string path)
        {
            if (!File.Exists(path))
                return default(T);
            XmlSerializer reader = new XmlSerializer(typeof(T));
            StreamReader file = new StreamReader(path);
            T obj = (T)reader.Deserialize(file);
            file.Close();
            return obj;
        }

        /// <summary>
        /// Converts an object to a serialized JSON string
        /// </summary>
        /// <param name="obj">Any Object</param>
        /// <returns>Returns the JSON string that it serialized from the object</returns>
        public static string ObjectToJSONString(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Converts a JSON string into an object
        /// </summary>
        /// <param name="jsonText">JSON string that you want converted</param>
        /// <returns>Returns the Object that it constructed out of the JSON string</returns>
        public static T JSONStringToObject(string jsonText)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            return JsonConvert.DeserializeObject<T>(jsonText, settings);
        }

        /// <summary>
        /// Writes an object to a file using JSON Serialization
        /// </summary>
        /// <param name="obj">Object that you want serialized</param>
        /// <param name="path">Path to save the file</param>
        public static void WriteToJSONFile(T obj, string path)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Include;
            serializer.TypeNameHandling = TypeNameHandling.All;

            FileSystemHandler.CreateDirectory(Path.GetDirectoryName(path));

            using (StreamWriter sw = new StreamWriter(path))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.Serialize(writer, obj);
                }
            }
        }

        /// <summary>
        /// Loads a file and converts it into an object
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>The object that it constructed from the JSON file</returns>
        public static T LoadFromJSONFile(string path)
        {
            return JSONStringToObject(File.ReadAllText(path));
        }
    }
}
