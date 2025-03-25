using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Object = UnityEngine.Object;

namespace UltimatePooling
{
    /// <summary>
    /// The method that is used to inform pooled objects about their current spawn state. 
    /// </summary>
    public enum PoolEventType
    {
        /// <summary>
        /// Broadcast a message to the game object and all scripts with a matching listener method will be informed.
        /// </summary>
        BroadcastMessage,
        /// <summary>
        /// 
        /// </summary>
        InterfaceCallback,
    }

    /// <summary>
    /// Represents a spawn pool for a specfific type of prefab.
    /// Valid types are game objects and components. 
    /// </summary>
    public abstract class PoolGroup : MonoBehaviour
    {
        // Private
        private bool isPrewarning = false;

        // Protected
        /// <summary>
        /// A collection of objects that have been spawned by this pool.
        /// </summary>
        protected HashSet<Object> tracked = new HashSet<Object>();
        /// <summary>
        /// A collection of objects that are ready to be spawned.
        /// </summary>
        protected Stack<Object> pooled = new Stack<Object>();

        // Public
        /// <summary>
        /// Should the pool preload a set number of objects at startup.
        /// This can avoid frame spikes cause by calls to 'Instantiate' but may increase loading time.
        /// </summary>
        [Tooltip("Should the pool load a number of objects at startup")]
        public bool prewarmPool = true;

        /// <summary>
        /// The amount of inctances to preload.
        /// </summary>
        [Tooltip("How many objects should the pool load at startup")]
        [Range(0, 100)]
        public int prewarmAmount = 20;

        /// <summary>
        /// The max amount of instances to preload per frame.
        /// </summary>
        [Tooltip("How many objects should be loaded per frame to avoid load spikes")]
        public int prewarmPerFrame = 10;

        /// <summary>
        /// The max amount of instances that the pool can contain. If this amount is exceeded then the pool will need to destroy some objects.
        /// </summary>
        [Tooltip("The maximum number of objects that can exist in the pool before culling is applied")]
        public int maxAmount = 100;

        /// <summary>
        /// When true, all spawned instances will be added as child objects to the managing pool group.
        /// </summary>
        [Tooltip("Should the pooled objects be parented to the PoolGroup game object")]
        public bool parentInstances = true;

        /// <summary>
        /// The method used to inform a spawned instance when it is added to or removed from the pool.
        /// </summary>
        [Tooltip("How should events be sent to pooled objects")]
        public PoolEventType eventType = PoolEventType.BroadcastMessage;

        // Properties
        /// <summary>
        /// Returns true if the pool is unable to store anymore pooled instances.
        /// </summary>
        public bool IsFull
        {
            get { return pooled.Count >= maxAmount; }
        }

        /// <summary>
        /// Returns true if the pool is currently prewarming.
        /// </summary>
        public bool IsPrewarming
        {
            get { return isPrewarning; }
        }

        /// <summary>
        /// Should be implemented by the inheritng class.
        /// Should return the specific prefab type, For example 'GameObject'.
        /// </summary>
        public abstract Object Prefab { get; set; }

        // Methods
        /// <summary>
        /// Should be implemented by the inheriting class.
        /// Called when the object has been taken from the pool and will be re-used.
        /// </summary>
        /// <param name="instance">The object that has been re-used</param>
        /// <param name="type">The event type that should be used to inform the object that it has been spawned</param>
        /// <param name="position">The position to spawn the object at</param>
        /// <param name="rotation">The rotaiton to spawn the object with</param>
        protected abstract void onInstanceSpawned(Object instance, PoolEventType type, Vector3 position, Quaternion rotation);

        /// <summary>
        /// Should be implemented by the inheriting class.
        /// Called when the object is about to be returned to the pool.
        /// </summary>
        /// <param name="instance">The object that is about to be pooled</param>
        /// <param name="type">The event type that should be used to inform the object that it is about to be despawned</param>
        protected abstract void onInstanceDespawned(Object instance, PoolEventType type);
        
        /// <summary>
        /// Called by Unity on the first frame.
        /// </summary>
        protected virtual void Start()
        {
            // Register this pool
            UltimatePool.Pools.addPool(this);

            // Start loading the objects
            if(prewarmPool == true)
                StartCoroutine(prewarmRoutine(prewarmAmount, prewarmPerFrame));
        }

        private void Update()
        {
            // Shrink the pool if we have too many instances
            if (pooled.Count > maxAmount)
            {
                // Make sure the pool is not empty
                if (pooled.Count > 0)
                {
                    // Look at the top of the stack
                    Object instance = pooled.Peek();

                    // Destroy 1 per frame until we reach our target
                    destroy(instance);

                    // Keep the stack clean
                    pooled.Pop();
                }
            }
        }

        /// <summary>
        /// Called by Unity when the pool has been destroyed.
        /// </summary>
        private void OnDestroy()
        {
            // Unregister this pool
            UltimatePool.Pools.removePool(this);
        }

