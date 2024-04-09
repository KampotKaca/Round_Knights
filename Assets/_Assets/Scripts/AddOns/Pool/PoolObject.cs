using UnityEngine;

namespace RoundKnights
{
    public abstract class PoolObject : MonoBehaviour
    {
        public abstract string PoolKey { get; }
        public bool IsBeingUsed { get; private set; }
        public GameObject GM { get; private set; }
        public Transform Trs { get; private set; }
        
        protected virtual void OnCreate() {}
        public void Create()
        {
            GM = gameObject;
            Trs = transform;
            GM.SetActive(false);
            IsBeingUsed = false;
            OnCreate();
        }
        
        protected virtual void OnSpawn() {}
        public void Spawn()
        {
            IsBeingUsed = true;
            GM.SetActive(true);
            OnSpawn();
        }
        
        protected virtual void OnReturn() {}
        public void Return()
        {
            IsBeingUsed = false;
            GM.SetActive(false);
            OnReturn();
        }
    }
}