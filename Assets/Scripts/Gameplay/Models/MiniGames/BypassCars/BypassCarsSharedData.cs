using System.Collections.Generic;

namespace Gameplay.Models
{
    public interface IBypassCarsSharedData
    {
        IList<IMovableFrame> MovableFrames { get; }
        IList<IPopulatableFrame> PopulatableFrames { get; }
    }

    public class BypassCarsSharedData : IBypassCarsSharedData
    {
        public IList<IMovableFrame> MovableFrames { get; } = new List<IMovableFrame>();
        public IList<IPopulatableFrame> PopulatableFrames { get; } = new List<IPopulatableFrame>();
    }
}