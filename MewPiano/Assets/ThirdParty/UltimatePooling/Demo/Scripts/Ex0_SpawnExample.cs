using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Make sure we can access the api.
using UltimatePooling;

namespace UltimatePooling.Demo
{
    /// <summary>
    /// This class shows how to spawn and despawn prefab objects.
    /// </summary>
    public class Ex0_SpawnExample : MonoBehaviour
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
                GUILayout.Label("Spawn Example");

                if (GUILayout.Button("Spawn") == true)
                {
                    // This is the call that directly replaces 'Instantiate'
                    GameObject instance = UltimatePool.spawn(prefab);

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
    }
}
