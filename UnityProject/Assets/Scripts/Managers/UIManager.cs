using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	static private UIManager s_Instance;
	static public UIManager Instance
	{
		get { return s_Instance; }
	}

	public Texture2D m_cursorDrag;
	public Texture2D m_cursorDragging;
	public Texture2D m_cursorClick;
	public RectTransform mouseBack;

	private void Awake()
	{
		s_Instance = this;
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
		mouseBack.gameObject.SetActive(boolean);
	}
}
