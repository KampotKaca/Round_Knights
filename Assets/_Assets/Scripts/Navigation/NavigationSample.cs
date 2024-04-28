using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Navigation
{
    [CreateAssetMenu(menuName = "Navigation/Sample")]
    public class NavigationSample : ScriptableObject
    {
        [ReadOnly] public Bounds FullBounds;
        [ReadOnly] public Vector3Int VoxelCount;
        [SerializeField, ReadOnly] NavigationVoxel[] m_Voxels;

        public NavigationVoxel this[int x, int y, int z] => m_Voxels[x * VoxelCount.y * VoxelCount.z + y * VoxelCount.z + z];
        
        public void SetBaseVariables(List<Bounds> bounds, float sampleSize)
        {
            FullBounds = calculateFullBounds(bounds);
            var size = FullBounds.size;
            VoxelCount = new Vector3Int(Mathf.Max(1, Mathf.CeilToInt(size.x / sampleSize)), 
                                        Mathf.Max(1, Mathf.CeilToInt(size.y / sampleSize)),
                                        Mathf.Max(1, Mathf.CeilToInt(size.z / sampleSize)));
            m_Voxels = new NavigationVoxel[VoxelCount.x * VoxelCount.y * VoxelCount.z];
        }
        
        Bounds calculateFullBounds(List<Bounds> bounds)
        {
            Bounds fullBounds = new();
            foreach (var b in bounds) fullBounds.Encapsulate(b);
            return fullBounds;
        }
    }

    [Serializable]
    public class NavigationVoxel
    {
        
    }
}