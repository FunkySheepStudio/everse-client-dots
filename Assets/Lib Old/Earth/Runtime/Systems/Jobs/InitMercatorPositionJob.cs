using Unity.Entities;
using Unity.Collections;
using Unity.Burst;

namespace FunkySheep.Earth
{
    [BurstCompile]
    public struct InitMercatorPositionJob : IJobEntityBatch
    {
        public ComponentTypeHandle<MercatorPositionComponent> mercatorPositionType;
        [ReadOnly]
        public ComponentTypeHandle<GpsPositionComponent> gpsPositionType;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            NativeArray<MercatorPositionComponent> mercatorPositions = batchInChunk.GetNativeArray(this.mercatorPositionType);
            NativeArray<GpsPositionComponent> gpsPositions = batchInChunk.GetNativeArray(this.gpsPositionType);

            for (int i = 0; i < batchInChunk.Count; i++)
            {
                MercatorPositionComponent mercatorPosition = mercatorPositions[i];
                GpsPositionComponent gpsPosition = gpsPositions[i];

                mercatorPosition.Value = mercatorPosition.Initial = Utils.toCartesianFloat2(gpsPosition.Value);
                mercatorPositions[i] = mercatorPosition;
            }
        }
    }
}
