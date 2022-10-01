using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace FunkySheep.Maps
{
    [BurstCompile]
    public struct UpdateMapPositionJob : IJobEntityBatch
    {
        public ComponentTypeHandle<MapPositionComponent> MapPositionComponentType;
        [ReadOnly]
        public ComponentTypeHandle<TilePositionComponent> TilePositionComponentType;
        public float2 initialMapPosition;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            NativeArray<MapPositionComponent> mapPositions = batchInChunk.GetNativeArray(this.MapPositionComponentType);
            NativeArray<TilePositionComponent> tilePositions = batchInChunk.GetNativeArray(this.TilePositionComponentType);

            for (int i = 0; i < batchInChunk.Count; i++)
            {
                MapPositionComponent mapPosition = mapPositions[i];
                TilePositionComponent tilePosition = tilePositions[i];

                mapPosition.Value = new int2(
                  Mathf.FloorToInt(initialMapPosition.x) + tilePosition.Value.x,
                  Mathf.FloorToInt(initialMapPosition.y) - tilePosition.Value.y
                );

                mapPositions[i] = mapPosition;
            }
        }
    }
}
