using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace FunkySheep.Maps
{
    public partial class UpdateTilePosition : SystemBase
    {
        public JobHandle updateTilePositionJobHandle;
        private EntityQuery query;

        protected override void OnCreate()
        {
            this.query = GetEntityQuery(typeof(TilePositionComponent), typeof(Translation));
        }

        protected override void OnUpdate()
        {
            MapSingletonComponent mapSingletonComponent = GetSingleton<MapSingletonComponent>();

            UpdateTilePositionJob updateTilePositionJob = new UpdateTilePositionJob
            {
                TilePositionComponentType = GetComponentTypeHandle<TilePositionComponent>(),
                TranslationComponentType = GetComponentTypeHandle<Translation>(),
                mapSingleton = mapSingletonComponent
            };

            updateTilePositionJobHandle = updateTilePositionJob.ScheduleParallel(this.query, this.Dependency);

            this.Dependency = JobHandle.CombineDependencies(this.Dependency, updateTilePositionJobHandle);
        }
    }
}
