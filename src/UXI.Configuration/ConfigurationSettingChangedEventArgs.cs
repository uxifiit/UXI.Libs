namespace UXI.Configuration
{

    public delegate void ConfigurationSettingChangedEventHandler(string sectionName, string settingKey);
    public delegate void ConfigurationSettingMissingEventHandler(string sectionName, string settingKey);
    public delegate void ConfigurationSettingWriteFailedEventHandler(string sectionName, string settingKey);
}
