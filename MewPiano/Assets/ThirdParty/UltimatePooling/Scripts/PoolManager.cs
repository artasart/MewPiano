using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace UltimatePooling
{
    /// <summary>
    /// The manager that is responsible for all pool groups and handles the creation and destruction of pools at runtime.
    /// </summary>
    public sealed class PoolManager
    {
        // Private
        private static string poolPrefix = "PoolGroup_";
        private Dictionary<Object, PoolGroup> pools = new Dictionary<Object, PoolGroup>();

        // Constructor
        internal PoolManager() { }

        // Methods
        /// <summary>
        /// Attempt to create a new obejct pool for prefab type.
        /// If a pool already exists for the specified prefab then this method will simply return the exising pool.
        /// </summary>
        /// <param name="prefab">The prefab to create the pool for</param>
        /// <param name="name">The name of the pool</param>
        /// <returns>An instance of a pool group</returns>
        public PoolGroup createPool(GameObject prefab, string name = "")
        {
            PoolUtil.validateReference(prefab, "prefab");

            // Check for existing pools
            if (hasPool(prefab) == true)
                return findPool(prefab);

            // Call the common method
            return createPoolCommon(prefab, name);
        }

        /// <summary>
        /// Attempts to create a new object pool for the component prefab type.
        /// If a pool already exists for the specified prefab then this method will simply return the existing pool.
        /// </summary>
        /// <param name="prefab">The prefab to create the pool for</param>
        /// <param name="name">The name of the pool</param>
        /// <returns>An instance of a pool group</returns>
        public PoolGroup createPool(Component prefab, string name = "")
        {
            PoolUtil.validateReference(prefab, "prefab");

            // Check for existing pools
            if(hasPool(prefab) == true)
                return findPool(prefab);

            // Call the common method
            return createPoolCommon(prefab, name);
        }

        /// <summary>
        /// Attempts to create a new object pool for a prefab located in the resources folder.
        /// If a pool already exists for the specified prefab name then this method will simply return the existing pool.
        /// </summary>
        /// <param name="prefabName">The name of the prefab in the resources folder</param>
        /// <returns>An instance of a pool group</returns>
        public PoolGroup createPool(string prefabName)
        {
            // Create the pool object
            GameObject go = new GameObject(poolPrefix + prefabName);

            // Add the component
            ResourcesPoolGroup pool = go.AddComponent<ResourcesPoolGroup>();
            pool.prefabName = prefabName;

            return pool;
        }
        
        // This method cant be public because it would allow invalid pools to be created if the user passed objects that are not game objects or components.
        private PoolGroup createPoolCommon(Object prefab, string name)
        {
            // Generate the pool name
            string poolName = (string.IsNullOrEmpty(name) == true) ? prefab.name : name;

            // Create the pool object
            GameObject go = new GameObject(poolPrefix + poolName);

            // Add the component
            PoolGroup pool = go.AddComponent<GenericPoolGroup>();
            pool.Prefab = prefab;

            return pool;
        }

        /// <summary>
        /// Destroys a pool group and call of its pooled instances.
        /// </summary>
        /// <param name="pool">The pool to destroy</param>
        /// <param name="keepSpawnedInstances">When true, all spawned instances will be kept alive</param>
        public void destroyPool(PoolGroup pool, bool keepSpawnedInstances = true)
        {
            PoolUtil.validateReference(pool, "pool");

            // Destroy the pooled spawned instances
            pool.destroySelf(keepSpawnedInstances);
        }

        /// <summary>
        /// Find the pool for the prefab with the specified name.
        /// </summary>
        /// <param name="name">The name of the prefab to find the pool for</param>
        /// <returns>An instance of the pool group responsible for the prefab with the specified name</returns>
        public PoolGroup findPool(string name)
        {
            // Try to find by name
            foreach(PoolGroup pool in pools.Values)
                if (pool.name == name || pool.name == poolPrefix + name)
                    return pool;

            // No pool found
            return null;
        }

        /// <summary>
        /// Find the pool for the specified prefab.
        /// </summary>
        /// <param name="prefab">The prefab to find the pool for</param>
        /// <returns>An insatnce of the pool group responsible for the specified prefab</returns>
        public PoolGroup findPool(Object prefab)
        {
            // Check if we have a pool for this prefab
            if (hasPool(prefab) == true)
                return pools[prefab];

            // No pool found
            return null;
        }

        /// <summary>
        /// Returns true if there is an existing pool for the specified prefab type.
        /// </summary>
        /// <param name="prefab">The prefab to check for</param>
        /// <returns>True if the specified prefab is already associated with a pool group</returns>
        public bool hasPool(Object prefab)
        {
            // Check for prefab
            return pools.ContainsKey(prefab);
        }

        /// <summary>
        /// Find the pool that initially spawned the specified instance.
        /// This method will fail if the pool that spawned this instance has been destroyed. In this case it will be up to the user to destroy the object manually, or call <see cref="UltimatePool.despawn(Object)"/> which will result in the same thing.
        /// </summary>
        /// <param name="instance">The instance to find the managing pool for</param>
        /// <returns>The managin pool group if found</returns>
        public PoolGroup findPoolWithInstance(Object instance)
        {
            foreach(PoolGroup pool in pools.Values)
                if(pool.didSpawn(instance) == true)
                    return pool;
            
            // No pool found
            return null;
        }

        internal void addPool(PoolGroup pool)
        {
            // Make sure the pool is setup correctly
            if (pool.Prefab == null)
            {
                PoolUtil.logWarning("Failed to register pool group because it is missing its associated prefab");
                return;
            }

            // Add the pool to the manager
            pools.Add(pool.Prefab, pool);
        }

        internal void removePool(PoolGroup pool)
        {
            // Check for pool
            if (pools.ContainsKey(pool.Prefab) == true)
            {
                pools.Remove(pool.Prefab);
            }
        }
    }
}