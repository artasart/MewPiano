using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Make sure we can access the api
using UltimatePooling;

/// <summary>
/// This example shows how to destroy a spawned object after a specified number of seconds.
/// This provides similar to the overloaded 'Destroy' that Unity provides.
/// </summary>
public class Ex2_DelayedDespawnExample : MonoBehaviour
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
            GUILayout.Label("Delayed Despawn Example");

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
                    // Note that we are using the overloaded despawn method to despawn the object after 2 seconds.
                    UltimatePool.despawn(instance, 2);
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
