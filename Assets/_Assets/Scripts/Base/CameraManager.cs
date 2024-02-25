#define EDIT_MODE
using System;
using UnityEngine;

namespace RoundKnights
{
    public class CameraManager : Singleton<CameraManager>, ISavedObject
    {
        [field:SerializeField] public Camera MainCamera { get; private set; }
        [SerializeField, Range(0, 360f)] float defaultYaw = 150f;
        [SerializeField, Range(0, 180f)] float defaultPitch = 50f;
        [SerializeField, Range(5f, 80f)] float defaultDistance = 14f;
        [SerializeField] Vector2 offset = new(0f, -.7f);
        [SerializeField] Vector2 pitchLimit = new(25f, 75f);
        [SerializeField] Vector2 distanceLimit = new(2f, 80f);
        
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
        
        public Transform Target { get; private set; }

        public void SetTarget(Transform trs)
        {
            Target.SetParent(trs);
            Target.localPosition = Vector3.zero;
        }

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

        void Load()
        {
            Target = new GameObject("Camera_Target").transform;
            Loaded = true;
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
            return transform.rotation * direction;
        }

        #region Save&Load
        public Type FileType => typeof(SaveFile);
        public int Queue => (int)SaveFileQueue.PostLoad;
        public string SaveFileIdentifier => "Camera_Manager";
        
        [Serializable]
        public struct SaveFile
        {
            public Vec3SaveFile TargetPosition;
            
            public float Distance;
            public float Pitch;
            public float Yaw;
        }
        
        public bool Loaded { get; private set; }
        
        public void LoadDefault()
        {
#if UNITY_EDITOR
            if (Loaded)
            {
                Debug.LogError("Trying to load camera twice");
                return;
            }
#endif
            Distance = defaultDistance;
            Yaw = defaultYaw;
            Pitch = defaultPitch;
            Load();
            Target.position = Vector3.zero;
        }

        public void LoadFromSaveFile(object saveFile)
        {
#if UNITY_EDITOR
            if (Loaded)
            {
                Debug.LogError("Trying to load camera twice");
                return;
            }
#endif
            var save = (SaveFile)saveFile;
            Distance = save.Distance;
            Yaw = save.Yaw;
            Pitch = save.Pitch;
            Load();
            Target.position = save.TargetPosition.Value;
        }

        public object GetSaveFile()
        {
#if UNITY_EDITOR
            if (!Loaded)
            {
                Debug.LogError("Trying to save camera when it's not loaded");
                return null;
            }
#endif
            var save = new SaveFile
            {
                Distance = Distance,
                Yaw = Yaw,
                Pitch = Pitch,
            };
            save.TargetPosition.Value = Target.position;

            return save;
        }
        #endregion
    }
}