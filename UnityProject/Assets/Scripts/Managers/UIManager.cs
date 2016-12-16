using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    public static UIManager Instance
	{
		get { return s_Instance; }
	}

    public static Camera s_camera;
    public static Canvas s_canvas;

    private static UIManager s_Instance;

	public Texture2D m_cursorDrag;
	public Texture2D m_cursorDragging;
	public Texture2D m_cursorClick;
	public RectTransform mouseBack;
    public GameObject m_buttonPromtPrefab;

    private CanvasGroup mouseBackGroup;

	private void Awake()
	{
		s_Instance = this;
        s_camera = Camera.main;
        s_canvas = FindObjectOfType<Canvas>();

        mouseBackGroup = mouseBack.GetComponent<CanvasGroup>();

		hideCursor();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			showCursor();
		}
	}

	public void hideCursor()
	{
		Cursor.visible = false;
		Screen.lockCursor = true;
	}

	public void showCursor()
	{
		Cursor.visible = true;
		Screen.lockCursor = false;
	}

	public void lookAtUI(bool boolean)
	{
        mouseBackGroup.alpha = boolean ? 1f : 0f;
	}

    public static Vector2 WorldToCanvasPosition(Vector3 position) 
    {
        return WorldToCanvasPosition(s_canvas.transform as RectTransform, s_camera, position);
    }

    // http://answers.unity3d.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
    private static Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position) 
    {
        //Vector position (percentage from 0 to 1) considering camera size.
        //For example (0,0) is lower left, middle is (0.5,0.5)
        Vector2 temp = camera.WorldToViewportPoint(position);

        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
        temp.x *= canvas.sizeDelta.x;
        temp.y *= canvas.sizeDelta.y;

        //The result is ready, but, this result is correct if canvas recttransform pivot is 0,0 - left lower corner.
        //But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
        //We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
        //returned value will still be correct.

        temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
        temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

        return temp;
    }
}
