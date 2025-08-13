using System.IO;
using UnityEditor;

public static class InstallerConfigHolder
{
    private const string ConfigName = "GenerateInstallerConfig.asset";
    private const string ConfigPath = "Assets/Resources";

    private static GenerateInstallerConfig _config;

    public static GenerateInstallerConfig Config
    {
        get
        {
            if (_config == null)
            {
                _config = (GenerateInstallerConfig)AssetDatabase.LoadAssetAtPath(Path.Combine(ConfigPath, ConfigName), typeof(GenerateInstallerConfig));
            }

            return _config;
        }
    }
}