using Cysharp.Threading.Tasks;

namespace AsyncAppTime
{
    public interface IAppTime
    {
        public float Time { get; set; }
        public float DeltaTime { get; set; }
        public float FixedDeltaTime { get; set; }
        AsyncReactiveProperty<float> TimeScale { get; }
    }

    public class AppTime : IAppTime
    {
        public float Time { get; set; }
        public float DeltaTime { get; set; }
        public float FixedDeltaTime { get; set; }
        public AsyncReactiveProperty<float> TimeScale { get; } = new(1);
    }
}