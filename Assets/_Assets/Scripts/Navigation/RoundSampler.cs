using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Navigation
{
    public partial class NavigationSample
    {
        public static void WriteSample(NavigationSample sample, Scene scene, SampleSettings settings)
        {
            Vector3 surfaceOffset = settings.SurfaceParent.position;
            sample.m_NodeSize = settings.NodeSize;
            var samples = sampleObstacles(scene, settings.SampleMask, surfaceOffset, out var globalBounds);
            sample.m_GridDimensions = new(Mathf.CeilToInt(globalBounds.size.x / settings.NodeSize),
                Mathf.CeilToInt(globalBounds.size.y / settings.NodeSize));

            Vector3 corner = surfaceOffset - new Vector3(sample.m_GridDimensions.x * sample.m_NodeSize * .5f, 
                0, sample.m_GridDimensions.y * sample.m_NodeSize * .5f);

            sample.m_Nodes = new SurfaceNode[sample.m_GridDimensions.x * sample.m_GridDimensions.y];
            
            for (int x = 0; x < sample.m_GridDimensions.x; x++)
            {
                for (int y = 0; y < sample.m_GridDimensions.y; y++)
                {
                    var ray = new Ray(corner + new Vector3(x * sample.m_NodeSize, 0, y * sample.m_NodeSize), Vector3.down);
                    bool wasCast = false;
                    
                    foreach (var s in samples)
                    {
                        if (s.RayCast(ray, out _))
                        {
                            wasCast = true;
                            break;
                        }
                    }

                    sample.m_Nodes[x * sample.m_GridDimensions.y + y] = new SurfaceNode
                    {
                        IsWalkable = !wasCast
                    };
                }
            }
        }

        static List<NavigationObstacle> sampleObstacles(Scene scene, LayerMask mask, Vector3 surfaceOffset, out Bounds globalBounds)
        {
            var roots = scene.GetRootGameObjects();

            List<NavigationObstacle> obstacles = new();
            foreach (var root in roots)
                obstacles.AddRange(root.GetComponentsInChildren<NavigationObstacle>());

            int id = 0;
            while (obstacles.Count > id)
            {
                if ((mask.value & 1 << obstacles[id].gameObject.layer) == 0) obstacles.RemoveAt(id);
                else id++;
            }

            globalBounds = new();
            foreach (var obstacle in obstacles) obstacle.EncapsulateIn(ref globalBounds, surfaceOffset);
            
            return obstacles;
        }
    }
}