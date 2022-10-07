using Unity.Entities;
using Unity.Mathematics;
using FunkySheep.Earth;
using UnityEngine;

namespace FunkySheep.Maps
{
    [DisableAutoCreation]
    public partial class InitMapSingleton : SystemBase
    {
        private MapSingletonComponent mapSingletonComponent;
        protected override void OnStartRunning()
        {
            mapSingletonComponent = GetSingleton<MapSingletonComponent>();

            double2 gpsPosition = GetSingleton<EarthSingletonComponent>().initialGpsPosition;
            mapSingletonComponent.tileSize = (float)Utils.TileSize(mapSingletonComponent.zoomLevel, gpsPosition.x);
            mapSingletonComponent.initialMapPosition = Utils.GpsToMapRealFloat2(mapSingletonComponent.zoomLevel, gpsPosition);

            mapSingletonComponent.initialOffset = new float2
            (
              -(mapSingletonComponent.initialMapPosition.x - Mathf.Floor(mapSingletonComponent.initialMapPosition.x)),
              -1 + (mapSingletonComponent.initialMapPosition.y - Mathf.Floor(mapSingletonComponent.initialMapPosition.y))
            );

            SetSingleton<MapSingletonComponent>(mapSingletonComponent);
        }

        protected override void OnUpdate()
        {
        }
    }

}
