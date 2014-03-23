using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraFollow))]
public class CameraInput : MonoBehaviour 
{
	[Range(0f, 180)]
	public float verticalLimit = 45f;
	[Range(0f, 180)]
	public float horizontalLimit = 45f;

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

		float pitchAngle = cameraFollow.pitch + deltaMousePosition.y * vertSensetivity;
		pitchAngle = Mathf.Clamp (pitchAngle, horizontalLimit, horizontalLimit * 2f);

		cameraFollow.yaw = Mathf.Repeat(cameraFollow.yaw + deltaMousePosition.x * horizontalSensetivity, 359.9f);

		cameraFollow.pitch = pitchAngle;
	}
}
