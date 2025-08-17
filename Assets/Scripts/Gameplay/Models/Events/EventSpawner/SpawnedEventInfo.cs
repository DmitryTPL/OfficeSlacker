namespace Gameplay.Models
{
    public struct SpawnedEventInfo
    {
        public float FinishTime { get; }
        public bool CanBeParallel { get; }

        public SpawnedEventInfo(float finishTime, bool canBeParallel)
        {
            FinishTime = finishTime;
            CanBeParallel = canBeParallel;
        }
    }
}