using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	static private UIManager s_Instance;
	static public UIManager Instance
	{
		get { return s_Instance; }
	}

	public Texture2D m_cursorDrag;
	public Texture2D m_cursorDragging;
	public Texture2D m_cursorClick;

	private void Awake()
	{
		s_Instance = this;
	}
}
