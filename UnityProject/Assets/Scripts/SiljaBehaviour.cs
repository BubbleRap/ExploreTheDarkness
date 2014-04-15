using UnityEngine;
using System.Collections;

public class SiljaBehaviour : MonoBehaviour 
{
	public Health healthController;

	public void TakeALimb()
	{
		healthController.looseLife();
	}
}
