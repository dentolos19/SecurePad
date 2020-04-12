﻿using System;
using System.IO;
using System.Xml.Serialization;

namespace SecurePad.Core
{

    [Serializable]
    public class Configuration
    {

        private static readonly string Source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SecurePad.cfg");
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Configuration));

        public string Seed = "S3CUR3P4D";

        public void Save()
        {
            Seed = Utilities.ToHexString(Seed);
            var stream = new StreamWriter(Source);
            Serializer.Serialize(stream, this);
            stream.Close();
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