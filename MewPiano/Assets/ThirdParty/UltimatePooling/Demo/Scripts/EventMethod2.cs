using UnityEngine;
using System.Collections;
using UltimatePooling;

/// <summary>
/// An example script that shows how to receive spawn and despawn events when the interface event type is used.
/// </summary>
public class EventMethod2 : MonoBehaviour, IPoolReceiver
{
    // Methods  
    public void OnSpawned(PoolGroup pool)
    {
        Debug.Log("[Interface Method]: Message for 'OnSpawned'");
    }

    public void OnDespawned(PoolGroup pool)
    {
        Debug.Log("[Interface Method]: Message for 'OnDespawned'");
    }
}
