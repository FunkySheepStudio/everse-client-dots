using Unity.Entities;
using UnityEngine;
using FunkySheep.Maps;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using FunkySheep.OSM.Ecs;
using Unity.Mathematics;
using FunkySheep.Geometry;

namespace FunkySheep.Buildings
{
    public class Downloader : MonoBehaviour
    {
        //public String urlTemplate;
        public GameObject tilePrefab;
        public GameObject nodePrefab;
        [HideInInspector]
        public Entity tileEntity;
        [HideInInspector]
        public Entity nodeEntity;
        [HideInInspector]
        public EntityManager entityManager;
        [HideInInspector]
        public BlobAssetStore blobAssetStore;
        List<MapPositionComponent> currentMapPositions = new List<MapPositionComponent>();
        ConcurrentQueue<OsmNodeGpsPosition> nodes = new ConcurrentQueue<OsmNodeGpsPosition>();

        private void Awake()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            blobAssetStore = new BlobAssetStore();
            GameObjectConversionSettings convertionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);

            tileEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(tilePrefab, convertionSettings);
            nodeEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(nodePrefab, convertionSettings);
        }

        public void Download(MapSingletonComponent mapSingleton, MapPositionComponent mapPosition, double[] gpsBoundaries)
        {
            if (!currentMapPositions.Contains(mapPosition))
            {
                string startLatitude = gpsBoundaries[0].ToString().Replace(',', '.');
                string startLongitude = gpsBoundaries[1].ToString().Replace(',', '.');
                string endLatitude = gpsBoundaries[2].ToString().Replace(',', '.');
                string endLongitude = gpsBoundaries[3].ToString().Replace(',', '.');

                string url = $"https://overpass.kumi.systems/api/interpreter?data=[out:json][timeout:25];(way[%22building%22]({startLatitude},{startLongitude},{endLatitude},{endLongitude});relation[%22building%22]({startLatitude},{startLongitude},{endLatitude},{endLongitude}););out%20body%20geom%20qt;";

                StartCoroutine(FunkySheep.Network.Downloader.Download(url, (fileID, file) =>
                {
                    /*Entity entity = entityManager.Instantiate(tileEntity);
                    DynamicBuffer<BuildingsComponent> buildings = entityManager.AddBuffer<BuildingsComponent>(entity);*/

                    Thread extractOsmThread = new Thread(() => ExtractOsmData(file));
                    extractOsmThread.Start();
                }));

                currentMapPositions.Add(mapPosition);
            }
        }

        private void Update()
        {
            while(nodes.Count != 0)
            {
                OsmNodeGpsPosition osmNodeGpsPosition;
                if (nodes.TryDequeue(out osmNodeGpsPosition))
                {
                    Entity entity = entityManager.Instantiate(nodeEntity);
                    entityManager.SetComponentData<OsmNodeGpsPosition>(entity, osmNodeGpsPosition);
                }
            }
        }

        public void ExtractOsmData(byte[] osmFile)
        {
            try
            {
                FunkySheep.OSM.Data parsedData = FunkySheep.OSM.Parser.Parse(osmFile);
                foreach (FunkySheep.OSM.Way way in parsedData.ways)
                {
                    //Building building = new Building(way.id);
                   

                    for (int i = 0; i < way.nodes.Count; i++)
                    {
                        double2 point = new double2
                        {
                            x = way.nodes[i].latitude,
                            y = way.nodes[i].longitude
                        };

                        /*if (i > 0)
                        {
                            double2 previousPoint = new double2
                            {
                                x = way.nodes[(i - 1) % (way.nodes.Count - 1)].latitude,
                                y = way.nodes[(i - 1) % (way.nodes.Count - 1)].longitude
                            };


                            double2 nextPoint = new double2
                            {
                                x = way.nodes[(i + 1) % (way.nodes.Count - 1)].latitude,
                                y = way.nodes[(i + 1) % (way.nodes.Count - 1)].longitude
                            };


                            if (math.abs(Geometry.Utils.AngleBetween(previousPoint, point, nextPoint)) < 10 && i > 0)
                            {
                                Debug.Log("Discard");
                                break;
                            }
                        }*/

                        nodes.Enqueue(new OsmNodeGpsPosition
                        {
                            Value = point
                        });
                        //Vector2 point = earthManager.CalculatePosition(way.nodes[i].latitude, way.nodes[i].longitude);
                        //building.points.Add(point);
                    }

                    /*building.tags = way.tags;

                    building.Initialize();
                    buildings.Enqueue(building);*/
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void OnDestroy()
        {
            blobAssetStore.Dispose();
        }
    }

}
