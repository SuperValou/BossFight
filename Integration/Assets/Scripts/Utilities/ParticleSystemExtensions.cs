using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public static class ParticleSystemExtensions
    {
        public static ICollection<ParticleCollisionEvent> GetCollisionsWith(this ParticleSystem particleSystem, GameObject collidingGameObject)
        {
            List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();
            ParticlePhysicsExtensions.GetCollisionEvents(particleSystem, collidingGameObject, events);
            return events;
        }
    }
}