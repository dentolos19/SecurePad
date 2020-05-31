using System;
using System.IO;
using System.Xml.Serialization;

namespace SecurePad.Core
{

    [Serializable]
    public class Configuration
    {

        private static readonly string Source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SecurePad.cfg");
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Configuration));
        public string Accent = "Cobalt";

        public bool IsDarkMode = true;
        public string Seed = Utilities.GetUniqueCode();

        public void Save()
        {
            var temp = Seed;
            Seed = Utilities.ToHexString(Seed);
            var stream = new StreamWriter(Source);
            Serializer.Serialize(stream, this);
            stream.Close();
            Seed = temp;
        }

        public static Configuration Load()
        {
            var result = new Configuration();
            if (!File.Exists(Source))
                return result;
            var stream = new StreamReader(Source);
            result = Serializer.Deserialize(stream) as Configuration;
            stream.Close();
            result.Seed = Utilities.FromHexString(result.Seed);
            return result;
        }

    }

}