using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PersonalBudgetingApplication.Classes
{
    /// <summary>
    /// Properties for managing custom application settings. Serializable
    /// </summary>
    public class ApplicationSettings
    {
        public Profile DefaultProfile { get; set; }

        public ApplicationSettings()
        {
            
        }
    }
}
