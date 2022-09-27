using UnityEngine;
using Unity.Entities;
using FunkySheep.Geometry;

namespace FunkySheep.Terrain
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(ConvertToEntity))]
    [AddComponentMenu("FunkySheep/Terrain/Tile")]
    public class Tile : MonoBehaviour, IConvertGameObjectToEntity
    {
        public TileComponent tileComponent;

        private void Awake()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = new Mesh();
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddBuffer<Vertex>(entity);
            dstManager.AddBuffer<Uv>(entity);
            dstManager.AddBuffer<Normal>(entity);
            dstManager.AddBuffer<Triangle>(entity);

            dstManager.AddComponentData<TileComponent>(entity, tileComponent);
        }
    }
}
