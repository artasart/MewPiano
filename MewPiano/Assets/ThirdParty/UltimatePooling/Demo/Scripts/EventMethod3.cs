using UnityEngine;
using System.Collections;
using UltimatePooling;

/// <summary>
/// An example script that shows how to receive spawn and despawn events when the interface event type is used.
/// This method make use of the helper 'PoolBehaviour' script which implements the 'IPoolReceiver' interface.
/// </summary>
public class EventMethod3 : PoolBehaviour
{ 
    // Methods
    public override void OnSpawned(PoolGroup pool)
    {
        Debug.Log("[Behaviour Method]: Message from 'OnSpawned'");
    }

    public override void OnDespawned(PoolGroup pool)
    {
        Debug.Log("[Behaviour Method]: Message from 'OnDespawned'");
    }
}
