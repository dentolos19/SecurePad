using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SecurePad.Core
{

    [Serializable]
    public class Configuration
    {

        private static readonly string Source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SecurePad.cfg");
        private static readonly BinaryFormatter Formatter = new BinaryFormatter();

        public string Seed = "S3CUR3P4D";

        public void Save()
        {
            var stream = new FileStream(Source, FileMode.Create);
            Formatter.Serialize(stream, this);
            stream.Close();
        }

        public static Configuration Load()
        {
            var result = new Configuration();
            if (!File.Exists(Source))
                return result;
            var stream = new FileStream(Source, FileMode.Open);
            result = Formatter.Deserialize(stream) as Configuration;
            stream.Close();
            return result;
        }

    }

}