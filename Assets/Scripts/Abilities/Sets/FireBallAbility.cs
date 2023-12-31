/*
using Game.Projectiles; 
using UnityEngine;

namespace Game.Abilities
{
    public class FireBallAbility : BaseAbility
    {
        public FireBallAbility(EntityBase owner) : 
            base(owner, "Fire Ball", CooldownOn.START, Cooldown.Create(3f), AbilityCost.None)
        {

            AddEvent(.3f, SpawnHitbox);
        }


        void DamageEnemy(BaseProjectile projectile, EntityBase target)
        {
            target.InflictDamage(DamageType.Normal, 10);
            if(target.GetType().IsSubclassOf(typeof(KineticEntity)))
            {
                throw new System.NotImplementedException(); 
            }
        }

        void OnProjectileDeath(BaseProjectile projectile)
        {

        }

        void MovementBehavior(BaseProjectile projectile, Vector2 target, float speed)
        {
            projectile.transform.position = Vector2.Lerp(projectile.transform.position, target, speed * Time.deltaTime);
        }
        void LostTargetBehavior(BaseProjectile projectile, Vector2 target, float speed)
        {

        }

        void SpawnHitbox()
        {
            var offsetPosition = new Vector2(1 * Owner.FacingDir, 1f);
            
            var spawnPosition = offsetPosition + (Vector2)Owner.transform.position;

            Vector2 target = Vector2.right * (offsetPosition.x * 5f + spawnPosition.x);

            target += spawnPosition.y * Vector2.up;

            float speed = 5f;
            LayerMask mask = LayerMask.GetMask(new string[] { "Entity" });

            var projectile = Resources.InstantiateSingleTargetProjectile(Owner, "test projectile", spawnPosition, Quaternion.identity, layerMask: mask, target, speed, 5f,
                DamageEnemy, MovementBehavior, LostTargetBehavior, OnProjectileDeath);

            
            //projectile.SetVelocity(Vector2.right * Owner.FacingDir * 500f * Time.fixedDeltaTime);
        } 

        public override void OnAnimationStart(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnAnimationStart(animator, stateInfo, layerIndex); 
        }

        public override void OnAnimationUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnAnimationUpdate(animator, stateInfo, layerIndex); 
        }

        public override void OnAnimationEnd(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnAnimationEnd(animator, stateInfo, layerIndex);
        }
    }
}
*/