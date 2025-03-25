using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Make sure we can access the api
using UltimatePooling;

namespace UltimatePooling.Demo
{
    /// <summary>
    /// This example shows how to destory a pool at runtime. 
    /// Note that all pooled iters are destroyed but all items spawnd by this pool will remain in the scene.
    /// </summary>
    public class Ex5_DestroyPoolExample : MonoBehaviour
    {
        // Private
        private Stack<GameObject> spawned = new Stack<GameObject>();

        // Public
        /// <summary>
        /// The prefab we want to spawn - Assigned in the editor inspector.
        /// </summary>
        public GameObject prefab;

        // Methods
        private void Start()
        {
            // Create a new pool for the prefab object.
            // Note that if there is already a pool for this prefab then this method will simply returne a reference to that pool.
            PoolGroup pool = UltimatePool.Pools.createPool(prefab);

            // Initialize the prewarm values
            pool.prewarmPool = true;
            pool.prewarmAmount = 200;
            pool.prewarmPerFrame = 20;

            // Initialize the spawning values
            pool.maxAmount = 500;
            pool.parentInstances = true;
            pool.eventType = PoolEventType.InterfaceCallback;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.Label("Destroy Pool Example");

                if (GUILayout.Button("Spawn") == true)
                {
                    // This is the call that directly replaces 'Instantiate'
                    // Note that since we have already created a pool for this prefab, this method call will attempt to spawn from that pool.
                    // Note that we are using an overload that allows a position and rotation to be passed, just like 'Instantiate'
                    GameObject instance = UltimatePool.spawn(prefab, randomPosition(), randomRotation());

                    // Save the instance in a stack for matching 'despawn' calls
                    spawned.Push(instance);
                }

                if (GUILayout.Button("Despawn") == true)
                {
                    // Error checking - the user could click despawn when there are no objects in the scene.
                    if (spawned.Count > 0)
                    {
                        // Get the last spawned object
                        GameObject instance = spawned.Pop();

                        // This is the call that directly replaces 'Destroy'
                        UltimatePool.despawn(instance);
                    }
                }

                if (GUILayout.Button("Destroy Pool") == true)
                {
                    // Destroy the pool for our prefab
                    UltimatePool.Pools.destroyPool(UltimatePool.Pools.findPool(prefab));
                }
            }
            GUILayout.EndVertical();
        }

        private Vector3 randomPosition()
        {
            // Random value between -3 and 3
            float x = Random.Range(0, 6) - 3;

            // Random value between 0 and 3
            float z = Random.Range(0, 3);

            return new Vector3(x, 0, z);
        }

        private Quaternion randomRotation()
        {
            // Random angle
            float y = Random.Range(0, 360);

            return Quaternion.Euler(0, y, 0);
        }
    }
}
