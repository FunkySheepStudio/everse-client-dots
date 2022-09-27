using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using FunkySheep.Geometry;

namespace FunkySheep.Terrain
{
    [AddComponentMenu("FunkySheep/Terrain/Spawner")]
    public class Spawner : MonoBehaviour
    {
        public int squareSize;
        public int squaresCount;

        private void Start()
        {
            SpawnTile(new int2(0, 0));
        }

        void SpawnTile(int2 position)
        {
            /*Entity tileEntity = World.DefaultGameObjectInjectionWorld.EntityManager.Instantiate(tilePrefab.Value);
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData<TileComponent>(
                tileEntity,
                new TileComponent
                {
                    size = squareSize,
                    count = squaresCount,
                    position = position
                }
            );*/
        }
    }

}
