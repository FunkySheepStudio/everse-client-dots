using Unity.Entities;
using Unity.Mathematics;

namespace FunkySheep.Geometry
{
    /// <summary>
    ///     The buffer of mesh vertices.
    /// </summary>
    [InternalBufferCapacity(0)]
    [GenerateAuthoringComponent]
    public struct Vertex : IBufferElementData
    {
        /// <summary>
        ///     The Vertex.
        /// </summary>
        public float3 Value;
    }
}
