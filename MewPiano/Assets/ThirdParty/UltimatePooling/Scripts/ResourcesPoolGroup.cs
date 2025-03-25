using UnityEngine;
using System.Collections;
using UltimatePooling;

namespace UltimatePooling
{
    /// <summary>
    /// Represents a pool group that manages a prefab object located within the resources folder.
    /// </summary>
    public class ResourcesPoolGroup : GenericPoolGroup
    {
        // Public
        /// <summary>
        /// The name of the prefab to load from the resources folder.
        /// </summary>
        public string prefabName = "";

        // Properties
        /// <summary>
        /// We need to modify the way that the prefab is retrieved.
        /// </summary>
        public override Object Prefab
        {
            get
            {
                // Only load from resources when in game
                if (Application.isPlaying == true)
                {
                    // Check if the prefab is loaded
                    if (base.Prefab == null)
                    {
                        // Try to load the prefab
                        base.Prefab = Resources.Load(prefabName);
                    }
                }

                // Get the base prefab
                return base.Prefab;
            }
            set { base.Prefab = value; }
        }

        // Methods
        /// <summary>
        /// Called by Unity when the pool is created.
        /// </summary>
        protected override void Start()
        {
            if (Prefab == null)
            {
                // There is a problem with the pool
                PoolUtil.logWarning(string.Format("Failed to load resource prefab '{0}', Are you sure it exists and is located within a valid 'Resources' folder?", prefabName));
            }

            // Remember to call the base method
            base.Start();
        }
    }
}
