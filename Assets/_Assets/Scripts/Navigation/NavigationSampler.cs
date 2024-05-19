using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace Navigation
{
    public class NavigationSampler : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField] private float m_NodeSize;
        [SerializeField] private LayerMask m_SampleMask;
        
        [Button]
        void takeSample()
        {
            if (Application.isPlaying)
            {
                Debug.LogError("Cannot take sample at runtime");
                return;
            }
            
            var samplePath = SamplePath;
            NavigationSample sample;
            
            if (AssetDatabase.AssetPathExists(samplePath))
                sample = AssetDatabase.LoadAssetAtPath<NavigationSample>(samplePath);
            else
            {
                sample = ScriptableObject.CreateInstance<NavigationSample>();
                AssetDatabase.CreateAsset(sample, samplePath);
            }
            
            NavigationSample.WriteSample(sample, gameObject.scene, new()
            {
                NodeSize = m_NodeSize,
                SurfaceParent = transform,
                SampleMask = m_SampleMask
            });
        }

        string SamplePath
        {
            get
            {
                string sceneFolderPath = Path.GetDirectoryName(gameObject.scene.path);
                return sceneFolderPath == null ? "" : 
                    Path.Combine(sceneFolderPath, $"{gameObject.scene.name}_SurfaceSample.asset");
            }
        }

#endif
    }
}