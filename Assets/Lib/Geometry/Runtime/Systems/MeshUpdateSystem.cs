using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;


namespace FunkySheep.Geometry
{
    public partial class MeshUpdateSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
        public Material material;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer.ParallelWriter ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();
            Entities
                .ForEach((Entity entity, int entityInQueryIndex, DynamicBuffer <Vertex> vertexBuffer, DynamicBuffer<Triangle> trianglesBuffer, DynamicBuffer<Uv> uvsBuffer, in MeshUpdateTag meshUpdateTag) =>
                {
                    var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

                    Mesh mesh = new Mesh();
                    mesh.indexFormat = IndexFormat.UInt32;
                    mesh.Clear();
                    mesh.SetVertices(vertexBuffer.AsNativeArray().Reinterpret<Vector3>());
                    mesh.SetIndices(trianglesBuffer.AsNativeArray(), MeshTopology.Triangles, 0);
                    mesh.SetUVs(0, uvsBuffer.AsNativeArray().Reinterpret<Vector3>());
                    mesh.RecalculateNormals();

                    var desc = new RenderMeshDescription(
                        mesh,
                        material,
                        shadowCastingMode: ShadowCastingMode.Off,
                        receiveShadows: false);

                    RenderMeshUtility.AddComponents(
                        entity,
                        entityManager,
                        desc);

                    ecb.RemoveComponent<MeshUpdateTag>(entityInQueryIndex, entity);
                })
                .WithoutBurst()
                .WithStructuralChanges()
                .Run();

                m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}