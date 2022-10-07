using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Geometry
{
    [InternalBufferCapacity(0)]
    [GenerateAuthoringComponent]
    public struct Triangle : IBufferElementData
    {
        /// <summary>
        ///     The triangle.
        /// </summary>
        public int Value;
    }
}
