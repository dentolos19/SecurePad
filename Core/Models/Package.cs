using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace SecurePad.Core.Models
{

    [Serializable]
    public class Package
    {

        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Package));

        public string Password { get; set; }

        public string Seed { get; set; }

        public string Content { get; set; }

        public bool Verify(string password, string seed)
        {
            return Password == password && Seed == seed;
        }

        public void Save(string output)
        {
            Seed = Utilities.ToHexString(Seed);
            Password = Utilities.ToHexString(Password);
            Content = Utilities.ToHexString(Content);
            var stream = new StreamWriter(output);
            Serializer.Serialize(stream, this);
            stream.Close();
        }

        public static Package Load(string input)
        {
            var stream = new StreamReader(input);
            var output = Serializer.Deserialize(stream) as Package;
            stream.Close();
            output.Seed = Utilities.FromHexString(output.Seed);
            output.Password = Utilities.FromHexString(output.Password);
            output.Content = Utilities.FromHexString(output.Content);
            return output;
        }

    }

}