        /// <summary>
        /// Spawn an instance from the pool.
        /// </summary>
        /// <returns>An instance of a pooled object</returns>
        public Object spawn()
        {
            // Call through
            return spawn(Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Spawn an instance from the pool using the specified position and rotation.
        /// </summary>
        /// <param name="position">The position in 3D space to place the spawned object</param>
        /// <param name="rotation">The initial rotation to give the spawned object</param>
        /// <returns>An instance of a pooled object</returns>
        public Object spawn(Vector3 position, Quaternion rotation)
        {
            // Check for pooled
            Object instance = null;

            // Loop until we find a pool instance
            while (pooled.Count > 0)
            {
                // Get a pooled instance from the stack
                instance = pooled.Pop();

                // Loop until we find a valid instance - The user could have called 'Destroy' on a pooled instance resulting in our references being set to null.
                if (instance != null)
                    break;
            }

            // Make sure we have a pooled object
            if (instance == null)
            {
                // Check for parenting
                Transform parent = (parentInstances == true) ? transform : null;

                // We need to manually instantiate because the pool is empty
                instance = PoolUtil.instantiate(Prefab, position, rotation, parent);
            }
            else
            {
                // Trigger the 'OnSpawned' event with the specified transform parameters
                onInstanceSpawned(instance, eventType, position, rotation);
            }

            // Track the instance
            tracked.Add(instance);

            return instance;
        }

        /// <summary>
        /// Indicates that the specified instance can be returned to the pool and re-used at a later time.
        /// </summary>
        /// <param name="instance">The instance to despawn</param>
        public void despawn(Object instance)
        {
            // Make sure this pool is responsible for this instance
            if (didSpawn(instance) == false)
            {
                PoolUtil.logWarning("Spawn Group is attempting to despawn a pooled instance that it did not create");
                return;
            }

            // Remove from the tracked list
            tracked.Remove(instance);

            // Check for too many objects
            if (IsFull)
            {
                // Destroy the instance because the pool size is at its upper limit
                PoolUtil.destroy(instance);
            }
            else
            {
                // Trigger the 'OnDespawn' event
                onInstanceDespawned(instance, eventType);

                // Add to pooled
                pooled.Push(instance);
            }
        }

        /// <summary>
        /// Indicates that the specified instance can be returned to the pool and re-used at a later time.
        /// </summary>
        /// <param name="instance">The instance to despawn</param>
        /// <param name="time">The amount of time to wait before the object is despawned</param>
        public void despawn(Object instance, float time)
        {
            // Make sure this pool is responsible for this instance
            if (didSpawn(instance) == false)
            {
                PoolUtil.logWarning("Pool Group is attempting to despawn a pooled instance that it did not create");
                return;
            }

            // Run the despawn routine
            StartCoroutine(delayedDespawnRoutine(instance, time));
        }

        /// <summary>
        /// Attempts to reclaim all instances spawned by this pool and return them to the pool.
        /// Any instances spawned by this pool will be forcefully returned without warning.
        /// </summary>
        public void despawnAll()
        {
            // Process each instance spawned by this pool
            foreach (Object instance in tracked)
            {
                // The user could have called destroy on any of these spawned objects.
                if(instance == null)
                    continue;

                // Despawn the instance - We could call 'Despawn' but that method provides unnecessary error checking that will surely slow us down
                // Check for too many objects
                if (IsFull == true)
                {
                    // Destroy the instance because the pool size is at its upper limit
                    PoolUtil.destroy(instance);
                }
                else
                {
                    // Trigger the 'OnDespawn' event
                    onInstanceDespawned(instance, eventType);

                    // Add to pooled
                    pooled.Push(instance);
                }
            }

            // We can safely empty the tracked list now
            tracked.Clear();
        }

        /// <summary>
        /// Attempts to reclaim all instances spawned by this pool after the specified time delay.
        /// Any instances spawned by this pool will be forcefully returned without warning.
        /// </summary>
        /// <param name="time">The amount of time to wait before despawning all instances</param>
        public void despawnAll(float time)
        {
            // Run the despawn all routine
            StartCoroutine(delayedDespawnAllRoutine(time));
        }

        /// <summary>
        /// Returns true if this spawn group created the instance specified.
        /// Useful for spawn validation to make sure multiple pools are not attempting to manage the same instance.
        /// </summary>
        /// <param name="instance">The instance to check</param>
        /// <returns>True if this pool spawned the specified instance otherwise false</returns>
        public bool didSpawn(Object instance)
        {
            PoolUtil.validateReference(instance, "instance");

            // Check if this instance was spawned by this pool
            return tracked.Contains(instance);
        }

        /// <summary>
        /// Attempts to destroy a specific instance from the pool.
        /// Note that 'OnDespawn' will not be called on the instance. Instead you should handle any cleanup in 'OnDestroy'
        /// </summary>
        /// <param name="instance">The instance to remove from the pool</param>
        /// <param name="keepSpawnedInstances">If true, the pool will also try to locate this instance in its spawned list</param>
        public void destroy(Object instance, bool keepSpawnedInstances = true)
        {
            PoolUtil.validateReference(instance, "instance");
            bool found = false;

            // Check the pooled objects
            foreach (Object val in pooled)
            {
                // Check for the instance
                if (instance == val)
                {
                    // Destroy the instance - We can leave the null reference in the stack and it will be handled when it is popped. It would be too costly to walk the stack and remove the element out of order.
                    PoolUtil.destroy(val);
                    found = true;
                    break;
                }
            }

            if (found == false && keepSpawnedInstances == false)
            {
                // Check for existing instance
                if (tracked.Contains(instance) == true)
                {
                    // Destroy the instance and remove it from the tracked list
                    PoolUtil.destroy(instance);
                    tracked.Remove(instance);
                }
            }
        }

        /// <summary>
        /// Attempts to destroy all pooled objects effectivley emptying the pool and resetting its state. 
        /// Note that 'OnDespawn' will not be called on the pooled objects. Instead you should handle any cleanup in 'OnDestroy'
        /// </summary>
        /// <param name="keepSpawnedInstances">If true, all spawned objects created by this pool will also be destroyed</param>
        public void destroyAll(bool keepSpawnedInstances = true)
        {
            // Destroy the pooled instances
            foreach (Object val in pooled)
            {
                // Hard destroy the object
                PoolUtil.destroy(val);
            }

            if (keepSpawnedInstances == false)
            {
                // Destory the spawned instances
                foreach (Object val in tracked)
                {
                    // Hard destroy the object
                    PoolUtil.destroy(val);
                }
            }
        }

        /// <summary>
        /// Attempts to destroy all pooled objects effectivley emptying the pool as well as destroying the pool instance.
        /// This is the prefered way of destroying an object pool as it allows the spawned objects to remain in the scene if required as opposed to being destroyed along with the pool.
        /// Note that 'OnDespawn' will not be called on any of the pooled objects. Instead you should handle any cleanup in 'OnDestroy'
        /// </summary>
        /// <param name="keepSpawnedInstances">When true, the pool will avoid destroying its parent object if the objects spawned by this pool are parented to it. This allows them to remain in the scene even though the pool will be destroyed</param>
        public void destroySelf(bool keepSpawnedInstances = true)
        {
            // Destroy the pooled instances
            foreach (Object val in pooled)
            {
                // Hard destroy the object
                PoolUtil.destroy(val);
            }

            // Check if we need to keep all spawned objects
            if (keepSpawnedInstances == true)
            {
                // Destory the poolgroup component and not the object
                if (parentInstances == true && tracked.Count > 0)
                {
                    // Destroy just the script because the game object is acting as a folder for all spawned objects
                    PoolUtil.destroy(this);
                    gameObject.name = "[Destroyed] " + gameObject.name;
                }
                else
                {
                    // We can safely destroy the game object knowing that all spawned objects are not parented to this game object
                    PoolUtil.destroy(gameObject);
                }
            }
            else
            {
                // Destory the spawned instances
                foreach (Object val in tracked)
                {
                    // Hard destroy the object
                    PoolUtil.destroy(val);
                }

                // Destroy the pool
                PoolUtil.destroy(gameObject);
            }
        }

        private IEnumerator prewarmRoutine(int amount, int amountPerFrame)
        {
            isPrewarning = true;
            int spawned = 0;

            // Loop untial we have spawned enough objects
            while (spawned < amount)
            {
                // Spawn the designated amount for this frame
                for (int i = 0; i < amountPerFrame; i++)
                {
                    // Spawn
                    // Check for parenting
                    Transform parent = (parentInstances == true) ? transform : null;

                    // We need to manually instantiate because the pool is empty
                    Object instance = PoolUtil.instantiate(Prefab, Vector3.zero, Quaternion.identity, parent, false);

                    // Pool the instance
                    pooled.Push(instance);
                    spawned++;

                    // Check if we have reached the amount
                    if (spawned == amount)
                        break;
                }

                // Wait for next frame
                yield return null;
            }

            isPrewarning = false;
        }

        private IEnumerator delayedDespawnRoutine(Object instance, float delay)
        {
            // Wait for time
            yield return new WaitForSeconds(delay);

            // Call destroy
            despawn(instance);
        }

        private IEnumerator delayedDespawnAllRoutine(float delay)
        {
            // Wait for time
            yield return new WaitForSeconds(delay);

            // Call destroy all
            despawnAll();
        }

        /// <summary>
        /// Override the string value to return detaild state information about the pool.
        /// </summary>
        /// <returns>A string representation of the current pool state</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (isPrewarning == true)
            {
                builder.Append("Status: Prewarming ");
                builder.Append((pooled.Count / prewarmAmount) * 100);
                builder.Append("%");
            }
            else
            {
                builder.Append("Status: Active - Ready");
                builder.AppendLine();

                builder.Append("Pooled objects: ");
                builder.Append(pooled.Count);
                builder.AppendLine();

                builder.Append("Spawned objects: ");
                builder.Append(tracked.Count);
                builder.AppendLine();

                builder.Append("Pool usage: ");
                builder.Append(((float)pooled.Count / (float)maxAmount) * 100);
                builder.Append("%");
            }

            return builder.ToString();
        }
    }
}
