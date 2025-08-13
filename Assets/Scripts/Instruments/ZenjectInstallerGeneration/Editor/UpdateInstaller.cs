using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zenject;

public class UpdateInstaller
{
    [MenuItem("CONTEXT/MonoInstaller/Update")]
    private static void UpdateContextMenu(MenuCommand command)
    {
        if (!BaseInstallersLogic.IsInvalidationPassed())
        {
            return;
        }

        var installer = command.context as MonoInstaller;

        Update(installer);
    }

    public static void Update(MonoInstaller installer)
    {
        var name = installer.GetType().Name;

        var path = BaseInstallersLogic.GetTypePath(name);
        var fullSystemPath = Path.Combine(Application.dataPath, path.TrimStart("Assets/".ToCharArray()));

        var script = File.ReadLines(fullSystemPath).ToList();
        
        var ignoreTypes = BaseInstallersLogic.GetAlreadyBoundTypes(installer.gameObject, name);
        var presenters = BaseInstallersLogic.GetPresenters(installer.gameObject);
        var otherTypes = BaseInstallersLogic.GetOtherTypes(installer.gameObject);
        
        var filteredPresenters = presenters.Where(presenter => ignoreTypes.All(ignored => !presenter.Contains(ignored)));
        var filteredOtherTypes = otherTypes.Where(otherType => ignoreTypes.All(ignored => !otherType.Contains(ignored)));

        UpdateBindings(script, BaseInstallersLogic.BindPresentersMethod, filteredPresenters, BaseInstallersLogic.BindPresenterLine);
        UpdateBindings(script, BaseInstallersLogic.BindOtherTypesMethod, filteredOtherTypes, BaseInstallersLogic.BindOtherTypeLine);

        File.Delete(fullSystemPath);
        File.WriteAllLines(fullSystemPath, script);

        AssetDatabase.ImportAsset(path);
    }

    private static void UpdateBindings(IList<string> script, string scopeMethodName, IEnumerable<string> names, string bindingLine)
    {
        for (var i = 0; i < script.Count; i++)
        {
            if (script[i].Contains(scopeMethodName))
            {
                i += 2; //pass braces

                for (var j = i; j < script.Count; j++)
                {
                    if (script[j].Contains("}"))
                    {
                        break;
                    }

                    script.RemoveAt(j); // remove old bindings
                    j--;
                }

                foreach (var typeName in names)
                {
                    script.Insert(i, string.Format(bindingLine, typeName));
                    i++;
                }
            }
        }
    }
}