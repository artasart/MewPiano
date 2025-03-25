using UnityEngine;
using System.Collections;

using UltimatePooling;


public class Weapon : MonoBehaviour
{
    // Bullet prefab
    public GameObject bullet;

    // How long the bullet lasts
    public float bulletLifetime = 3;

    void Start()
    {
        // Make sure our prefab is valid
        if (bullet == null)
            enabled = false;
    }

    void Update()
    {
        // Check for shoot
        if (Input.GetButtonDown("Fire 1") == true)
        {
            // Spawn a bullet at using the weapon transform for position
            GameObject bulletInstance = UltimatePool.spawn(bullet, transform.position, Quaternion.identity);

            // Apply force and direction to the bullet
            applyBulletForce(bulletInstance);

            // Despawn the bullet after a certain amount of time
            UltimatePool.despawn(bulletInstance, bulletLifetime);
        }
    }

    void applyBulletForce(GameObject g)
    {
    }
}


/// <summary>
/// An example script that shows how to receive spawn and despawn events when the Broadcast event type is used.
/// Note that the PoolGroup argument is optional and can be left out if it is not required.
/// </summary>
public class EventMethod1 : MonoBehaviour
{
    // Methods
    public void OnSpawned(/*PoolGroup pool*/)
    {
        Debug.Log("[Broadcast Method]: Message from 'OnSpawned'");   
    }

    public void OnDespawned(/*PoolGroup pool*/)
    {
        Debug.Log("[Broadcast Method]: Message from 'OnDespawned'");
    }
}
