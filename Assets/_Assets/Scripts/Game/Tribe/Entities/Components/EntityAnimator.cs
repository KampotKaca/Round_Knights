using UnityEngine;

namespace RoundKnights
{
    public class EntityAnimator : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        public Entity Entity { get; private set; }
        
        void OnEnable()
        {
            Entity = GetComponentInParent<Entity>();
            Animator = GetComponent<Animator>();

            Animator.Rebind();
            Animator.Update(0f);
        }

        void LateUpdate()
        {
            SpeedControl();
        }

        #region AnimCalls

        static readonly int Attack = Animator.StringToHash("Attack");
        static readonly int Die = Animator.StringToHash("Die");
        static readonly int DeathState = Animator.StringToHash("DeathState");
        static readonly int Speed = Animator.StringToHash("Speed");

        public void DoAttack() => Animator.SetTrigger(Attack);
        public void DoDeath()
        {
            Animator.SetInteger(DeathState, Random.Range(0, 2));
            Animator.SetTrigger(Die);
        }
        
        void SpeedControl()
        {
            Animator.SetFloat(Speed,  Entity.Movement.VelocityPercent);
        }

        #endregion
    }
}