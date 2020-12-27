using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Weaponry.Impacts
{
    public class DecalImpact : ProjectileImpact
    {
        private ParticleSystem _decalEmitter;

        void Start()
        {
            _decalEmitter = this.GetOrThrow<ParticleSystem>();
        }

        public override void OccurAt(ParticleCollisionEvent collisionEvent)
        {
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            emitParams.position = collisionEvent.intersection;
            emitParams.rotation3D = Quaternion.LookRotation(collisionEvent.normal).eulerAngles;
            _decalEmitter.Emit(emitParams, count: 1);
        }
    }
}