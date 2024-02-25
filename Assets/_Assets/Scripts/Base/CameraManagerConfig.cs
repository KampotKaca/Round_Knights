using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    [CreateAssetMenu(menuName = "RoundKnights/General/Camera", fileName = "Camera_Config")]
    public class CameraManagerConfig : ScriptableObject
    {
        [field: SerializeField, FoldoutGroup("Controls")] public float MoveSpeedPerDistance  { get; private set; } = 1.2f;
        [field: SerializeField, FoldoutGroup("Controls")] public float PitchSpeed { get; private set; } = 5f;
        [field: SerializeField, FoldoutGroup("Controls")] public float YawSpeed   { get; private set; } = 5f;
        [field: SerializeField, FoldoutGroup("Controls")] public float ZoomSpeed  { get; private set; } = 5f;
    }
}