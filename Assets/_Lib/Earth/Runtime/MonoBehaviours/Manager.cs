using UnityEngine;
using Unity.Mathematics;

namespace FunkySheep.Earth
{
    [AddComponentMenu("FunkySheep/Earth/Manager")]
    public class Manager : FunkySheep.Types.Singleton<Manager>
    {
        public FunkySheep.Types.Double latitude;
        public FunkySheep.Types.Double longitude;
        public FunkySheep.Types.Int32 zoomLevel;
        public FunkySheep.Types.Float tileSize;
        public FunkySheep.Types.Vector2Int initialMapPosition;

        static int _zoomLevel;
        static Vector2Int _initialMapPosition;
        float _tileSize;

        public override void Awake()
        {
            base.Awake();
            tileSize.value = (float)Utils.TileSize(zoomLevel.value, latitude.value);
            Vector2 mapPosition = Utils.GpsToMapReal(zoomLevel.value, latitude.value, longitude.value);

            initialMapPosition.value = new UnityEngine.Vector2Int(
                (int)mapPosition.x,
                (int)mapPosition.y
            );

            _zoomLevel = zoomLevel.value;
            _initialMapPosition = initialMapPosition.value;
            _tileSize = tileSize.value;
        }

        /// <summary>
        /// Return the world position given GPS Coordinates
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static float3 GetWorldPosition(double latitude, double longitude)
        {
            Vector2 calculatedMapPosition = Utils.GpsToMapReal(_zoomLevel, latitude, longitude);
            Vector2 mapOffset = calculatedMapPosition - _initialMapPosition;

            return new float3(
                mapOffset.x * Manager.Instance._tileSize,
                0,
                 Manager.Instance._tileSize - mapOffset.y * Manager.Instance._tileSize // Since map coordinates are reversed on Y axis
            );
        }


        /// <summary>
        /// Get the position on the grid relative to the transform
        /// </summary>
        /// <param name="translation"></param>
        /// <returns></returns>
        public static int2 GetTilePosition(float3 translation, float tileSize)
        {
            return new int2
            {
                x = (int)math.floor((translation.x / tileSize)),
                y = (int)math.floor((translation.z / tileSize))
            };
        }
    }
}
