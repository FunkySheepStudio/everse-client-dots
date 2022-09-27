using Unity.Entities;
using Unity.Mathematics;
using FunkySheep.Geometry;
using Unity.Rendering;
using UnityEngine;
using Unity.Jobs;

namespace FunkySheep.Terrain
{
    public partial class MeshUpdate : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithStructuralChanges()
                .WithoutBurst()
                .ForEach((Entity entity, DynamicBuffer<Vertex> vertexBuffer, DynamicBuffer<Triangle> trianglesBuffer, DynamicBuffer<Uv> uvsBuffer, in RenderMesh meshRenderer, in MeshUpdateTag meshUpdateTag) =>
                {
                    meshRenderer.mesh.Clear();
                    meshRenderer.mesh.SetVertices(vertexBuffer.AsNativeArray().Reinterpret<Vector3>());
                    meshRenderer.mesh.SetIndices(trianglesBuffer.AsNativeArray(), MeshTopology.Triangles, 0);
                    meshRenderer.mesh.SetUVs(0, uvsBuffer.AsNativeArray().Reinterpret<Vector3>());
                    meshRenderer.mesh.RecalculateNormals();
                    World.DefaultGameObjectInjectionWorld.EntityManager.RemoveComponent<MeshUpdateTag>(entity);
                }).Run();
        }
    }
}