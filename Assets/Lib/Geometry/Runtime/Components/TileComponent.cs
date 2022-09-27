using System;
using Unity.Entities;

namespace FunkySheep.Geometry
{
    [Serializable]
    public struct TileComponent : IComponentData
    {
        public int size;
        public int count;
    }
}
