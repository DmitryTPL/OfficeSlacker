using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = nameof(ScriptableObjectsInstaller), menuName = "Installers/ScriptableObjectsInstaller")]
    public class ScriptableObjectsInstaller : ScriptableObjectInstaller<ScriptableObjectsInstaller>
    {
        [SerializeField] private ScriptableObject[] _scriptableObjects;

        public override void InstallBindings()
        {
            // ReSharper disable once CoVariantArrayConversion
            Container.BindInstances(_scriptableObjects);
        }
    }
}