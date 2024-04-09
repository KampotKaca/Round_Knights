using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace RoundKnights
{
    public class EntityMovement : MonoBehaviour
    {
        const float DELTA_TIME = .02f;
        const float DELTA_DISTANCE = .01f;
        const float DELTA_SQR_DISTANCE = DELTA_DISTANCE * DELTA_DISTANCE;
        
        [field: SerializeField, InlineEditor] public EntityMovementConfig Config { get; private set; }
        
        public NavMeshAgent Agent { get; private set; }
        public float VelocityPercent { get; private set; }

        public Vector3 TargetPos { get; set; }
        public bool IsMoving { get; private set; }

        bool IsNearEnoughToTarget => (TargetPos - transform.position).sqrMagnitude <= DELTA_SQR_DISTANCE;

        Action m_OnComplete;
        Action m_OnCancel;
        
        public void MoveToTarget(Vector3 targetPos, Action onComplete, Action onCancel)
        {
            TargetPos = targetPos;
            m_OnComplete = onComplete;
            if (IsNearEnoughToTarget)
            {
                m_OnComplete?.Invoke();
                Agent.ResetPath();
                if (m_OnCancel != null)
                {
                    m_OnCancel.Invoke();
                    m_OnCancel = onCancel;
                }
                return;
            }

            Agent.SetDestination(TargetPos);
            if (!IsMoving) StartCoroutine(MovementLoop());
            else
            {
                m_OnCancel.Invoke();
                m_OnCancel = onCancel;
            }
        }

        IEnumerator MovementLoop()
        {
            IsMoving = true;

            while (IsMoving)
            {
                yield return new WaitForSeconds(DELTA_TIME);
            }
            
            m_OnComplete?.Invoke();
            Terminate();
        }

        public void Terminate()
        {
            if (!IsMoving) return;
            IsMoving = false;
            Agent.ResetPath();
            m_OnComplete = null;
            
            if (m_OnCancel == null) return;
            m_OnCancel.Invoke();
            m_OnCancel = null;
        }
        
        #region Save&Load

        public Transform Trs { get; private set; }
        
        [Serializable]
        public struct SaveFile
        {
            public TransformSaveFile Trs;
        }
        
        public void Load()
        {
            collect();
        }
        
        public void Load(ref SaveFile saveFile)
        {
            collect();
            Trs.position = saveFile.Trs.Position;
            Trs.eulerAngles = saveFile.Trs.Euler;
        }

        public SaveFile GetSaveFile()
        {
            SaveFile saveFile = new();
            saveFile.Trs.SetTransform(Trs);
            return saveFile;
        }

        void collect()
        {
            Trs = transform;
            Agent = GetComponent<NavMeshAgent>();

            Agent.speed = Config.Speed;
            Agent.acceleration = Config.Acceleration;
            Agent.angularSpeed = Config.AngularSpeed;
        }
        
        #endregion
    }
}