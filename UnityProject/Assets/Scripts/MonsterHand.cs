using UnityEngine;
using System.Collections;

public class MonsterHand : MonoBehaviour 
{
	private GameObject siljaGO = null;

	void Start()
	{
		siljaGO = GameObject.FindGameObjectWithTag("Player");
	}

	// called from monster hand animation

	public void StartAnimationEvent()
	{
		siljaGO.SendMessage("MonsterHandStartEvent", transform, SendMessageOptions.RequireReceiver);
	}

	public void HitAnimationEvent()
	{
		siljaGO.SendMessage("MonsterHandHitEvent", SendMessageOptions.RequireReceiver);
	}
}
