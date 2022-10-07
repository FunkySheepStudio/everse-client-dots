using Unity.Entities;
using FunkySheep.Transform;
using Unity.Jobs;
using FunkySheep.Maps;

namespace FunkySheep.Earth
{
    [UpdateAfter(typeof(UpdateMercatorPosition))]
    public partial class UpdateGpsPosition : SystemBase
    {
        public JobHandle updateGpsPositionJobHandle { get; private set; }
        private EntityQuery query;

        protected override void OnCreate()
        {
            this.query = GetEntityQuery(typeof(GpsPositionComponent), typeof(MercatorPositionComponent), typeof(DeltaTranslationComponent));
        }

        protected override void OnUpdate()
        {
            JobHandle UpdateMercatorPositionJobHandle = World.GetOrCreateSystem<UpdateMercatorPosition>().updateMercatorPositionJobHandle;
            JobHandle initMercatorPositionJobHandle = World.GetOrCreateSystem<InitMercatorPosition>().initMercatorPositionJobHandle;
            JobHandle updateMapPositionJobHandle = World.GetOrCreateSystem<UpdateMapPosition>().updateMapPositionJobHandle;

            this.Dependency = JobHandle.CombineDependencies(UpdateMercatorPositionJobHandle, initMercatorPositionJobHandle, updateMapPositionJobHandle);

            UpdateGpsPositionJob updateGpsPositionJob = new UpdateGpsPositionJob
            {
                gpsPositionType = GetComponentTypeHandle<GpsPositionComponent>(),
                mercatorPositionType = GetComponentTypeHandle<MercatorPositionComponent>()
            };

            updateGpsPositionJobHandle = updateGpsPositionJob.ScheduleParallel(query, this.Dependency);
            this.Dependency = updateGpsPositionJobHandle;
        }
    }
}
