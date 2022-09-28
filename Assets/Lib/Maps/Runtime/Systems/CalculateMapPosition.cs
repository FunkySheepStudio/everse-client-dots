using Unity.Entities;
using Unity.Mathematics;
using FunkySheep.Earth;

namespace FunkySheep.Maps
{
    public partial class CalculateMapPosition : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref MapPosition mapPosition, in ZoomLevel zoomLevel, in GpsPosition gpsPosition) =>
            {
                float2 newMapPosition = Utils.GpsToMapRealFloat2(zoomLevel.Value, gpsPosition.Value);
                if (!mapPosition.Value.Equals(newMapPosition))
                {
                    mapPosition.Value = newMapPosition;
                }

            }).ScheduleParallel();
        }
    }
}
