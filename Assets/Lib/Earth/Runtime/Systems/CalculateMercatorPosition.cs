using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Earth
{
    public partial class CalculateMercatorPosition : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref MercatorPosition mercatorPosition, in GpsPosition gpsPosition) =>
            {
                float2 newMercatorPosition = Utils.toCartesianFloat2(gpsPosition.Value);
                if (!mercatorPosition.Value.Equals(newMercatorPosition))
                {
                    mercatorPosition.Value = newMercatorPosition;
                }

            }).ScheduleParallel();
        }
    }
}
