using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nini.Config;
using Dirac.Logging;

namespace Dirac.Config
{
    public class ConfigManagerHelper : Dirac.Singleton.Singleton<ConfigManagerHelper>
    {
        //private static readonly Logger Logger = LogManager.CreateLogger();
        private IniConfigSource Parser; // the ini parser.
        private string ConfigFile;
        private bool _fileExists = false; // does the ini file exists?

        public ConfigManagerHelper()
        {
            try
            {
                ConfigFile = Environment.CurrentDirectory + "\\" + "config.ini"; // the config file's location.
                Parser = new IniConfigSource(ConfigFile); // see if the file exists by trying to parse it.
                _fileExists = true;
            }
            catch (Exception ex)
            {
                Logging.LogManager.DefaultLogger.Error("Error loading settings config.ini, will be using default settings " + ex.Message);
            }

            finally
            {
                // adds aliases so we can use On and Off directives in ini files.
                Parser.Alias.AddAlias("On", true);
                Parser.Alias.AddAlias("Off", false);

                // logger level aliases.
                Parser.Alias.AddAlias("MinimumLevel", Logger.Level.Trace);
                Parser.Alias.AddAlias("MaximumLevel", Logger.Level.Trace);
            }

            Parser.ExpandKeyValues();
        }

        public IConfig Section(string section) // Returns the asked config section.
        {
            return Parser.Configs[section];
        }

        public IConfig AddSection(string section) // Adds a config section.
        {
            return Parser.AddConfig(section);
        }

        public void Save() //  Saves the settings.
        {
            if (_fileExists) Parser.Save();
            else
            {
                Parser.Save(ConfigFile);
                _fileExists = true;
            }
        }
    }
}
