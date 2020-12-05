using UnityEngine;

namespace Assets.Scripts.Weaponry.Impacts
{
    public class BombImpact : ProjectileImpact
    {
        public ParticleSystem bombExplosionEmitter;
        public ParticleSystem decalEmitter;

        void Start()
        {
            if (bombExplosionEmitter == null)
            {
                Debug.LogError($"{this.GetType().Name} ({name}) has a null {nameof(bombExplosionEmitter)}.");
            }

            if (decalEmitter == null)
            {
                Debug.LogError($"{this.GetType().Name} ({name}) has a null {nameof(decalEmitter)}.");
            }
        }

        public override void OccurAt(ParticleCollisionEvent collisionEvent)
        {
            bombExplosionEmitter.transform.position = collisionEvent.intersection;
            bombExplosionEmitter.transform.rotation = Quaternion.LookRotation(collisionEvent.normal);
            bombExplosionEmitter.Emit(10);
            
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            emitParams.position = collisionEvent.intersection;
            emitParams.rotation3D = Quaternion.LookRotation(collisionEvent.normal).eulerAngles;
            decalEmitter.Emit(emitParams, 1);
        }
    }
}