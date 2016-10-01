using UnityEngine;
using System.Collections;

public class ButtonPrompt : MonoBehaviour {

	public float distance = 0.5f;

	public Camera hudCamera;

	private Interactor interactor;
	private Transform connectedObject;
    private Renderer m_renderer;
    private TextMesh textMesh;
	private SpriteRenderer spriteRenderer;

    private Renderer cachedRenderer;

    public bool isVisible { get; private set; }

    void Awake()
    {
        textMesh = GetComponentInChildren<TextMesh>();
        cachedRenderer = textMesh.GetComponent<Renderer>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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

	public void setInteractableUI(bool boolean){
		spriteRenderer.enabled = boolean;

		Vector3 basePosition = textMesh.gameObject.transform.localPosition;
		textMesh.gameObject.transform.localPosition = new Vector3(basePosition.x,boolean ? 0.021f : 0,basePosition.y);
		spriteRenderer.gameObject.transform.localPosition = new Vector3(basePosition.x,boolean ? -0.021f : 0,basePosition.y);
	}

	public void SetConnectedTransform(Transform t){
		connectedObject = t;
        m_renderer = connectedObject.GetComponent<Renderer>();
	}

	void Update () {

		float size = (hudCamera.gameObject.transform.position - transform.position).magnitude ;
		transform.localScale = new Vector3(size,size,size);

        float alpha = GetAlpha();

        isVisible = alpha > 0f;

		if(alpha < 0f)
		{
			alpha = 0;
		}

		cachedRenderer.material.color = new Color (1f, 1f, 1f, Mathf.Lerp(cachedRenderer.material.color.a, alpha, 0.1f));
		spriteRenderer.material.color = cachedRenderer.material.color;

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

            LayerMask mask = (1 << (LayerMask.NameToLayer("Default") | 1 << (LayerMask.NameToLayer("InteractiveComponents")))); //~(1 << (LayerMask.NameToLayer("Trigger") | LayerMask.NameToLayer("Character")));
            RaycastHit hit;

            if (Camera.main != null && 
                Physics.Raycast(Camera.main.transform.position, -objToCamera.normalized, out hit, objToCamera.magnitude, mask) &&
                hit.transform != null &&
                connectedObject != null)
            {
                if (hit.transform.gameObject != connectedObject.gameObject &&
                    (connectedObject.transform.parent == null ||
                    hit.transform.gameObject != connectedObject.transform.parent.gameObject) &&
                    (hit.transform.parent == null ||
                    hit.transform.parent.gameObject != connectedObject.gameObject)
                    )
                {
                    return 0f;
                }
            }
        }

		float distFromSilja = Vector3.Distance (connectedObject.position, interactor.transform.position);

		distFromSilja /= 4f;

		//float dist = Mathf.Max (distFromCenter, distFromSilja);
		float dist = distFromSilja;

		return 1f - (dist*dist);
	}
}
