using System;

namespace Dirac.DB
{
    public sealed class DataBaseConfig : Config.Config
    {
        public string Server
        {
            get { return this.GetString("Server", @"localhost"); }
            set { this.Set("Server", value); }
        }

        public string User
        {
            get { return this.GetString("User", @"root"); }
            set { this.Set("User", value); }
        }

        public string Password
        {
            get { return this.GetString("Password", @"123456"); }
            set { this.Set("Password", value); }
        }

        public DataBaseConfig() :
            base("DataBaseConfig") // Call the base ctor with section name 'Logging'.
        { }

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static DataBaseConfig Instance { get { return _instance; } }

        /// <summary>
        /// The internal instance pointer.
        /// </summary>
        private static readonly DataBaseConfig _instance = new DataBaseConfig();
    }
}
