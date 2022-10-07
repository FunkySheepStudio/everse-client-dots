using Unity.Entities;
using Unity.Collections;
using Unity.Burst;

namespace FunkySheep.Earth
{
    [BurstCompile]
    public struct UpdateGpsPositionJob : IJobEntityBatch
    {
        public ComponentTypeHandle<GpsPositionComponent> gpsPositionType;
        [ReadOnly]
        public ComponentTypeHandle<MercatorPositionComponent> mercatorPositionType;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            NativeArray<GpsPositionComponent> gpsPositions = batchInChunk.GetNativeArray(this.gpsPositionType);
            NativeArray<MercatorPositionComponent> mercatorPositions = batchInChunk.GetNativeArray(this.mercatorPositionType);

            for (int i = 0; i < batchInChunk.Count; i++)
            {
                MercatorPositionComponent mercatorPosition = mercatorPositions[i];
                GpsPositionComponent gpsPosition = gpsPositions[i];

                gpsPosition.Value = Utils.toGeoCoordDouble2(mercatorPosition.Value);
                gpsPositions[i] = gpsPosition;
            }
        }
    }
}
