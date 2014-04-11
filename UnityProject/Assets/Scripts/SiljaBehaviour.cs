using UnityEngine;
using System.Collections;

public class SiljaBehaviour : MonoBehaviour 
{
	private int lives = 5;

	public void TakeALimb()
	{
		lives--;
		print ("Took a limb. " + lives + " limbs left");
	}
}
