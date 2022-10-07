using Unity.Entities;
using FunkySheep.Earth;
using Unity.Jobs;

namespace FunkySheep.Maps
{
    [DisableAutoCreation]
    [UpdateAfter(typeof(UpdateGpsPosition))]
    public partial class UpdateMapPosition : SystemBase
    {
        public JobHandle updateMapPositionJobHandle;
        private EntityQuery query;

        protected override void OnCreate()
        {
            this.query = GetEntityQuery(typeof(MapPositionComponent), typeof(TilePositionComponent));
        }

        protected override void OnUpdate()
        {
            JobHandle updateGpsPositionJobHandle = World.GetOrCreateSystem<UpdateGpsPosition>().updateGpsPositionJobHandle;

            this.Dependency = JobHandle.CombineDependencies(this.Dependency, updateGpsPositionJobHandle);

            MapSingletonComponent mapSingletonComponent = GetSingleton<MapSingletonComponent>();

            UpdateMapPositionJob updateMapPositionJob = new UpdateMapPositionJob
            {
                MapPositionComponentType = GetComponentTypeHandle<MapPositionComponent>(),
                TilePositionComponentType = GetComponentTypeHandle<TilePositionComponent>(),
                initialMapPosition = mapSingletonComponent.initialMapPosition
            };

            updateMapPositionJobHandle = updateMapPositionJob.ScheduleParallel(this.query, this.Dependency);

            this.Dependency = JobHandle.CombineDependencies(this.Dependency, updateMapPositionJobHandle);
        }
    }
}
