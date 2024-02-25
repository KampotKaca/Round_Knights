//#define EDIT_MODE
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class RtsCamera : MonoBehaviour
    {
        [SerializeField, FoldoutGroup("Defaults"), Range(0, 360f)] float defaultYaw = 45f;
        [SerializeField, FoldoutGroup("Defaults"), Range(0, 180f)] float defaultPitch = 50f;
        [SerializeField, FoldoutGroup("Defaults"), Range(5f, 80f)] float defaultDistance = 5f;
        
        [SerializeField, FoldoutGroup("Limits")] Vector2 offset = new(0f, -.7f);
        [SerializeField, FoldoutGroup("Limits")] Vector2 pitchLimit = new(25f, 75f);
        [SerializeField, FoldoutGroup("Limits")] Vector2 distanceLimit = new(2f, 80f);
        
        #region Controls
        float m_Pitch;
        public float Pitch 
        { 
            get => m_Pitch;
            set => m_Pitch = Mathf.Clamp(value, pitchLimit.x, pitchLimit.y);
        }

        float m_Yaw;
        public float Yaw
        {
            get => m_Yaw;
            set => m_Yaw = value % 360f;
        }

        float m_Distance;

        public float Distance
        {
            get => m_Distance;
            set => m_Distance = Mathf.Clamp(value, distanceLimit.x, distanceLimit.y);
        }
        #endregion
        
        public Transform Target { get; private set; }

        void LateUpdate()
        {
#if EDIT_MODE
            Distance = defaultDistance;
            Yaw = defaultYaw;
            Pitch = defaultPitch;
#endif
            
            var off = CalculateOffset();

            var trs = transform;
            trs.rotation = off.Rotation;
            trs.position = Target.position + off.Offset;
        }
        
        (Vector3 Offset, Quaternion Rotation) CalculateOffset()
        {
            float yawRad = Mathf.Deg2Rad * m_Yaw;
            float pitchRad = Mathf.Deg2Rad * (Pitch - 90f) % 180f;

            Vector3 dir = new Vector3
            {
                x = Mathf.Cos(pitchRad) * Mathf.Sin(yawRad),
                y = Mathf.Sin(pitchRad),
                z = Mathf.Cos(pitchRad) * Mathf.Cos(yawRad)
            };

            Vector3 off = Vector3.zero;
            off.y += offset.y;
            var rot = Quaternion.LookRotation(dir);

            return new()
            {
                Offset = rot * Vector3.right * offset.x + off - dir * m_Distance,
                Rotation = rot,
            };
        }

        public Vector3 RotateDirection(Vector3 direction)
        {
            return Quaternion.AngleAxis(Yaw, Vector3.up) * direction;
        }
        
        public void Load(bool setDefaults)
        {
            Target = new GameObject("Camera_Target").transform;
            if (setDefaults)
            {
                Distance = defaultDistance;
                Yaw = defaultYaw;
                Pitch = defaultPitch;
            }
        }
    }
}