using Gameplay.SharedData;
using UnityEngine;

namespace Gameplay.Models
{
    public interface IMovableFrame
    {
        BypassCarsFrameType FrameType { get; }
        Vector3 Position { get; }

        void Reset();
        void AddToPosition(Vector3 delta);
    }
}