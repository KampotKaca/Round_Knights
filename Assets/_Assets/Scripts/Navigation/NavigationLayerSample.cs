using System;
using System.Collections;
using System.Collections.Generic;
using RoundKnights;
using UnityEditor;
using UnityEngine;

namespace Navigation
{
    [CreateAssetMenu(menuName = "Navigation/LayerSample", fileName = "NavigationLayerSample")]
    public class NavigationLayerSample : SingletonScriptableObject<NavigationLayerSample>
    {
        [Serializable]
        public struct LayerSample
        {
            public string LayerName;
            public int LayerWeight;

            public bool IsUsed => LayerName != string.Empty;
        }
        
        [SerializeField] LayerSample[] m_Samples = new LayerSample[32];
        
        
        public int GetWeight(int layerId) => m_Samples[layerId].LayerWeight;

        public static int ToBitwiseId(int layerId) => 1 << layerId;
    }
}