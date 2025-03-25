using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UltimatePooling.Editor
{
    public static class CreateUtility
    {
        // Methods
        [MenuItem("GameObject/UltimatePooling/Create Pool", false, 11)]
        public static void createObjectPool()
        {
            // Create a parent object
            GameObject go = new GameObject("New Pool Group");

            // Add a generic pool group
            go.AddComponent<GenericPoolGroup>();

            // Select the new object
            Selection.activeGameObject = go;

            // Register for undo
            Undo.RegisterCreatedObjectUndo(go, "Create Pool");
        }

        [MenuItem("GameObject/UltimatePooling/Create Resources Pool", false, 11)]
        public static void createResourcesObjectPool()
        {
            // Create a parent object
            GameObject go = new GameObject("New Resources Pool Group");

            // Add a resources pool group
            go.AddComponent<ResourcesPoolGroup>();

            // Select the new object
            Selection.activeGameObject = go;

            // Register for undo
            Undo.RegisterCreatedObjectUndo(go, "Create Resources Pool");
        }
    }
}
