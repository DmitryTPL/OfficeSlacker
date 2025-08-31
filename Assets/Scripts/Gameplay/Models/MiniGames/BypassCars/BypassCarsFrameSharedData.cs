using Gameplay.SharedData;

namespace Gameplay.Models
{
    public interface IBypassCarsFrameSharedData
    {
        BypassCarsFrameType FrameType { get; set; }
    }

    public class BypassCarsFrameSharedData : IBypassCarsFrameSharedData
    {
        public BypassCarsFrameType FrameType { get; set; }
    }
}