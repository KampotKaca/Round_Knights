using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RoundKnights
{
    public struct Bounds2D
    {
        public Vector2 Center;
        public Vector2 Size;

        public Vector2 LeftEdge => Center - new Vector2(Size.x * .5f, 0);
        public Vector2 RightEdge => Center + new Vector2(Size.x * .5f, 0);
        public Vector2 DownEdge => Center - new Vector2(0, Size.y * .5f);
        public Vector2 UpEdge => Center + new Vector2(0, Size.y * .5f);
        
        public Vector2 LeftLowerCorner => Center - Size * .5f;
        public Vector2 RightUpperCorner => Center + Size * .5f;
        
        public bool Contains(Vector3 pos) => Contains(new Vector2(pos.x, pos.z));

        public bool Contains(Vector2 pos)
        {
            Vector2 left = LeftLowerCorner, right = RightUpperCorner;
            return pos.x >= left.x && pos.x <= right.x && pos.y >= left.y && pos.y <= right.y;
        }

        public Vector2 Clamp(Vector2 pos)
        {
            Vector2 left = LeftLowerCorner, right = RightUpperCorner;
            return new Vector2(Mathf.Clamp(pos.x, left.x, right.x), 
                               Mathf.Clamp(pos.y, left.y, right.y));
        }

        public Vector3 Clamp(Vector3 pos)
        {
            var newPos = Clamp(new Vector2(pos.x, pos.z));
            return new Vector3(newPos.x, pos.y, newPos.y);
        }
    }
    
    public class CameraManager : Singleton<CameraManager>, ISavedObject
    {
        [SerializeField, FoldoutGroup("General"), BoxGroup("General/Group", ShowLabel = false)] 
        InputReader inputReader;
        [SerializeField, BoxGroup("General/Group")] 
        RtsCamera Camera;
        [SerializeField, BoxGroup("General/Group"), InlineEditor] 
        CameraManagerConfig config;
        
        void Load(bool loadDefaults)
        {
            Camera.Load(loadDefaults);

            inputReader.On_CameraMove += OnMoveEvent;
            inputReader.On_CameraRotate += OnRotateEvent;
            inputReader.On_CameraZoom += OnZoomEvent;
            
            Loaded = true;
        }

        #region Edge Bounds

        [ShowInInspector, BoxGroup("General/Group"), ReadOnly] List<Bounds2D> m_EdgeBounds = new();

        public void AddEdgeBounds(Bounds2D b2D)
        {
            m_EdgeBounds.Add(b2D);
        }

        public void ClearEdgeBounds()
        {
            m_EdgeBounds.Clear();
        }
        
        #endregion

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
            Camera.Target.position = GetTargetPosition();
            Camera.Yaw += config.YawSpeed * m_RotateValue.x * Time.deltaTime;
            Camera.Pitch += config.PitchSpeed * m_RotateValue.y * Time.deltaTime;
        }

        Vector3 GetTargetPosition()
        {
            var targetPos = Camera.Target.position + Camera.RotateDirection(m_MoveValue.ToV3()) 
                                    * (Camera.Distance * config.MoveSpeedPerDistance * Time.deltaTime);
            bool isInBounds = m_EdgeBounds.Count == 0;
            for (int i = 0; i < m_EdgeBounds.Count; i++)
                isInBounds = isInBounds || m_EdgeBounds[i].Contains(targetPos);

            if (!isInBounds)
            {
                Vector3 closePos = Vector3.positiveInfinity;
                float closeDist = float.MaxValue;
                for (int i = 0; i < m_EdgeBounds.Count; i++)
                {
                    Vector3 newEdge = m_EdgeBounds[i].Clamp(targetPos);
                    float dist = (newEdge - targetPos).sqrMagnitude;
                    if (dist < closeDist)
                    {
                        closeDist = dist;
                        closePos = newEdge;
                    }
                }

                targetPos = closePos;
            }
            
            return targetPos;
        }

        #endregion

        #region Editor

#if UNITY_EDITOR

        [SerializeField, FoldoutGroup("Gizmo"), BoxGroup("Gizmo/Group", ShowLabel = false)] 
        bool drawGizmo;
        [SerializeField, BoxGroup("Gizmo/Group"), ShowIf(nameof(drawGizmo))] 
        Color gizmoColor = Color.red;
        
        void OnDrawGizmos()
        {
            if (!drawGizmo) return;

            Gizmos.color = gizmoColor;
            for (int i = 0; i < m_EdgeBounds.Count; i++)
            {
                Gizmos.DrawWireCube(m_EdgeBounds[i].Center.ToV3(), m_EdgeBounds[i].Size.ToV3());
            }
        }

#endif

        #endregion
    }
}