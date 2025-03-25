using UnityEngine;

namespace UltimatePooling
{
    /// <summary>
    /// Represents a pool groupd that can accept either a game object or component prefab as its root prefab.
    /// </summary>
    public class GenericPoolGroup : PoolGroup
    {
        // Private
        [SerializeField]
        private Object prefab = null;

        // Properties
        /// <summary>
        /// Access the component or game object prefab.
        /// </summary>
        public override Object Prefab
        {
            get { return prefab; }
            set
            {
                // Set the value
                if (value is Component || value is GameObject)
                {
                    // Save the reference
                    prefab = value;
                }
                else
                {
                    // Default to null
                    prefab = null;
                }
            }
        }

        // Methods
        /// <summary>
        /// Handle the spawning of a pooled object. 
        /// By default, this method enabled the game object.
        /// </summary>
        /// <param name="instance">The newly spawned instance to handle</param>
        /// <param name="type">The type of event used to inform the object of its spawn status</param>
        /// <param name="position">The position to spawn the object at</param>
        /// <param name="rotation">The rotation to spawn the object with</param>
        protected override void onInstanceSpawned(Object instance, PoolEventType type, Vector3 position, Quaternion rotation)
        {
            // Get the game object for the instance
            GameObject go = getInstanceObject(instance);

            // Check for unsupported type - something has gone wrong
            if (go == null)
                return;

            // Modify the transform
            go.transform.position = position;
            go.transform.rotation = rotation;

            // Enable the object
            go.SetActive(true);

            // Check the event type
            if (type == PoolEventType.BroadcastMessage)
            {
                // Trigger the event
                go.BroadcastMessage(PoolBehaviour.monoSpawnedEvent, this, SendMessageOptions.DontRequireReceiver);
            }
            else if (type == PoolEventType.InterfaceCallback)
            {
                // Find all receivers and trigger the spawned event
                foreach(IPoolReceiver receiver in go.GetComponentsInChildren<IPoolReceiver>())
                    receiver.OnSpawned(this);
            }
        }

        /// <summary>
        /// Handle despawning of a pooled object.
        /// By default, this method disables the game object.
        /// </summary>
        /// <param name="instance">The instance to handle the despawning of</param>
        /// <param name="type">The type of event used to inform the object of its spawned status</param>
        protected override void onInstanceDespawned(Object instance, PoolEventType type)
        {
            // Get the game object for the instance
            GameObject go = getInstanceObject(instance);

            // Check for unsupported type - something has gone wrong
            if (go == null)
                return;

            // Check the event type
            if (type == PoolEventType.BroadcastMessage)
            {
                // Trigger the event
                go.BroadcastMessage(PoolBehaviour.monoDespawnedEvent, this, SendMessageOptions.DontRequireReceiver);
            }
            else if (type == PoolEventType.InterfaceCallback)
            {
                // Find all receivers and trigger the spawned event
                foreach (IPoolReceiver receiver in go.GetComponentsInChildren<IPoolReceiver>())
                    receiver.OnDespawned(this);
            }

            // Disable the object
            go.SetActive(false);
        }

        private GameObject getInstanceObject(Object instance)
        {
            // Get the game object
            GameObject go = null;

            // Check for a component
            if (instance is Component)
                go = (instance as Component).gameObject;

            //  Check for a game object
            if (instance is GameObject)
                go = instance as GameObject;

            return go;
        }
    }
}
