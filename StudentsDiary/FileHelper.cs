using System.IO;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public class FileHelper<T> where T:new()  
    {
        private string _filePath;
        public FileHelper(string filePath)
        {
            _filePath = filePath;
        }

        public void SerializeToFile(T students)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var writer = new StreamWriter(_filePath))
            {
                serializer.Serialize(writer, students);
                writer.Close();
            }
        }

        public T Deserialize()
        {
            if (!File.Exists(_filePath))
                return new T();

            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StreamReader(_filePath))
            {
                var students = (T)serializer.Deserialize(reader);
                reader.Close();
                return students;
            }
        }
    }
}
