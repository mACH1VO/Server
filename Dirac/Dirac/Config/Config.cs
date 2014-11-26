using System;
using Nini.Config;
using Dirac.Logging;

namespace Dirac.Config
{
    public class Config
    {
        private IConfig _section;
        public string SectionName { get; private set; }

        public Config(string sectionName)
        {
            this._section = ConfigManagerHelper.Instance.Section(sectionName) ?? ConfigManagerHelper.Instance.AddSection(sectionName);
            this.SectionName = sectionName;
        }

        public void Save()
        {
            ConfigManagerHelper.Instance.Save();
        }

        protected bool GetBoolean(string key, bool defaultValue) { return this._section.GetBoolean(key, defaultValue); }
        protected double GetDouble(string key, double defaultValue) { return this._section.GetDouble(key, defaultValue); }
        protected float GetFloat(string key, float defaultValue) { return this._section.GetFloat(key, defaultValue); }
        protected int GetInt(string key, int defaultValue) { return this._section.GetInt(key, defaultValue); }
        protected int GetInt(string key, int defaultValue, bool fromAlias) { return this._section.GetInt(key, defaultValue, fromAlias); }
        protected long GetLong(string key, long defaultValue) { return this._section.GetLong(key, defaultValue); }
        protected string GetString(string key, string defaultValue) { return this._section.Get(key, defaultValue); }
        protected string[] GetEntryKeys() { return this._section.GetKeys(); }
        protected void Set(string key, object value) { this._section.Set(key, value); }
    }
}
