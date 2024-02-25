using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public class CameraManager : Singleton<CameraManager>, ISavedObject
    {
        [SerializeField] InputReader inputReader;
        [SerializeField] RtsCamera Camera;
        [SerializeField, InlineEditor] CameraManagerConfig config;

        void Load(bool loadDefaults)
        {
            Camera.Load(loadDefaults);

            inputReader.On_CameraMove += OnMoveEvent;
            inputReader.On_CameraRotate += OnRotateEvent;
            inputReader.On_CameraZoom += OnZoomEvent;
            
            Loaded = true;
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
            Load(true);
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
            Camera.Distance = save.Distance;
            Camera.Yaw = save.Yaw;
            Camera.Pitch = save.Pitch;
            Load(false);
            Camera.Target.position = save.TargetPosition.Value;
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
                Distance = Camera.Distance,
                Yaw = Camera.Yaw,
                Pitch = Camera.Pitch,
            };
            save.TargetPosition.SetValue(Camera.Target.position);

            return save;
        }
        #endregion

        #region Events

        Vector2 m_MoveValue;
        void OnMoveEvent(Vector2 v2)
        {
            m_MoveValue = v2;
        }

        Vector2 m_RotateValue;
        void OnRotateEvent(Vector2 v2)
        {
            m_RotateValue = v2;
        }

        void OnZoomEvent(float zoom)
        {
            Camera.Distance += zoom * config.ZoomSpeed * Time.deltaTime;
        }

        void Update()
        {
            Camera.Target.position += Camera.RotateDirection(m_MoveValue.ToV3()) 
                                      * (Camera.Distance * config.MoveSpeedPerDistance * Time.deltaTime);
            Camera.Yaw += config.YawSpeed * m_RotateValue.x * Time.deltaTime;
            Camera.Pitch += config.PitchSpeed * m_RotateValue.y * Time.deltaTime;
        }

        #endregion
    }
}