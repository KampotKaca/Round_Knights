//#define EDIT_MODE

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class RtsCamera : MonoBehaviour
    {
        [SerializeField, FoldoutGroup("Defaults"), BoxGroup("Defaults/Group", ShowLabel = false), Range(0, 360f)] 
        float defaultYaw = 45f;
        [SerializeField, BoxGroup("Defaults/Group"), Range(0, 180f)] 
        float defaultPitch = 50f;
        [SerializeField, BoxGroup("Defaults/Group"), Range(5f, 80f)] 
        float defaultDistance = 5f;
        
        [SerializeField, FoldoutGroup("Limits"), BoxGroup("Limits/Group", ShowLabel = false)] 
        Vector2 offset = new(0f, -.7f);
        [SerializeField, BoxGroup("Limits/Group")] 
        Vector2 pitchLimit = new(25f, 75f);
        [SerializeField, BoxGroup("Limits/Group")] 
        Vector2 distanceLimit = new(2f, 80f);
        
        [SerializeField, BoxGroup("Limits/Group")] 
        float lerp = 20f;

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
        float m_TargetDistance;

        public float Distance
        {
            get => m_TargetDistance;
            set => m_TargetDistance = Mathf.Clamp(value, distanceLimit.x, distanceLimit.y);
        }
        #endregion
        
        [ShowInInspector, ReadOnly] public Transform Target { get; private set; }

        void LateUpdate()
        {
#if EDIT_MODE
            Distance = defaultDistance;
            Yaw = defaultYaw;
            Pitch = defaultPitch;
#endif

            m_Distance = Mathf.Lerp(m_Distance, m_TargetDistance, lerp * Time.deltaTime);
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

                m_Distance = Distance;
            }
        }

        #region Editor

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (!Target) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Target.position, .5f);
        }
#endif

        #endregion
    }
}