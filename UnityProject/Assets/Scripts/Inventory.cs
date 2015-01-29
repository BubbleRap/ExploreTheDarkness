using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	private int lilleBrorPieces = 0;
	public bool lillebrorComplete = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int getLilleBrorPieces()
	{
		return lilleBrorPieces;
	}

	public void setLilleBrorPieces()
	{
		lilleBrorPieces ++;
	}
}
