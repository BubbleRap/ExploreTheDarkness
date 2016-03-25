using UnityEngine;
using System.Collections;

public class ButtonPrompt : MonoBehaviour {

	public float distance = 0.5f;

	public Camera hudCamera;

	private Interactor interactor;
	private Transform connectedObject;
    private Renderer m_renderer;
    private TextMesh textMesh;

    void Awake()
    {
        textMesh = GetComponentInChildren<TextMesh>();
    }

	void Start()
	{
		hudCamera = Camera.main;
	}

	void OnEnable(){
        textMesh.color = new Color (1f, 1f, 1f, 0f);

		GameObject siljaGO = GameObject.FindGameObjectWithTag("Player");
		interactor = siljaGO.GetComponent<Interactor>();
	}

	public void SetText(string text){
        textMesh.text = text;
	}

	public void SetConnectedTransform(Transform t){
		connectedObject = t;
        m_renderer = connectedObject.GetComponent<Renderer>();
	}

	void Update () {

		float size = (hudCamera.gameObject.transform.position - transform.position).magnitude ;
		transform.localScale = new Vector3(size,size,size);

        textMesh.color = new Color (1f, 1f, 1f, Mathf.Lerp(textMesh.color.a, GetAlpha (),0.1f));

		transform.LookAt(hudCamera.gameObject.transform);
	}

	private float GetAlpha(){

        if (m_renderer != null && !m_renderer.isVisible)
			return 0f;
		else {
			Vector3 objToCamera = Camera.main.transform.position - connectedObject.position;

            LayerMask mask = 1 << LayerMask.NameToLayer("Default");

			if (Physics.Raycast( Camera.main.transform.position, objToCamera.normalized, objToCamera.magnitude, mask ))
				return 0f;

          

			//RaycastHit hit;

			//if(Physics.Raycast(Camera.main.transform.position,-objToCamera, out hit, 10F,LayerMask.NameToLayer("Trigger"))){
			//	if(hit.transform.gameObject != connectedObject.gameObject)
			//	{
			//		return 0f;
			//	}
			//}
		}

		/*
		Vector2 screenpos = Camera.main.WorldToScreenPoint (transform.position);
		screenpos.x /= Screen.width;
		screenpos.y /= Screen.height;

		screenpos.x = Mathf.Abs(0.5f - screenpos.x);
		screenpos.y = Mathf.Abs(0.5f - screenpos.y);
		float distFromCenter = Mathf.Max (screenpos.x, screenpos.y);
		distFromCenter *= 4;
		*/

		float distFromSilja = Vector3.Distance (connectedObject.position, interactor.transform.position);

		distFromSilja /= 4f;

		//float dist = Mathf.Max (distFromCenter, distFromSilja);
		float dist = distFromSilja;

		return 1f - (dist*dist);

	}
}
