using Unity.Entities;
using Unity.Collections;

namespace FunkySheep.Earth
{
    public struct UpdateGpsPositionJob : IJobEntityBatch
    {
        public ComponentTypeHandle<GpsPosition> gpsPositionType;
        [ReadOnly]
        public ComponentTypeHandle<MercatorPosition> mercatorPositionType;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            NativeArray<GpsPosition> gpsPositions = batchInChunk.GetNativeArray(this.gpsPositionType);
            NativeArray<MercatorPosition> mercatorPositions = batchInChunk.GetNativeArray(this.mercatorPositionType);

            for (int i = 0; i < batchInChunk.Count; i++)
            {
                MercatorPosition mercatorPosition = mercatorPositions[i];
                GpsPosition gpsPosition = gpsPositions[i];

                gpsPosition.Value = Utils.toGeoCoordDouble2(mercatorPosition.Value);
                gpsPositions[i] = gpsPosition;
            }
        }
    }
}
