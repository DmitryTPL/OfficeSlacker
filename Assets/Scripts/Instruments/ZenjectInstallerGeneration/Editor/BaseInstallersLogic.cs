using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MVP;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Zenject;

public static class BaseInstallersLogic
{
    public const string BindPresentersMethod = "InstallPresenters";
    public const string BindOtherTypesMethod = "InstallInjectedInPresenterTypes";
    public const string BindPresenterLine = "            Container.BindInterfacesAndSelfTo<{0}>().AsSingle();";
    public const string BindOtherTypeLine = "            Container.BindInterfacesTo<{0}>().AsSingle();";

    public static bool IsInvalidationPassed()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("Object, to create installer for, must be selected");
            return false;
        }

        if (InstallerConfigHolder.Config == null)
        {
            Debug.LogError("Config CreateInstallerConfig need to be created in Resources folder");
            return false;
        }

        return true;
    }

    public static Type GetTypeByName(string name)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
        {
            var type = assembly.GetType(name);

            if (type != null)
            {
                return type;
            }
        }

        return null;
    }

    public static string GetTypePath(string name)
    {
        foreach (var assembly in CompilationPipeline.GetAssemblies(AssembliesType.Player))
        {
            var path = assembly.sourceFiles.FirstOrDefault(p => p.Contains(name));

            if (path != null)
            {
                return path;
            }
        }

        return null;
    }

    public static void GetViewsInScope(GameObject root, HashSet<IView> views)
    {
        var viewsOnObject = root.gameObject.GetComponents<IView>();

        foreach (var view in viewsOnObject)
        {
            views.Add(view);
        }

        for (var i = 0; i < root.transform.childCount; i++)
        {
            var child = root.transform.GetChild(i);

            if (!child.gameObject.TryGetComponent<GameObjectContext>(out _))
            {
                GetViewsInScope(child.gameObject, views);
            }
        }
    }

    public static IEnumerable<string> GetPresenters(GameObject selectedObject)
    {
        var views = new HashSet<IView>();

        GetViewsInScope(selectedObject, views);

        var presenterNames = new HashSet<string>();

        foreach (var view in views)
        {
            var presenterType = view.GetType().GetBaseTypeGenericArgument();

            if (presenterType == null)
            {
                continue;
            }

            presenterNames.Add(presenterType.FullName);
        }

        return presenterNames;
    }

    public static IEnumerable<string> GetOtherTypes(GameObject selectedObject)
    {
        var views = new HashSet<IView>();

        GetViewsInScope(selectedObject, views);

        var typeNames = new HashSet<string>();

        foreach (var view in views)
        {
            var presenterType = view.GetType().GetBaseTypeGenericArgument();

            if (presenterType == null)
            {
                continue;
            }

            var attributes = presenterType.GetCustomAttributes<InstallerGenerationInjectionAttribute>(true);

            foreach (var attribute in attributes)
            {
                typeNames.Add(attribute.BoundInjection.FullName);
            }
        }

        return typeNames;
    }

    public static IEnumerable<string> GetAlreadyBoundTypes(GameObject selectedObject, params string[] ignoredInstallers)
    {
        var installers = selectedObject.GetComponentsInChildren<MonoInstaller>(true);

        var boundTypes = new HashSet<string>();

        foreach (var installer in installers)
        {
            var installerName = installer.GetType().Name;
            
            if (ignoredInstallers != null && ignoredInstallers.Any(inst => inst.Contains(installerName)))
            {
                continue;
            }
            
            var installerPath = GetTypePath(installer.GetType().Name);

            var installerContent = File.ReadLines(installerPath).ToList();

            foreach (var line in installerContent)
            {
                if (line.Contains("Bind"))
                {
                    var start = line.IndexOf("Bind", StringComparison.Ordinal);

                    var braceStart = line.IndexOf('<', start);

                    if (braceStart == -1)
                    {
                        continue;
                    }

                    var braceEnd = line.IndexOf('>', braceStart);

                    boundTypes.Add(line.Substring(braceStart + 1, braceEnd - braceStart - 1));
                }
            }
        }

        return boundTypes;
    }
}