using System;
using Unity.Entities;
using Unity.Mathematics;
using FunkySheep.Geometry;

namespace FunkySheep.Terrain
{
    [Serializable]
    public struct TileComponent : IComponentData
    {
        public int2 position;
        public int size;
        public int count;
    }
}
