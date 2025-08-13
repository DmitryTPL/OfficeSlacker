using UnityEngine;

[CreateAssetMenu(fileName = "GenerateInstallerConfig", menuName = "ScriptableObjects/GenerateInstallerConfig")]
public class GenerateInstallerConfig : ScriptableObject
{
    [SerializeField] private string _installerNamespace = "GeneratedInstallers";
    [SerializeField] [Tooltip("Relative to Assets folder")] private string _defaultPath = "Scripts/Installers/Generated";
    [SerializeField] private string _installerPostfix = "GeneratedInstaller";

    public string InstallerNamespace => _installerNamespace;
    public string DefaultPath => _defaultPath;
    public string InstallerPostfix => _installerPostfix;
}