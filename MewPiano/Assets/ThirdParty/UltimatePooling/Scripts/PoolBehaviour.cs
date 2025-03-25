using UnityEngine;

namespace UltimatePooling
{
    /// <summary>
    /// Intermediate behaviour script that allows spawn and despawn events to be received.
    /// The events are broadcast to the object that is being re-used so the script can be at any level on the objects hierarchy.
    /// </summary>
    public class PoolBehaviour : MonoBehaviour, IPoolReceiver
    {
        // Public
        /// <summary>
        /// The name of the event that is called when an object is spawned from the pool.
        /// </summary>
        public static string monoSpawnedEvent = typeof(IPoolReceiver).GetMethods()[0].Name;

        /// <summary>
        /// The name of the event that is called when an object is returned to the pool.
        /// </summary>
        public static string monoDespawnedEvent = typeof(IPoolReceiver).GetMethods()[1].Name;

        // Methods
        /// <summary>
        /// Called by the managing pool to notify that this object has just been recycled from the pool.
        /// This method will not be called when the object is created for the first time.
        /// </summary>
        public virtual void OnSpawned(PoolGroup pool)
        {
            // Do nothing
        }

        /// <summary>
        /// Called by the managing pool to notify that this object is about to be returned to the pool.
        /// This method will not be called when the object is about to be destroyed.
        /// </summary>
        public virtual void OnDespawned(PoolGroup pool)
        {
            // Do nothing
        }
    }
}
