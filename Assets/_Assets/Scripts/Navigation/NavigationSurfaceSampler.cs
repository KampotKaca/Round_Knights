using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
#endif

namespace Navigation
{
    public class NavigationSurfaceSampler : MonoBehaviour
    {
        [SerializeField, Min(0.01f)] float m_SampleVoxelSize = 3f;
        [SerializeField, InlineEditor] NavigationSample m_Sample;
        [SerializeField] LayerMask m_NavigationMask;
        
        #region Editor

        #if UNITY_EDITOR
        
        [Button]
        public void TakeSurfaceSample()
        {
            if (Application.isPlaying)
            {
                Debug.LogError("Cannot take sample at application runtime!!!");
                return;
            }

            if (!m_Sample)
            {
                var sceneFolderPath = Path.GetDirectoryName(gameObject.scene.path);
                if (sceneFolderPath == null) return;
                
                var samplePath = Path.Combine(sceneFolderPath, $"{gameObject.scene.name}_SurfaceSample.asset");

                m_Sample = ScriptableObject.CreateInstance<NavigationSample>();
                AssetDatabase.CreateAsset(m_Sample, samplePath);
            }

            var bounds = collectBounds();
            m_Sample.SetBaseVariables(bounds, m_SampleVoxelSize);
            
            
            EditorUtility.SetDirty(m_Sample);
        }

        List<Bounds> collectBounds()
        {
            var rns = FindObjectsByType<MeshRenderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            List<Bounds> bounds = new();

            foreach (var rend in rns) if ((m_NavigationMask.value & 1 << rend.gameObject.layer) > 0) bounds.Add(rend.bounds);

            return bounds;
        }
        
        #endif
        
        #endregion
    }
}