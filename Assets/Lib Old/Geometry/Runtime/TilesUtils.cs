using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

namespace FunkySheep.Geometry
{
    [BurstCompile]
    public class TilesUtils
    {
        /// <summary>
        /// Position relative to the grid with the offset
        /// </summary>
        /// <param name="position">World position</param>
        /// <returns></returns>
        public static float2 RelativePosition(float2 position, float tileSize, float2 initialOffset)
        {
            float2 relativePosition = position - tileSize * initialOffset;
            return relativePosition;
        }

        /// <summary>
        /// Tile position in a given world position
        /// </summary>
        /// <param name="position">World position</param>
        /// <returns></returns>
        public static int2 TilePosition(float2 position, float tileSize, float2 initialOffset)
        {
            float2 relativePosition = RelativePosition(position, tileSize, initialOffset);
            int2 tilePosition = new int2(
              Mathf.FloorToInt(relativePosition.x / tileSize),
              Mathf.FloorToInt(relativePosition.y / tileSize)
            );

            return tilePosition;
        }

        public static int2 TilePosition(float3 position, float tileSize, float2 initialOffset)
        {
            float2 newPosition = new float2
            {
                x = position.x,
                y = position.z
            };

            return TilePosition(newPosition, tileSize, initialOffset);
        }

        /// <summary>
        /// Calculate the relative value position inside a tile
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float2 InsideTilePosition(float2 position, float tileSize, float2 initialOffset)
        {
            float2 relativePosition = RelativePosition(position, tileSize, initialOffset);
            int2 tilePosition = TilePosition(position, tileSize, initialOffset);
            float2 insideTilePosition = relativePosition -
            new float2(
              tilePosition.x * tileSize,
              tilePosition.y * tileSize
            );

            insideTilePosition /= tileSize;

            return insideTilePosition;
        }

        /// <summary>
        /// Calculate the quarter position inside a tile 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static int2 InsideTileQuarterPosition(float2 position, float tileSize, float2 initialOffset)
        {
            float2 insideTilePosition = InsideTilePosition(position, tileSize, initialOffset);
            int2 insideTileQuarterPosition = int2.zero;

            if (insideTilePosition.x >= 0.5f)
            {
                insideTileQuarterPosition.x = 1;
            }
            else
            {
                insideTileQuarterPosition.x = -1;
            }

            if (insideTilePosition.y >= 0.5f)
            {
                insideTileQuarterPosition.y = 1;
            }
            else
            {
                insideTileQuarterPosition.y = -1;
            }

            return insideTileQuarterPosition;
        }

        /// <summary>
        /// Calculate the world offset givent the offset and the size of a tile
        /// </summary>
        /// <returns></returns>
        public static float2 WorldOffset(float tileSize, float2 initialOffset)
        {
            float2 worldOffset = initialOffset * tileSize;
            return worldOffset;
        }
    }
}
