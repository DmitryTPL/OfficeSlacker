using Gameplay.SharedData;

namespace Gameplay.Models
{
    public interface IPopulatableFrame
    {
        BypassCarsFrameType FrameType { get; }
        
        void Populate();
    }
}