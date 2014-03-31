using UnityEngine;
using System.Collections;

public class RippingLegExample : MonoBehaviour 
{
	CameraFade fader = null;

	void Start()
	{
		fader = Camera.main.GetComponent<CameraFade>();
	}

	void Update()
	{
		if( fader.fadeIntensity > 0.3f )
			rigidbody.AddForce(fader.camera.transform.forward , ForceMode.Acceleration);
	}
}
