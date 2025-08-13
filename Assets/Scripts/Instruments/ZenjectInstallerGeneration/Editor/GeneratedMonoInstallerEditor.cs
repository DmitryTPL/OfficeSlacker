using UnityEditor;
using UnityEngine;
using Zenject;

[CustomEditor(typeof(GeneratedMonoInstaller), true)]
public class GeneratedMonoInstallerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Update"))
        {
            UpdateInstaller.Update(target as MonoInstaller);
        }
    }
}