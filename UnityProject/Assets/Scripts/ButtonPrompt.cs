using UnityEngine;
using System.Collections;

public class ButtonPrompt : MonoBehaviour {

	public float distance = 0.5f;

	public Camera hudCamera;

	private Interactor interactor;
	private Transform connectedObject;
    private Renderer m_renderer;
    private TextMesh textMesh;

    private Renderer cachedRenderer;

    void Awake()
    {
        textMesh = GetComponentInChildren<TextMesh>();
        cachedRenderer = textMesh.GetComponent<Renderer>();
    }

	void Start()
	{
		hudCamera = Camera.main;
	}

	void OnEnable(){
        textMesh.color = Color.white;
        cachedRenderer.material.color = new Color (1f, 1f, 1f, 0f);

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

        cachedRenderer.material.color = new Color (1f, 1f, 1f, Mathf.Lerp(cachedRenderer.material.color.a, GetAlpha (),0.1f));

		transform.LookAt(hudCamera.gameObject.transform);
	}

	private float GetAlpha(){

        if (m_renderer != null && !m_renderer.isVisible)
			return 0f;
		else {
			Vector3 objToCamera = Camera.main.transform.position - connectedObject.position;

            //10 is far, right?
            if (objToCamera.sqrMagnitude > 100)
                return 0f;

            LayerMask mask = (1 << (LayerMask.NameToLayer("Default"))); //~(1 << (LayerMask.NameToLayer("Trigger") | LayerMask.NameToLayer("Character")));
            RaycastHit hit;

            if (Camera.main != null && 
                Physics.Raycast(Camera.main.transform.position, -objToCamera.normalized, out hit, objToCamera.magnitude, mask) &&
                hit.transform != null &&
                connectedObject != null)
            {
                if (hit.transform.gameObject != connectedObject.gameObject &&
                    (connectedObject.transform.parent == null ||
                    hit.transform.gameObject != connectedObject.transform.parent.gameObject)
                    )
                {
                    return 0f;
                }
            }
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

    //void OnDrawGizmos()
    //{
    //    if (textMesh.color.a > 0)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(Camera.main.transform.position, connectedObject.position);
    //    }
    //}
}
