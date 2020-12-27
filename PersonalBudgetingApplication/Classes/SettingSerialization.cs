using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PersonalBudgetingApplication.Classes
{
    class SettingSerialization
    {
        public void SerializeFile()
        {
            Profile provided = new Profile(1);

            var xs = new XmlSerializer(typeof(Profile));

            var Writer = new StreamWriter(@"Profile.xml");

            xs.Serialize(Writer, provided);

            Writer.Close();
        }

        public Profile ReadXMLFile()
        {
            var xs = new XmlSerializer(typeof(Profile));
            var read = new StreamReader("Profile.xml");
            object boj = xs.Deserialize(read);
            Profile basic = (Profile)boj;
            read.Close();
            return basic;
        }

        public static void CreateSettingXMLFile()
        {
            ApplicationSettings saved = new ApplicationSettings();

            var xs = new XmlSerializer(typeof(ApplicationSettings));

            var Write = new StreamWriter(@"ApplicationSettings.xml");

            xs.Serialize(Write, saved);

            Write.Close();
        }

        public static ApplicationSettings ReadSettings()
        {
            var xs = new XmlSerializer(typeof(ApplicationSettings));
            try
            {
                using (var read = new StreamReader("ApplicationSettings.xml"))
                {
                    object obj = xs.Deserialize(read);
                    read.Close();
                    ApplicationSettings Deserialized = (ApplicationSettings)obj;
                    return Deserialized;
                }
            }
            catch(FileNotFoundException)
            {
                //Create Application Settings File
                CreateSettingXMLFile();
                return ReadSettings();
            }
        }

        public static void SaveSettings(ApplicationSettings toSave)
        {
            var xs = new XmlSerializer(typeof(ApplicationSettings));

            var Write = new StreamWriter(@"ApplicationSettings.xml");

            xs.Serialize(Write, toSave);

            Write.Close();
        }
    }
}
