using Unity.Entities;
using UnityEngine;
using Unity.Collections;
using FunkySheep.Images;
using FunkySheep.Earth;
using System.Collections.Generic;
using Unity.Mathematics;


namespace FunkySheep.Terrain
{
    [AddComponentMenu("FunkySheep/Terrain/Heights Downloader")]
    public class HeightMapDownloader : MapDownloader
    {
        List<Entity> spawnedEarthTiles = new List<Entity>();
        Entity tilePrefab;

        public override void Awake()
        {
            base.Awake();
            tilePrefab = entityManager.CreateEntity();
            entityManager.SetName(tilePrefab, "Terrain - Heights - Prefab");
            entityManager.AddComponent<Prefab>(tilePrefab);
            entityManager.AddComponent<EarthGridPosition>(tilePrefab);
            entityManager.AddBuffer<AdjacentTiles>(tilePrefab);
        }

        public void DownloadAtGridPosition(EarthGridPosition earthGridPosition)
        {
            if (spawnedEarthTiles.Find(entity => entityManager.GetComponentData<EarthGridPosition>(entity).Value.Equals(earthGridPosition.Value)) == Entity.Null)
            {
                Entity entity = entityManager.Instantiate(tilePrefab);
                entityManager.SetComponentData<EarthGridPosition>(entity, earthGridPosition);
                spawnedEarthTiles.Add(entity);

                base.Download(entity);
                CheckForBorderTiles(entity);
            }
        }

        public override void Process(string fileId, Texture2D texture, Vector2Int mapPosition, Entity entity)
        {
            NativeArray<PixelComponent> pixelBuffer = new NativeArray<PixelComponent>();
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            pixelBuffer = texture.GetRawTextureData<PixelComponent>();

            EarthGridPosition entityGridPosition = entityManager.GetComponentData<EarthGridPosition>(entity);
            entityManager.SetName(entity, new FixedString64Bytes("Terrain - Heights - " + entityGridPosition.Value.ToString()));

            entityManager.AddBuffer<PixelComponent>(entity);
            entityManager.GetBuffer<PixelComponent>(entity).CopyFrom(pixelBuffer.ToArray());

            SetBorders(pixelBuffer, entity);
        }

        void SetBorders(NativeArray<PixelComponent> pixelBuffer, Entity entity)
        {
            int BorderSize = (int)math.sqrt(pixelBuffer.Length);
            // Left Border
            entityManager.AddBuffer<TerrainLeftBorder>(entity);
            entityManager.GetBuffer<TerrainLeftBorder>(entity).CopyFrom(pixelBuffer.Reinterpret<TerrainLeftBorder>().Slice(1, BorderSize - 2).ToArray());

            // Right Border
            entityManager.AddBuffer<TerrainRightBorder>(entity);
            entityManager.GetBuffer<TerrainRightBorder>(entity).CopyFrom(pixelBuffer.Reinterpret<TerrainRightBorder>().Slice((BorderSize - 1) * BorderSize, 254).ToArray());

            // Bottom Border and top
            entityManager.AddBuffer<TerrainBottomBorder>(entity);
            entityManager.AddBuffer<TerrainTopBorder>(entity);

            for (int i = 256; i < ((BorderSize - 1) * BorderSize); i += 256)
            {
                entityManager.GetBuffer<TerrainBottomBorder>(entity).Add(new TerrainBottomBorder
                {
                    Value = pixelBuffer[i].Value
                });

                entityManager.GetBuffer<TerrainTopBorder>(entity).Add(new TerrainTopBorder
                {
                    Value = pixelBuffer[i + 255].Value
                });
            }

            // Corners
            entityManager.AddComponentData<TerrainCorners>(entity, new TerrainCorners {
                BottomLeft = pixelBuffer[0].Value,
                TopLeft = pixelBuffer[255].Value,
                TopRight = pixelBuffer[pixelBuffer.Length - 1].Value,
                BottomRight = pixelBuffer[pixelBuffer.Length - 256].Value
            });

        }

        void CheckForBorderTiles(Entity entity)
        {
            EarthGridPosition entityGridPosition = entityManager.GetComponentData<EarthGridPosition>(entity);

            List<EarthGridPosition> adjacentTiles = new List<EarthGridPosition>();
            adjacentTiles.Add(new EarthGridPosition { Value = entityGridPosition.Value + new int2 { x = 0, y = 1 } });
            adjacentTiles.Add(new EarthGridPosition { Value = entityGridPosition.Value + new int2 { x = 1, y = 1 } });
            adjacentTiles.Add(new EarthGridPosition { Value = entityGridPosition.Value + new int2 { x = 1, y = 0 } });
            adjacentTiles.Add(new EarthGridPosition { Value = entityGridPosition.Value + new int2 { x = -1, y = 0 } });
            adjacentTiles.Add(new EarthGridPosition { Value = entityGridPosition.Value + new int2 { x = -1, y = -1 } });
            adjacentTiles.Add(new EarthGridPosition { Value = entityGridPosition.Value + new int2 { x = 0, y = -1 } });
            adjacentTiles.Add(new EarthGridPosition { Value = entityGridPosition.Value + new int2 { x = -1, y = 1 } });
            adjacentTiles.Add(new EarthGridPosition { Value = entityGridPosition.Value + new int2 { x = 1, y = -1 } });

            foreach (EarthGridPosition newAdjacentTile in adjacentTiles)
            {
                Entity adjacentTile = spawnedEarthTiles.Find(entity => entityManager.GetComponentData<EarthGridPosition>(entity).Value.Equals(newAdjacentTile.Value));
                if (adjacentTile == Entity.Null)
                {
                    Entity newEntity = entityManager.Instantiate(tilePrefab);
                    entityManager.SetComponentData<EarthGridPosition>(newEntity, newAdjacentTile);
                    spawnedEarthTiles.Add(newEntity);
                    entityManager.GetBuffer<AdjacentTiles>(newEntity).Add(new AdjacentTiles { entity = entity });
                    entityManager.GetBuffer<AdjacentTiles>(entity).Add(new AdjacentTiles { entity = newEntity });
                    base.Download(newEntity);
                } else
                {
                    entityManager.GetBuffer<AdjacentTiles>(adjacentTile).Add(new AdjacentTiles { entity = entity });
                    entityManager.GetBuffer<AdjacentTiles>(entity).Add(new AdjacentTiles { entity = adjacentTile });
                }
            }
            
        }
    }

}
