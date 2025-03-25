

namespace UltimatePooling
{
    /// <summary>
    /// Implement this interface when you want to receive spawned and despawned events sent to the pooled object.
    /// This interface will typically be implemented by a mono behaviour script that is attached to a pooled object which will then receive the appropriate event when it is spawned or despawned.
    /// Alternativley you can inherit from <see cref="PoolBehaviour"/> which provides default overridable behaviour for these events. (Modifies the objects enabled state to show or hide the object).
    /// </summary>
    public interface IPoolReceiver
    {
        // Methods
        /// <summary>
        /// Called when an object has been spawned from the pool. 
        /// This event allows the state of the object to be reset so it can be treaded as a newly created object.
        /// Note that this method will not be called when the object is first created. 
        /// </summary>
        /// <param name="pool"></param>
        void OnSpawned(PoolGroup pool);

        /// <summary>
        /// Called when an object is about to be returned to the pool.
        /// Note that this method will not be called when the object is destroyed.
        /// </summary>
        /// <param name="pool"></param>
        void OnDespawned(PoolGroup pool);
    }
}
