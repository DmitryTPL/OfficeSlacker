using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Zenject;

[InitializeOnLoad]
public class GenerateInstaller
{
    public const string Assets = "Assets";

    static GenerateInstaller()
    {
        AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
    }

    [MenuItem("CONTEXT/GameObjectContext/Generate Default Installer")]
    private static void CreateDefaultInstaller(MenuCommand command)
    {
        if (!BaseInstallersLogic.IsInvalidationPassed())
        {
            return;
        }

        CreateInDefaultFolder();
    }

    private static void OnAfterAssemblyReload()
    {
        var name = SessionState.GetString("name", "");

        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        SessionState.EraseString("name");

        var installerType = BaseInstallersLogic.GetTypeByName($"{InstallerConfigHolder.Config.InstallerNamespace}.{name}");

        if (installerType == null)
        {
            Debug.LogError("Failed to assign installer to object. Please, assign it manually");

            return;
        }

        if (!Selection.activeGameObject.TryGetComponent(installerType, out var installerComponent))
        {
            installerComponent = Selection.activeGameObject.AddComponent(installerType);
        }

        var isGameContextExists = Selection.activeGameObject.gameObject.TryGetComponent<GameObjectContext>(out var gameObjectContext);

        if (!isGameContextExists)
        {
            Selection.activeGameObject.AddComponent<GameObjectContext>();
        }

        if (!gameObjectContext.Installers.FirstOrDefault(i => i.GetType() == installerType))
        {
            var installers = gameObjectContext.Installers.ToList();

            installers.Add(installerComponent as MonoInstaller);

            gameObjectContext.Installers = installers;
        }
    }

    [MenuItem("GameObject/Installer/Default")]
    private static void CreateInDefaultFolder()
    {
        if (!BaseInstallersLogic.IsInvalidationPassed())
        {
            return;
        }

        var name = $"{Selection.activeGameObject.name}{InstallerConfigHolder.Config.InstallerPostfix}";

        name = CorrectFileName(name);

        var installerType = BaseInstallersLogic.GetTypeByName($"{InstallerConfigHolder.Config.InstallerNamespace}.{name}");

        if (installerType != null)
        {
            ShowAlreadyCreatedInstallerMessage(name);
            return;
        }

        AssignInstaller(InstallerConfigHolder.Config.DefaultPath, name);
    }

    [MenuItem("GameObject/Installer/Specific Directory")]
    private static void CreateInSpecificFolder()
    {
        if (!BaseInstallersLogic.IsInvalidationPassed())
        {
            return;
        }

        var path = EditorUtility.SaveFilePanelInProject(
            "Save Installer",
            $"{Selection.activeGameObject.name}{InstallerConfigHolder.Config.InstallerPostfix}.cs",
            "cs",
            "",
            Path.Combine(Assets, InstallerConfigHolder.Config.DefaultPath));

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        path = path.TrimStart($"{Assets}/".ToCharArray());

        var name = CorrectFileName(Path.GetFileNameWithoutExtension(path));

        var installerType = BaseInstallersLogic.GetTypeByName($"{InstallerConfigHolder.Config.InstallerNamespace}.{name}");

        if (installerType != null)
        {
            ShowAlreadyCreatedInstallerMessage(name);
            return;
        }

        AssignInstaller(Path.GetDirectoryName(path), name);
    }

    private static void ShowAlreadyCreatedInstallerMessage(string name)
    {
        Debug.LogError(
            $"Root installer for this object is already created. Located: {BaseInstallersLogic.GetTypePath(name)}. Please, rename root object or assign installer to GameObjectContext manually");
    }

    private static void AssignInstaller(string path, string name)
    {
        var targetObject = Selection.activeGameObject;

        var isGameContextExists = targetObject.TryGetComponent<GameObjectContext>(out _);

        if (!isGameContextExists)
        {
            targetObject.AddComponent<GameObjectContext>();
        }

        var scriptPath = Path.Combine(Application.dataPath, path, $"{name}.cs");

        if (File.Exists(scriptPath))
        {
            File.Delete(scriptPath);
        }

        var directory = Path.GetDirectoryName(scriptPath);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var code = CreateScript(targetObject, name, InstallerConfigHolder.Config.InstallerNamespace);

        File.WriteAllText(scriptPath, code.ToString(), Encoding.UTF8);
        AssetDatabase.ImportAsset(Path.Combine(Assets, path, $"{name}.cs"));

        SessionState.SetString("name", name);

        EditorUtility.SetDirty(targetObject);
    }

    private static StringBuilder CreateScript(GameObject selectedObject, string name, string installerNamespace)
    {
        var code = new StringBuilder();

        var ignoreTypes = BaseInstallersLogic.GetAlreadyBoundTypes(selectedObject);
        var presenters = BaseInstallersLogic.GetPresenters(selectedObject);
        var otherTypes = BaseInstallersLogic.GetOtherTypes(selectedObject);

        var filteredPresenters = presenters.Where(presenter => ignoreTypes.All(ignored => !presenter.Contains(ignored)));
        var filteredOtherTypes = otherTypes.Where(otherType => ignoreTypes.All(ignored => !otherType.Contains(ignored)));

        AppendHeader(code);
        AppendScriptBody(code, name, installerNamespace, filteredPresenters, filteredOtherTypes);

        return code;
    }

    private static void AppendHeader(StringBuilder code)
    {
        code.AppendLine("//------------------------------------------------------------------------------");
        code.AppendLine("// <auto-generated>");
        code.AppendLine("//");
        code.AppendLine("//     Changes to this file may cause incorrect behavior and will be lost if");
        code.AppendLine("//     the code is regenerated.");
        code.AppendLine("//");
        code.AppendLine("// </auto-generated>");
        code.AppendLine("//------------------------------------------------------------------------------");
    }

    private static void AppendScriptBody(StringBuilder code, string name, string installerNamespace, IEnumerable<string> presenterNames,
        IEnumerable<string> otherTypeNames)
    {
        code.AppendLine($"namespace {installerNamespace}");
        code.AppendLine($"{{");
        code.AppendLine($"    public class {name} : GeneratedMonoInstaller ");
        code.AppendLine($"    {{");
        code.AppendLine($"        protected override void {BaseInstallersLogic.BindPresentersMethod}()");
        code.AppendLine($"        {{");

        foreach (var presenterName in presenterNames)
        {
            code.AppendLine(string.Format(BaseInstallersLogic.BindPresenterLine, presenterName));
        }

        code.AppendLine($"        }}");
        code.AppendLine();
        code.AppendLine($"        protected override void {BaseInstallersLogic.BindOtherTypesMethod}()");
        code.AppendLine($"        {{");

        foreach (var otherType in otherTypeNames)
        {
            code.AppendLine(string.Format(BaseInstallersLogic.BindOtherTypeLine, otherType));
        }

        code.AppendLine($"        }}");
        code.AppendLine($"    }}");
        code.AppendLine($"}}");
        code.AppendLine($"");
    }

    private static string CorrectFileName(string name)
    {
        var array = name.Where(c => c is >= 'A' and <= 'Z' or >= 'a' and <= 'z').ToArray();

        array[0] = char.ToUpper(array[0]);

        return new string(array);
    }
}