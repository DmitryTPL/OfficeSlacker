using System.IO;
using MVP;
using UnityEditor;
using UnityEngine;

public static class ViewMenuItem
{
    [MenuItem("CONTEXT/Component/Edit Presenter")]
    public static void OpenPresenter(MenuCommand menuCommand)
    {
        var viewType = menuCommand.context.GetType();
        
        var viewBaseType = menuCommand.context.GetType().BaseType;

        if (viewBaseType == null)
        {
            Debug.Log("Wrong view type without inheritance of base View type");
            return;
        }

        var presenterType = viewType.GetBaseTypeGenericArgument();

        if (presenterType == null)
        {
            Debug.Log("Can't get presenter from base view type");
            return;
        }

        var fileNames = Directory.GetFiles(Application.dataPath, presenterType.Name + ".cs", SearchOption.AllDirectories);

        if (fileNames.Length > 0)
        {
            var finalFileName = Path.GetFullPath(fileNames[0]);
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(finalFileName, 0);
        }
        else
        {
            Debug.Log("File Not Found:" + menuCommand.context.GetType() + ".cs");
        }
    }

    [MenuItem("CONTEXT/Component/Edit Presenter", true)]
    [MenuItem("CONTEXT/Component/Print And Copy Guid", true)]
    public static bool OpenPresenterValidation(MenuCommand menuCommand)
    {
        return menuCommand.context is IView;
    }
    
    [MenuItem("CONTEXT/Component/Print And Copy Guid")]
    public static void ShowGuid(MenuCommand menuCommand)
    {
        var view = menuCommand.context as IView;

        if (view == null)
        {
            return;
        }
        
        Debug.Log($"{view.Guid}\nGuid was copied to the clipboard");

        GUIUtility.systemCopyBuffer = view.Guid.ToString();
    }
}