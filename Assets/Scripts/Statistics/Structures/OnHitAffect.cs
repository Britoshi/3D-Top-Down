using Game.Buff;
using Game.Utilities;
using System;
using System.ComponentModel;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class OnHitAffect
    {
        public AffectTarget affectTarget;
        /// <summary>
        /// On Hit GameObject prefabs must, MUST has a script called OnHitAffectHandler.
        /// If this field is null, it means it spawns nothing and the onKill affect will apply directly.
        /// </summary> 
        public GameObject onHitGameObjectPrefab;
        public AffectTarget spawnAtTarget;
        public ResourceAffect resourceAffect;

        /// <summary>
        /// This will just apply to the target.
        /// </summary>
        public StatusBuff buff;

        public OnHitAffect(AffectTarget target, ResourceAffect affect, GameObject prefab, AffectTarget spawnTarget, StatusBuff buff)
        {
            affectTarget = target;
            onHitGameObjectPrefab = prefab;
            spawnAtTarget = spawnTarget;
            resourceAffect = affect;
            this.buff = buff;
        }


        public void ApplyOnGetHit(Entity owner, Entity source) =>
            ApplyOnHit(source, owner); 

        public void ApplyOnHit(Entity owner, Entity target)
        {
            var targetEntity = affectTarget == AffectTarget.Self ? owner : target;
             
            if (onHitGameObjectPrefab != null) 
            {
                var spawnTargetEntity = spawnAtTarget == AffectTarget.Self ? owner : target;
                throw new System.NotImplementedException();
                //Util.SpawnOnHitObject(this, spawnTargetEntity.Position);
            }
            else
            { 
                if (resourceAffect != null) 
                {
                    if (resourceAffect is HealthAffect) 
                        EntityUtil.Modify(targetEntity, HealthModificationData.OnHit(owner, targetEntity, resourceAffect as HealthAffect));  
                    else targetEntity.status.ApplyResourceAffect(owner, resourceAffect);
                }
            }
            buff?.Apply(owner, targetEntity);
        }

        public void ApplyOnKill(Entity killer, Entity victim) =>
            ApplyOnHit(killer, victim); 

        public static OnHitAffect Buff(AffectTarget target, StatusBuff buff) => new(target, null, null, 0, buff);
        public static OnHitAffect AOEBuff(AffectTarget target, GameObject prefab, StatusBuff buff) => new(target, null, prefab, 0, buff);
        public static OnHitAffect Simple(AffectTarget target, ResourceAffect affect) =>
            new(target, affect, null, 0, null);
        public static OnHitAffect AOE(AffectTarget target, ResourceAffect affect, GameObject prefab, AffectTarget spawnTarget) =>
            new(target, affect, prefab, spawnTarget, null);

    }
}