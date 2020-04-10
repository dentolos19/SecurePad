using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SecurePad.Core
{

    [Serializable]
    public class Package
    {

        private static readonly string Temp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SecurePad.tmp");
        private static readonly BinaryFormatter Formatter = new BinaryFormatter();

        private readonly string _password;

        public string Content { get; set; }

        public Package(string password)
        {
            _password = password;
        }

        public bool Verify(string password)
        {
            return _password == password;
        }

        public void Save(string output)
        {
            var stream = new FileStream(Temp, FileMode.Create);
            Formatter.Serialize(stream, this);
            stream.Close();
            var data = File.ReadAllBytes(Temp);
            File.WriteAllText(output, Encoding.Default.GetString(data));
            File.Delete(Temp);
        }

        public static Package Load(string input)
        {
            var data = File.ReadAllText(input);
            File.WriteAllBytes(Temp, Encoding.Default.GetBytes(data));
            var stream = new FileStream(Temp, FileMode.Open);
            var output = Formatter.Deserialize(stream) as Package;
            stream.Close();
            File.Delete(Temp);
            return output;
        }

    }

}