using Unity.Entities;
using Unity.Collections;

namespace FunkySheep.Earth
{
    public struct InitMercatorPositionJob : IJobEntityBatch
    {
        public ComponentTypeHandle<MercatorPosition> mercatorPositionType;
        [ReadOnly]
        public ComponentTypeHandle<GpsPosition> gpsPositionType;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            NativeArray<MercatorPosition> mercatorPositions = batchInChunk.GetNativeArray(this.mercatorPositionType);
            NativeArray<GpsPosition> gpsPositions = batchInChunk.GetNativeArray(this.gpsPositionType);

            for (int i = 0; i < batchInChunk.Count; i++)
            {
                MercatorPosition mercatorPosition = mercatorPositions[i];
                GpsPosition gpsPosition = gpsPositions[i];

                mercatorPosition.Value = mercatorPosition.Initial = Utils.toCartesianFloat2(gpsPosition.Value);
                mercatorPositions[i] = mercatorPosition;
            }
        }
    }
}
