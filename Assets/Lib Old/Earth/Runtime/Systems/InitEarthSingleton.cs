using Unity.Entities;

namespace FunkySheep.Earth
{
    public partial class InitEarthSingleton : SystemBase
    {
        private EarthSingletonComponent earthSingleton;
        protected override void OnStartRunning()
        {
            earthSingleton = GetSingleton<EarthSingletonComponent>();
        }

        protected override void OnUpdate()
        {
        }
    }

}
