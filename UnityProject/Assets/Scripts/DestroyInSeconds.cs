using UnityEngine;
using System.Collections;

public class DestroyInSeconds : MonoBehaviour {

	public float destroyIn = 5f;

	void Awake()
	{
		Destroy(gameObject, destroyIn);
	}
}
