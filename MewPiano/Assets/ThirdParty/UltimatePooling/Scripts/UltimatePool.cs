using UnityEngine;
using System.Collections.Generic;

namespace UltimatePooling
{
    /// <summary>
    /// The main class or interacting with the UltimatePooling API.
    /// All spawning and despawning methods are found in this class however you can use the individual spawn method on pools if required.
    /// </summary>
    public static class UltimatePool
    {
        // Private
        private static PoolManager poolManager = new PoolManager();

        // Public
        /// <summary>
        /// The amount level of logging that is allowed. Default is 'Message' - Full logging.
        /// </summary>
        public static LogLevel logLevel = LogLevel.Message;

        // Properties
        /// <summary>
        /// Access the pool manager which maintains all existing pools.
        /// Allows pools to be created and destroyed.
        /// </summary>
        public static PoolManager Pools
        {
            get { return poolManager; }
        }

        // Methods
        #region Spawn Overloads
        /// <summary>
        /// Spawn an instance of the specified prefab from the appropriate pool.
        /// If no pool exists, then one is automatically created for the prefab type.
        /// </summary>
        /// <param name="prefab">The prefab to spawn from</param>
        /// <returns>An instance of the prefab supplied</returns>
        public static GameObject spawn(GameObject prefab)
        {
            // Call through
            return spawn(prefab, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Spawn an instance of the specified component prefab from the appropriate pool.
        /// If no pool exists, then one is automatically created for the prefab type.
        /// </summary>
        /// <param name="prefab">The component prefab to spawn from</param>
        /// <returns>An instance of the prefab supplied</returns>
        public static Object spawn(Component prefab)
        {
            // Call through
            return spawn(prefab, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Spawn an instance of the specified component prefab from the appropriate pool.
        /// If no pool exists, then one is automatically created for the prefab type.
        /// </summary>
        /// <typeparam name="T">The type of object to return the instance as</typeparam>
        /// <param name="prefab">The component prefab to spawn from</param>
        /// <returns>An instance of the prefab supplied</returns>
        public static T spawn<T>(Component prefab) where T : Object
        {
            // Call through
            return spawn<T>(prefab, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Spawn an instance of a prefab with the specified name.
        /// The prefab name is the same name used when creating the pool, or if no name is used then the name of the prefab supplied is substituted.
        /// This method will only succeed if the pool has been created before hand.
        /// </summary>
        /// <param name="prefabName">The name of the prefab to spawn from</param>
        /// <returns>An instance of the prefab with the specified name</returns>
        public static GameObject spawn(string prefabName)
        {
            // Call through
            return spawn(prefabName, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Spawn an instance of the specified prefab from the appropriate pool.
        /// If no pool exists, then one is automatically created for the prefab type.
        /// </summary>
        /// <param name="prefab">The prefab to spawn from</param>
        /// <param name="position">The position to spawn the prefab at</param>
        /// <param name="rotation">The initial rotation to spawn the prefab with</param>
        /// <returns>An instance of the prefab supplied</returns>
        public static GameObject spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            // Validate the prefab
            PoolUtil.validatePrefab(prefab);

            // Find the pool for the prefab
            PoolGroup pool = poolManager.findPool(prefab);

            // Check for pool
            if (pool == null)
                pool = poolManager.createPool(prefab);

            // Spawn from the pool
            return pool.spawn(position, rotation) as GameObject;
        }

        /// <summary>
        /// Spawn an instance of the specified component prefab from the appropriate pool.
        /// If no pool exists, then one is automatically created for the component prefab type.
        /// </summary>
        /// <param name="prefab">The component prefab to spawn from</param>
        /// <param name="position">The position to spawn the prefab at</param>
        /// <param name="rotation">The initial rotation to spawn the prefab with</param>
        /// <returns>An instance of the component prefab supplied</returns>
        public static Object spawn(Component prefab, Vector3 position, Quaternion rotation)
        {
            // Validate the prefab
            PoolUtil.validatePrefab(prefab);

            // Find the pool group for the prefab
            PoolGroup pool = poolManager.findPool(prefab);

            // Check for pool
            if (pool == null)
                pool = poolManager.createPool(prefab);

            // Spawn from the pool
            return pool.spawn(position, rotation) as Component;
        }

        /// <summary>
        /// Spawn an instance of the specified component prefab from the appropriate pool.
        /// If no pools exists, then one is automatically created for the component prefab type.
        /// </summary>
        /// <typeparam name="T">The type of object to return the instance as</typeparam>
        /// <param name="prefab">The component prefab to spawn from</param>
        /// <param name="position">The position to spawn the prefab at</param>
        /// <param name="rotation">The initial rotation to spawn the prefab with</param>
        /// <returns>An instance of the component prefab supplied</returns>
        public static T spawn<T>(Component prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            // Validate the prefab
            PoolUtil.validatePrefab(prefab);

            // Find the pool group for the prefab
            PoolGroup pool = poolManager.findPool(prefab);

            // Check for pool
            if (pool == null)
                pool = poolManager.createPool(prefab);

            // Spawn from the pool
            return pool.spawn(position, rotation) as T;
        }

        /// <summary>
        /// Spawn an instance of a prefab with the specified name.
        /// This method will only succeed if the pool has been created before hand.
        /// </summary>
        /// <param name="prefabName">The name of the prefab to spawn from</param>
        /// <param name="position">The position to spawn the prefab at</param>
        /// <param name="identity">The initial rotation to spawn the prefab with</param>
        /// <returns>An instance of the prefab with the specified name</returns>
        public static GameObject spawn(string prefabName, Vector3 position, Quaternion identity)
        {
            // Find the pool group for the prefab
            PoolGroup pool = poolManager.findPool(prefabName);

            // Check for pool
            if (pool == null)
            {
                PoolUtil.logError(string.Format("Failed to spawn from pool '{0}' because it does not exist, Unable to create a suitable pool because the prefab type is unknown", prefabName));
                return null;
            }

            // Spawn from the pool
            return pool.spawn(position, identity) as GameObject;
        }
        #endregion

        #region Batch Spawn Overloads
        /// <summary>
        /// Spawn a number of instances of the specified prefab from the appropriate pool.
        /// If no pool exists, then one is automatically created for the prefab type.
        /// Batch spawning is quicker than multiple calls to <see cref="spawn(UnityEngine.GameObject)"/> because the pool is cached on the first spawn.
        /// </summary>
        /// <param name="prefab">The prefab to spawn from</param>
        /// <param name="amount">The amount of instances to create from this prefab</param>
        /// <returns>An enumeration of the spawned instances</returns>
        public static IEnumerable<GameObject> batchSpawn(GameObject prefab, int amount)
        {
            PoolUtil.validatePrefab(prefab);

            // Find the pool group for the prefab
            PoolGroup pool = poolManager.findPool(prefab);

            // Check for pool
            if (pool == null)
                pool = poolManager.createPool(prefab);

            // Spawn from pool
            for (int i = 0; i < amount; i++)
            {
                // Yield the spawned result
                yield return pool.spawn() as GameObject;
            }
        }

        /// <summary>
        /// Spawn a number of instances of the specified component prefab from the appropriate pool.
        /// If no pool exists, then one is automatically created for the prefab type.
        /// Batch spawning is quicker than multiple calls to <see cref="spawn(Component)"/> because the pool is cached on the first spawn.
        /// </summary>
        /// <param name="prefab">The component prefab to spawn from</param>
        /// <param name="amount">The amount of instances to create from this prefab</param>
        /// <returns>An enumeration of the spawned instances</returns>
        public static IEnumerable<Object> batchSpawn(Component prefab, int amount)
        {
            PoolUtil.validatePrefab(prefab);

            // Find the pool group for the prefab
            PoolGroup pool = poolManager.findPool(prefab);

            // Check for pool
            if (pool == null)
                pool = poolManager.createPool(prefab);

            // Spawn from pool
            for (int i = 0; i < amount; i++)
            {
                // Yield the spawned result
                yield return pool.spawn();
            }
        }

        /// <summary>
        /// Spawn a number of instances of the specified component prefab from the appropriate pool.
        /// If no pool exists, then one is automatically created for the prefab type.
        /// Batch spawning is quicker than multiple calls to <see cref="spawn{T}(UnityEngine.Component)"/> because the pool is cached on the first spawn.
        /// </summary>
        /// <typeparam name="T">The type of object to return the instances as</typeparam>
        /// <param name="prefab">The component prefab to spawn from</param>
        /// <param name="amount">The amount of instances to create from this prefab</param>
        /// <returns>An enumeration of the spawned instances</returns>
        public static IEnumerable<T> batchSpawn<T>(Component prefab, int amount) where T : Object
        {
            PoolUtil.validatePrefab(prefab);

            // Find the pool group for the prefab
            PoolGroup pool = poolManager.findPool(prefab);

            // Check for pool
            if (pool == null)
                pool = poolManager.createPool(prefab);

            // Spawn from pool
            for (int i = 0; i < amount; i++)
            {
                // Yield the spawned result
                yield return pool.spawn() as T;
            }
        }

        /// <summary>
        /// Spawn a number of instance of a prefab with the specified name from the appropriate pool.
        /// This method will only succeed if the pool has been created before hand.
        /// Batch spawning is quicker than multiple calls to <see cref="spawn(string)"/> because the pool is cached on the first spawn.
        /// </summary>
        /// <param name="prefabName">The name of the prefab to spawn from</param>
        /// <param name="amount">The amount of instances to create from this prefab</param>
        /// <returns>An enumeration of the spawned instances</returns>
        public static IEnumerable<Object> batchSpawn(string prefabName, int amount)
        {
            // Find the pool group for the prefab
            PoolGroup pool = poolManager.findPool(prefabName);

            // Check for pool
            if (pool == null)
            {
                PoolUtil.logError(string.Format("Failed to batch spawn from pool '{0}' because it does not exist, Unable to create a suitable pool because the prefab type is unknown", prefabName));
                yield break;
            }

            // Spawn from the pool
            for (int i = 0; i < amount; i++)
            {
                // Yield the spawned result
                yield return pool.spawn();
            }
        }

        /// <summary>
        /// Spawn a number of instances of the specified prefab and place the results in the specified array.
        /// This overload allows the user to manage the array where the objects will be spawned to avoid garbage generation.
        /// If no pool exists, then one is automatically created for the prefab type.
        /// Batch spawning is quicker than multiple calls to <see cref="spawn(UnityEngine.GameObject)"/> because the pool is cached on the first spawn.
        /// If amount is specified then the value should be less than or equal to the length of the array otherwise an out of bounds exception may occur.
        /// </summary>
        /// <param name="prefab">The prefab to spawn from</param>
        /// <param name="objects">The array to store the spawned objects in</param>
        /// <param name="amount">The amount of objects to spawn. If the value is set to -1 then the array is filled</param>
        public static void batchSpawn(GameObject prefab, GameObject[] objects, int amount = -1)
        {
            PoolUtil.validatePrefab(prefab);
            PoolUtil.validateReference(objects, "objects");

            // Find the pool group for the prefab
            PoolGroup pool = poolManager.findPool(prefab);

            // Check for pool
            if (pool == null)
                pool = poolManager.createPool(prefab);

            // Calcualte the amount to spawn 
            int size = (amount == -1) ? objects.Length : amount;

            // Spawn from pool
            for (int i = 0; i < size; i++)
            {
                // Add the spawned result to the array
                objects[i] = pool.spawn() as GameObject;
            }
        }

        /// <summary>
        /// Spawn a number of instances of the specified prefab and place the results in the specified array.
        /// This overload allows the user to manager the array where the objects will be spawned to avoid garbage generation.
        /// If no pool exists, then one is automatically created for the prefab type.
        /// Batch spawning is quicker than multiple calls to <see cref="spawn(Component)"/> because the pool is cached on the first spawn.
        /// If amount is specified then the value should be less than or equal to the length of the array otherwise an out of bounds exception may occur.
        /// </summary>
        /// <param name="prefab">The prefab to spawn from</param>
        /// <param name="objects">The array to store the spawned objects in</param>
        /// <param name="amount">The amount of objects to spawn. If the value is set to -1 then the array is filled</param>
        public static void batchSpawn(Component prefab, Object[] objects, int amount = -1)
        {
            PoolUtil.validatePrefab(prefab);
            PoolUtil.validateReference(objects, "objects");

            // Find the pool group for the prefab
            PoolGroup pool = poolManager.findPool(prefab);

            // Check for pool
            if (pool == null)
                pool = poolManager.createPool(prefab);

            // Calcualte the amount to spawn 
            int size = (amount == -1) ? objects.Length : amount;

            // Spawn from pool
            for (int i = 0; i < size; i++)
            {
                // Add the spawned result to the array
                objects[i] = pool.spawn();
            }
        }

        /// <summary>
        /// Spawn a number of instance of the specified prefab and place the results in the specified array.
        /// This overload allows the user to manage the array where the objects will be spawned to avoid garbage generation.
        /// If no pool exists, then one is automatically created for the prefab type.
        /// Batch spawning is quicker that multiple calls to <see cref="spawn{T}(UnityEngine.Component)"/> because the pool is cached on the first spawn.
        /// If amount is specified then the value should be less than or equal to the length of the array otherwise an out of bounds exception may occur.
        /// </summary>
        /// <typeparam name="T">The type of prefab that will be spawned</typeparam>
        /// <param name="prefab">The prefab to spawn from</param>
        /// <param name="objects">The array to store the spawned objects in</param>
        /// <param name="amount">The amount of objects to spawn. If the value is set to -1 then the array is filled</param>
        public static void batchSpawn<T>(Component prefab, T[] objects, int amount = -1) where T : Object
        {
            PoolUtil.validatePrefab(prefab);
            PoolUtil.validateReference(objects, "objects");

            // Find the pool group for the prefab
            PoolGroup pool = poolManager.findPool(prefab);

            // Check for pool
            if (pool == null)
                pool = poolManager.createPool(prefab);

            // Calcualte the amount to spawn 
            int size = (amount == -1) ? objects.Length : amount;

            // Spawn from pool
            for (int i = 0; i < size; i++)
            {
                // Add the spawned result to the array
                objects[i] = pool.spawn() as T;
            }
        }

        /// <summary>
        /// Spawn a number of instances of a prefab with the specified name from the appropriate pool.
        /// This overload allows the user to manage the array where the objects will be spawned to avoid garbage generation.
        /// This method will only succeed if the pool has been created before hand.
        /// Batch spawning is quicker than multiple calls to <see cref="spawn(string)"/> because the pool is cached on the first spawn.
        /// If amount is specified then the value should be less than or equal to the length of the array otherwise an out of bounds exception may occur.
        /// </summary>
        /// <param name="prefabName">The name of the prefab to spawn from</param>
        /// <param name="objects">The array to store the spawned objects in</param>
        /// <param name="amount">The amount of objects to spawn. If the value is set to -1 then the array is filled</param>
        public static void batchSpawn(string prefabName, Object[] objects, int amount = -1)
        {
            PoolUtil.validateReference(objects, "objects");

            // Find the pool group for the prefab
            PoolGroup pool = poolManager.findPool(prefabName);

            // Check for pool
            if (pool == null)
            {
                PoolUtil.logError(string.Format("Failed to batch spawn from pool '{0}' because it does not exist, Unable to create a suitable pool because the prefab type is unknown", prefabName));
                return;
            }

            // Calcualte the amount to spawn 
            int size = (amount == -1) ? objects.Length : amount;

            // Spawn from pool
            for (int i = 0; i < size; i++)
            {
                // Add the spawned result to the array
                objects[i] = pool.spawn();
            }
        }
        #endregion

        /// <summary>
        /// Attempts to Despawn all objects and return them to their pool group.
        /// Important: All objects in the enumerable collection must have been spawned from the same pool group.
        /// If you attempt to return instances from multiple pools using this method then you will invalidate all associated pool groups.
        /// </summary>
        /// <param name="objects">An enumerable collection of objects that should be despawned</param>
        public static void batchDespawn(IEnumerable<GameObject> objects)
        {
            PoolUtil.validateReference(objects, "objects");

            // Pool group
            PoolGroup pool = null;

            // Process each object
            foreach (GameObject go in objects)
            {
                // Check for null reference
                if(go == null)
                    continue;

                // Make sure we have found the pool - Only check once since we assume that all objects were created from the same pool.
                if (pool == null)
                    pool = poolManager.findPoolWithInstance(go);

                // Check if the pool has still not been found
                if (pool == null)
                {
                    PoolUtil.logWarning("Failed to despawn pooled instance because the managing pool could not be found. Has the pool been destroyed leaving references of spawned objects? The object will be permanantly destroyed");

                    // Destroy the instance
                    Object.Destroy(go);
                    continue;
                }

                // Despawn the instance
                pool.despawn(go);
            }
        }

        /// <summary>
        /// Attempts to Despawn all objects and return them to their pool group.
        /// Important: All objects in the enumerable collection must have been spawned from the same pool group.
        /// If you attempt to return instances from multiple pools using this method then you will invalidate all associated pool groups.
        /// </summary>
        /// <param name="objects">An enumerable collection of components that should be despawned</param>
        public static void batchDespawn(IEnumerable<Component> objects)
        {
            PoolUtil.validateReference(objects, "objects");

            // Pool group
            PoolGroup pool = null;

            // Precoess each object
            foreach (Component component in objects)
            {
                // Check for null reference
                if(component == null)
                    continue;
                
                // Make sure we have found the pool - Only checn once since we assume that all objects were created from the same pool.
                if (pool == null)
                    pool = poolManager.findPoolWithInstance(component);

                // Check if the pool has still not been found
                // Check for pool
                if (pool == null)
                {
                    PoolUtil.logWarning("Failed to despawn pooled instance because the managing pool could not be found. Has the pool been destroyed leaving references of spawned objects? The object will be permanantly destroyed");

                    // Destroy the instance
                    Object.Destroy(component);
                    continue;
                }

                // Despawn the instance
                pool.despawn(component);
            }
        }

        /// <summary>
        /// Direct replacement for 'Object.Destroy' for pooling.
        /// Allows the specified instance to be returned to the pool and re-used at a later time.
        /// </summary>
        /// <param name="instance">A reference to a spawned instance</param>
        public static void despawn(Object instance)
        {
            // Make sure the argument is valid
            PoolUtil.validateReference(instance, "instance");

            // Find the pool that spawned the instance
            PoolGroup pool = poolManager.findPoolWithInstance(instance);

            // Check for pool
            if (pool == null)
            {
                PoolUtil.logWarning("Failed to despawn pooled instance because the managing pool could not be found. Has the pool been destroyed leaving references of spawned objects? The object will be permanantly destroyed");

                // Destroy the instance
                Object.Destroy(instance);
                return;
            }

            // Despawn from pool
            pool.despawn(instance);
        }

        /// <summary>
        /// Direct overload replacement for 'Object.Destroy' for pooling.
        /// Allows the specified instance to be returned to the pool and re-used at a later time.
        /// </summary>
        /// <param name="instance">A reference to a spawned instance</param>
        /// <param name="time">The amount of time to wait before despawning the instance</param>
        public static void despawn(Object instance, float time)
        {
            // Make sure the argument is valid
            PoolUtil.validateReference(instance, "instance");

            // Find the pool that spawned the instance
            PoolGroup pool = poolManager.findPoolWithInstance(instance);

            // Check for pool
            if (pool == null)
            {
                PoolUtil.logWarning("Failed to despawn pooled instance because the managing pool could not be found. Has the pool been destroyed leaving references of spawned objects? The object will be permanantly destroyed");

                // Destroy the instance
                Object.Destroy(instance, time);
                return;
            }

            // Despawn from pool
            pool.despawn(instance, time);
        }

        /// <summary>
        /// Calls all spawned instances of the specified prefab back to their pool.
        /// This method allows you to pass a prefab such as a 'Bullet' which will subsequently cause all 'Bullet' instances to be despawned.
        /// </summary>
        /// <param name="prefab">The prefab to despawn all instances of</param>
        public static void despawnAll(GameObject prefab)
        {
            // Make sure the prefab is valid
            PoolUtil.validatePrefab(prefab);

            // Find the pool that manages this prefab
            PoolGroup pool = poolManager.findPool(prefab);

            // Check for pool - There is no pool for this prefab type so fail silently
            if (pool == null)
                return;

            // Despawn all instances
            pool.despawnAll();
        }

        /// <summary>
        /// Calls all spawned instances of the specified prefab back to their pool.
        /// This method allows you to pass a prefab such as a 'Bullet' which will subsequently cause all 'Bullet' instances to be despawned. 
        /// </summary>
        /// <param name="prefab">The prefab to despawn all instances of</param>
        /// <param name="time">The amount of time to wait before despawning</param>
        public static void despawnAll(GameObject prefab, float time)
        {
            // Make sure the prafab is valid
            PoolUtil.validatePrefab(prefab);

            // Find the pool that manages this prefab
            PoolGroup pool = poolManager.findPool(prefab);

            // Check for pool - These is no pool for this prefab type so fail silently
            if (pool == null)
                return;

            // Despawn all instances after a delay
            pool.despawnAll(time);
        }
    }
}
