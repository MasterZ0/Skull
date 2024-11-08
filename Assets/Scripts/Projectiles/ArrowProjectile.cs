﻿using AdventureGame.BattleSystem;
using AdventureGame.ObjectPooling;
using AdventureGame.Audio;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AdventureGame.Projectiles
{
    public class ArrowProjectile : Projectile
    {
        [Title("Arrow")]
        [SerializeField] private new Collider collider;
        [SerializeField] private SoundReference hitKillSFX; // Maybe an event is sufficient

        private float delayToDisapear;

        private void Awake()
        {
            rigidbody.centerOfMass = transform.position;
        }

        private void OnEnable()
        {
            rigidbody.isKinematic = false;
        }

        public void Shoot(Damage damage, float velocity, float delayToDisapear)
        {
            this.delayToDisapear = delayToDisapear;
            Shoot(damage, velocity);
        }

        protected override void AfterHit(TargetHitType targetHit, Vector3 contact)
        {
            if (targetHit == TargetHitType.Unknown)
            {
                transform.position = transform.forward * -collider.bounds.size.z + contact;
                rigidbody.isKinematic = true;
                StartCoroutine(DelayToDisapear());
            }
            else if (targetHit == TargetHitType.Alive)
            {
                Impact();
            }
            else
            {
                hitKillSFX.PlaySound(transform);
            }
        }

        private IEnumerator DelayToDisapear()
        {
            yield return new WaitForSeconds(delayToDisapear);
            this.ReturnToPool();
        }
    }
}
