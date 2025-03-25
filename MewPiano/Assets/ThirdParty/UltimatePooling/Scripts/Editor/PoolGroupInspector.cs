using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using Object = UnityEngine.Object;

namespace UltimatePooling.Editor
{
    [CustomEditor(typeof(ResourcesPoolGroup))]
    public class ResourcesPoolGroupInspector : PoolGroupInspector
    {
        protected override bool displayPrefabField()
        {
            // Get the target
            ResourcesPoolGroup pool = target as ResourcesPoolGroup;

            // Display prefab name field
            EditorGUILayout.PropertyField(serializedObject.FindProperty("prefabName"));
            
            // Check for a prefab name
            if (string.IsNullOrEmpty(pool.prefabName) == true)
            {
                // Display the error box
                EditorGUILayout.HelpBox("This pool group does not have a prefab name assigned and will be ignored at runtime. Assign the name of a resources prefab to get started!", MessageType.Error);
                return false;
            }
            else
            {
                string prefabName = Path.GetFileName(pool.prefabName);

                // Search the database
                string[] assets = AssetDatabase.FindAssets(string.Format("{0} t:gameObject t:component", prefabName));
                
                // Check for matches
                bool matches = false;

                foreach (string guid in assets)
                {
                    // Get the path from id
                    string path = AssetDatabase.GUIDToAssetPath(guid);

                    // Check for matching names
                    if (string.Equals(Path.GetFileNameWithoutExtension(path), prefabName, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        matches = true;
                        break;
                    }
                }

                if (matches == false)
                {
                    // Display a warning box
                    EditorGUILayout.HelpBox(
                        "This pool group points to a prefab that could not be found. Are you sure it exists?",
                        MessageType.Warning);
                    return false;
                }
            }

            // Success
            return true;
        }
    }

    [CustomEditor(typeof(PoolGroup), true)]
    public class PoolGroupInspector : UnityEditor.Editor
    {
        // Private
        private bool expandPrewarm = false;
        private bool expandSpawning = false;
        private PoolGroup pool = null;

        // Methods
        private void OnEnable()
        {
            if (EditorPrefs.HasKey("UltimatePooling") == true)
            {
                expandPrewarm = EditorPrefs.GetBool("UltimatePooling.ExpandPrewarm", false);
                expandSpawning = EditorPrefs.GetBool("UltimatePooling.ExpandSpawning", false);
            }

            // Add event listener
            EditorApplication.update += onUpdate;
        }

        private void OnDisable()
        {
            EditorPrefs.SetString("UltimatePooling", "");
            EditorPrefs.SetBool("UltimatePooling.ExpandPrewarm", expandPrewarm);
            EditorPrefs.SetBool("UltimatePooling.ExpandSpawning", expandSpawning);

            // Remove listener
            EditorApplication.update += onUpdate;
        }

        private void onUpdate()
        {
            // Check for play mode
            if(Application.isPlaying == true)
                Repaint();
        }

        public override void OnInspectorGUI()
        {
            // Get the pool
            pool = target as PoolGroup;
            
            // Leave a space
            EditorGUILayout.Space();

            // Display the prefab field
            if(displayPrefabField())
            {
                // Prewarm section
                GUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUI.indentLevel++;

                    if ((expandPrewarm = EditorGUILayout.Foldout(expandPrewarm, "Prewarm")) == true)
                    {
                        // Render the prewarm section
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("prewarmPool"));

                        if (pool.prewarmPool == true)
                        {
                            pool.prewarmAmount = EditorGUILayout.IntSlider("Prewarm Amount", pool.prewarmAmount, 0, pool.maxAmount);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("prewarmPerFrame"));
                        }
                    }

                    EditorGUI.indentLevel--;
                }
                GUILayout.EndVertical();

                // Spawning section
                GUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUI.indentLevel++;

                    if ((expandSpawning = EditorGUILayout.Foldout(expandSpawning, "Spawning")) == true)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAmount"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("parentInstances"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("eventType"));
                    }

                    EditorGUI.indentLevel--;
                }
                GUILayout.EndVertical();

                // Display pool values
                displayRuntimeInfo();
            }

            // Apply changes
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual bool displayPrefabField()
        {
            // Display the prefab field
            pool.Prefab = EditorGUILayout.ObjectField("Prefab", pool.Prefab, typeof(Object), true);

            // Display remaining fields
            if (pool.Prefab == null)
            {
                // Display an error about the prefab
                EditorGUILayout.HelpBox("This pool group has no prefab assigned and will be ignored at runtime. Assign a valid Component or GameObject prefab to get started!", MessageType.Error);
            }

            // Check for a valid prefab
            return pool.Prefab != null;
        }

        private void displayRuntimeInfo()
        {
            if (EditorApplication.isPlaying == false)
            {
                // Helpful message
                EditorGUILayout.HelpBox("Pooling status will be shown here when the game is running", MessageType.Info);
            }
            else
            {
                // Helpful status
                EditorGUILayout.HelpBox(pool.ToString(), MessageType.Info);
            }
        }
    }
}
