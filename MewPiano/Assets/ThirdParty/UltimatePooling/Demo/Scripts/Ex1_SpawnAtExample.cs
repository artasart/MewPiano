using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Make sure we can access the api.
using UltimatePooling;

namespace UltimatePooling.Demo
{
    /// <summary>
    /// This class shows hot to spawn prefab objects using the overloaded methods.
    /// </summary>
    public class Ex1_SpawnAtExample : MonoBehaviour
    {
        // Private
        private Stack<GameObject> spawned = new Stack<GameObject>();

        // Public
        /// <summary>
        /// The prefab we want to spawn - Assigned in the editor inspector.
        /// </summary>
        public GameObject prefab;

        // Methods
        private void OnGUI()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.Label("Spawn at Example");

                if (GUILayout.Button("Spawn") == true)
                {
                    // This is the call that directly replaces 'Instantiate'
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
