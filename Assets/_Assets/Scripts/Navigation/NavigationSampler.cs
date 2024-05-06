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

        [SerializeField] float m_NodeSize;
        [SerializeField] LayerMask m_SampleMask;
        
        [Button]
        void takeSample()
        {
            if (Application.isPlaying)
            {
                Debug.LogError("Cannot take sample at runtime");
                return;
            }
            
            var samplePath = SamplePath;
            NavigationSample m_Sample;
            
            if (AssetDatabase.AssetPathExists(samplePath))
                m_Sample = AssetDatabase.LoadAssetAtPath<NavigationSample>(samplePath);
            else
            {
                m_Sample = ScriptableObject.CreateInstance<NavigationSample>();
                AssetDatabase.CreateAsset(m_Sample, samplePath);
            }
            
            NavigationSample.WriteSample(m_Sample, gameObject.scene, new()
            {
                NodeSize = m_NodeSize
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