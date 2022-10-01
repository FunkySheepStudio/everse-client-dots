using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using FunkySheep.Geometry;

namespace FunkySheep.Maps
{
    [BurstCompile]
    public struct UpdateTilePositionJob : IJobEntityBatch
    {
        public ComponentTypeHandle<TilePositionComponent> TilePositionComponentType;
        [ReadOnly]
        public ComponentTypeHandle<Translation> TranslationComponentType;
        public MapSingletonComponent mapSingleton;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            NativeArray<TilePositionComponent> tilePositions = batchInChunk.GetNativeArray(this.TilePositionComponentType);
            NativeArray<Translation> translations = batchInChunk.GetNativeArray(this.TranslationComponentType);

            for (int i = 0; i < batchInChunk.Count; i++)
            {
                TilePositionComponent tilePosition = tilePositions[i];
                Translation translation = translations[i];

                tilePosition.Value = TilesUtils.TilePosition(translation.Value, mapSingleton.tileSize, mapSingleton.initialOffset);
                tilePositions[i] = tilePosition;
            }
        }
    }
}
