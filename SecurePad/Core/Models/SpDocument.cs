using System;
using System.IO;
using System.Xml.Serialization;

namespace SecurePad.Core.Models
{

    [Serializable]
    public class SpDocument
    {

        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(SpDocument));

        public string Password { get; set; }

        public string Seed { get; set; }

        public string Content { get; set; }

        public bool Verify(string password, string seed)
        {
            return Password == password && Seed == seed;
        }

        public void Save(string output)
        {
            var tmpSeed = Seed;
            var tmpPassword = Password;
            var tmpContent = Content;
            Seed = Utilities.ToHexString(Seed);
            Password = Utilities.ToHexString(Password);
            Content = Utilities.Encrypt(Content, Utilities.GetUniqueCode($"{tmpSeed}+{tmpPassword}"));
            var stream = new StreamWriter(output);
            Serializer.Serialize(stream, this);
            stream.Close();
            Seed = tmpSeed;
            Password = tmpPassword;
            Content = tmpContent;
        }

        public static SpDocument Load(string input)
        {
            var stream = new StreamReader(input);
            var output = Serializer.Deserialize(stream) as SpDocument;
            stream.Close();
            output.Seed = Utilities.FromHexString(output.Seed);
            output.Password = Utilities.FromHexString(output.Password);
            output.Content = Utilities.Decrypt(output.Content, Utilities.GetUniqueCode($"{output.Seed}+{output.Password}"));
            return output;
        }

    }

}