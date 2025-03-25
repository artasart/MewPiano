using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake3D : MonoBehaviour
{
    public static CameraShake3D Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<CameraShake3D>();
            return instance;
        }
    }
    private static CameraShake3D instance;

    CameraNoise cameraNoise;
    public CameraNoise.Properties noise;

	public void Shake()
    {
		if (cameraNoise == null)
		{
			cameraNoise = GetComponent<CameraNoise>();
			noise = new CameraNoise.Properties(90f, .05f, 10f, .5f, .625f, .415f, .35f);
		}

		cameraNoise.StartShake(noise);
	}

	public void Shake(CameraNoise.Properties noise = null)
	{
		cameraNoise.StartShake(noise);
	}
}