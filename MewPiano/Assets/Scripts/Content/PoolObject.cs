using UltimatePooling;
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolReceiver
{
	public void OnDespawned(PoolGroup pool)
	{

	}

	public void OnSpawned(PoolGroup pool)
	{
		Debug.Log("Spawned");

		Invoke("Repool", 3f);
	}

	private void Repool()
	{
		UltimatePool.despawn(this.gameObject);
	}
}
