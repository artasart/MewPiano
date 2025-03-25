using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UltimatePooling;

namespace UltimatePooling.Demo
{
    public class Benchmark : MonoBehaviour
    {
        // Private
        private Stack<GameObject> spawned = new Stack<GameObject>();
        private Stopwatch timer = new Stopwatch();
        private string timingMessage = "Time: No test run";

        // Public
        public GameObject prefab;

        // Methods
        private void OnGUI()
        {
            UnityEngine.Profiling.Profiler.BeginSample("Ultimate Pooling Benchmark");
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.Label("Benchmark");

                if (GUILayout.Button("Spawn 1000") == true)
                {
                    startTiming();

                    for (int i = 0; i < 1000; i++)
                    {
                        GameObject instance = UltimatePool.spawn(prefab, randomPosition(), randomRotation());
                        spawned.Push(instance);
                    }

                    stopTiming("Spawned 1000");
                }

                if (GUILayout.Button("Despawn 1000") == true)
                {
                    startTiming();

                    // Error checking - the user could click despawn when there are no objects in the scene.
                    if (spawned.Count > 0)
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            GameObject instance = spawned.Pop();
                            UltimatePool.despawn(instance);
                        }
                    }

                    stopTiming("Despawned 1000");
                }

                if (GUILayout.Button("Batch Spawn 1000") == true)
                {
                    startTiming();

                    foreach (GameObject result in UltimatePool.batchSpawn(prefab, 1000))
                    {
                        spawned.Push(result);
                    }

                    stopTiming("Batch Spawned 1000");
                }

                if (GUILayout.Button("Batch Despawn All") == true)
                {
                    startTiming();

                    UltimatePool.batchDespawn(spawned);
                    spawned.Clear();

                    stopTiming("Batch Despawned All");
                }

                if (GUILayout.Button("Destroy Pool") == true)
                {
                    // Destroy the pool for our prefab
                    UltimatePool.Pools.destroyPool(UltimatePool.Pools.findPool(prefab));
                }

                GUILayout.Label(timingMessage);
            }
            GUILayout.EndVertical();
            UnityEngine.Profiling.Profiler.EndSample();
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

        private void startTiming()
        {
            timer.Reset();
            timer.Start();
        }

        private void stopTiming(string msg)
        {
            timer.Stop();
            timingMessage = string.Format("{0} in {1} Milliseconds", msg, timer.ElapsedMilliseconds);
        }
    }
}
