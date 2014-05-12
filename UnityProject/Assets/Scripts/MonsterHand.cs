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
	public void HitAnimationEvent()
	{
		siljaGO.SendMessage("MonsterHandHitEvent", SendMessageOptions.RequireReceiver);
	}
}
