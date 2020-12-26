using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public static class ParticleSystemExtensions
    {
        public static ICollection<ParticleCollisionEvent> GetCollisionsWith(this ParticleSystem particleSystem, GameObject collidingGameObject)
        {
            List<ParticleCollisionEvent> eventList = new List<ParticleCollisionEvent>();
            int validItemCount = ParticlePhysicsExtensions.GetCollisionEvents(particleSystem, collidingGameObject, eventList);
            var events = eventList.Take(validItemCount).ToList();
            return events;
        }
    }
}