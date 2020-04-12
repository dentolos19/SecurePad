using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SecurePad.Core.Models
{

    [Serializable]
    public class Package
    {

        private static readonly string Temp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SecurePad.tmp");
        private static readonly BinaryFormatter Formatter = new BinaryFormatter();

        private string _password;
        private string _seed;

        public string Content { get; set; }

        public Package(string password, string seed)
        {
            _password = password;
            _seed = seed;
        }

        public bool Verify(string password, string seed)
        {
            return _password == password && _seed == seed;
        }

        public void Save(string output)
        {
            _seed = Utilities.ToHexString(_seed);
            _password = Utilities.ToHexString(_password);
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
            output._seed = Utilities.FromHexString(output._seed);
            output._password = Utilities.FromHexString(output._password);
            return output;
        }

    }

}