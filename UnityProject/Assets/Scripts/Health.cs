using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	private GameObject siljaCharacter = null;
	public int health = 4;
	public SiljaBehaviour siljaBeh;

	private float barDisplay = 0;
	private Vector2 pos = new Vector2(Screen.width/2-400,Screen.height-50);
	private Vector2 size = new Vector2(800,20);
	private Texture2D progressBarEmpty;
	private Texture2D progressBarFull;

	private GUIStyle currentStyle = new GUIStyle();
	public Texture2D texture;

	private GUIStyle currentStyle2 = new GUIStyle();
	public Texture2D texture2;

	// Use this for initialization
	void Start () {
		siljaCharacter = GameObject.FindGameObjectWithTag("Player");
	}

	void OnGUI () {
		// Make a background box
		if(siljaBeh.darkMode)
		{
			GUI.skin.label.fontSize = 100;
			GUI.Label(new Rect (40,20,100,300), health.ToString());

        	currentStyle.normal.background = texture;
        	currentStyle2.normal.background = texture2;


			GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
		        GUI.Box (new Rect (0,0, size.x, size.y),progressBarEmpty, currentStyle);
		 
		        // draw the filled-in part:
		        GUI.BeginGroup (new Rect (0, 0, size.x * (barDisplay/1.75f), size.y));
		            GUI.Box (new Rect (0,0, size.x, size.y),progressBarFull, currentStyle2);
		        GUI.EndGroup ();
	 
	    	GUI.EndGroup ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		barDisplay = siljaBeh.getTeddyLight();

		if(health <= 0)
		{
			Destroy(siljaCharacter);
			Application.LoadLevel(0);
		}
	}

	public void looseLife() {
		health --;
	}
}
