using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraFollow))]
public class CameraInput : MonoBehaviour 
{
	// make a "Min-Max" property drawer for the first 
	// time in your life for gods sake already!

//	[MinMax(0f, 360f)]
//	public float verticalLimit = 0f;
//	[MinMax(0f, 360f)]
//	public float horizontalLimit = 0f;
	[Range(0f, 1f)]
	public float verticalSensetivity = 1f;
	[Range(0f, 1f)]
	public float horizontalSensetivity = 1f;

	public bool iversedVertical = false; 

	private CameraFollow cameraFollow = null;
	private Vector3 cachedMousePosition = Vector3.zero;

	void Awake()
	{
		cameraFollow = GetComponent<CameraFollow>();
	}

	void Start()
	{
		cachedMousePosition = Input.mousePosition;
	}

	void Update () 
	{
		Vector3 deltaMousePosition = Input.mousePosition - cachedMousePosition;
		cachedMousePosition = Input.mousePosition;

		float vertSensetivity = iversedVertical == false ? verticalSensetivity : -verticalSensetivity;


		cameraFollow.yaw = Mathf.Repeat(cameraFollow.yaw + deltaMousePosition.x * horizontalSensetivity, 359.9f);
		cameraFollow.pitch = Mathf.Repeat(cameraFollow.pitch + deltaMousePosition.y * vertSensetivity, 359.9f);
	}
}
