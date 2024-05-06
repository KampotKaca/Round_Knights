using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Navigation
{
    public partial class NavigationSample
    {
        public static void WriteSample(NavigationSample sample, Scene scene, SampleSettings settings)
        {
            sample.m_NodeSize = settings.NodeSize;
            var samples = sampleRenderers(scene, settings.SampleMask, out var globalBounds);
            sample.m_GridDimensions = new(Mathf.CeilToInt(globalBounds.size.x / settings.NodeSize),
                Mathf.CeilToInt(globalBounds.size.y / settings.NodeSize));
        }

        static List<MeshRenderer> sampleRenderers(Scene scene, LayerMask mask, out Bounds globalBounds)
        {
            List<MeshRenderer> rns = new();
            var roots = scene.GetRootGameObjects();

            foreach (var root in roots)
                rns.AddRange(root.GetComponentsInChildren<MeshRenderer>());

            int id = 0;
            while (rns.Count > id)
            {
                if ((mask.value & 1 << rns[id].gameObject.layer) == 0) rns.RemoveAt(id);
                else id++;
            }

            globalBounds = new();
            foreach (var renderer in rns) globalBounds.Encapsulate(renderer.bounds);
            
            return rns;
        }
    }
}