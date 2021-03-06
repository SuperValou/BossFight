﻿using Assets.Scripts.Damages;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Projectiles
{
    public class BeamBullet : Projectile
    {
        private ParticleSystem _particleSystem;
        private SphereCollider _collider;

        protected override void Start()
        {
            base.Start();
            _particleSystem = this.GetComponent<ParticleSystem>();
            _collider = this.GetComponent<SphereCollider>();
        }

        protected override void AfterContact()
        {
            // let the beam die
            this.Rigidbody.velocity = Vector3.zero;
            this.Rigidbody.detectCollisions = false;
            _collider.enabled = false;
            _particleSystem.Stop();
        }
    }
}