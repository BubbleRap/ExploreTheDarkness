using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraFollow))]
public class CameraInput : MonoBehaviour 
{
//	[Range(0f, 180)]
//	public float verticalLimit = 45f;
//	[Range(0f, 180)]
//	private float horizontalLimit = 120f;

	[Range(0f, 10f)]
	public float verticalSensetivity = 1f;
	[Range(0f, 10f)]
	public float horizontalSensetivity = 1f;

	public bool iversedVertical = false; 

	private CameraFollow cameraFollow = null;

	void Awake()
	{
		cameraFollow = GetComponent<CameraFollow>();
	}

	void Update () 
	{
		Vector3 deltaMousePosition = new Vector3 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"), 0f);

		float vertSensetivity = iversedVertical == false ? verticalSensetivity : -verticalSensetivity;

		float pitchAngle = cameraFollow.pitch + deltaMousePosition.y * vertSensetivity ;
		pitchAngle = Mathf.Clamp (pitchAngle, 30f, 110f);

		cameraFollow.yaw = Mathf.Repeat(cameraFollow.yaw + deltaMousePosition.x * horizontalSensetivity, 359.999f);

		cameraFollow.pitch = pitchAngle;
	}
}
