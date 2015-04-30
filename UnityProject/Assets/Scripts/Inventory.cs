using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	private int lilleBrorPieces = 0;
	public bool lillebrorComplete = false;

	private List<string> inv = new List<string>(); 

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

	public void putInInventory(string theObject)
	{
		inv.Add (theObject);
	}

	public bool lookUpInventory(string theObject)
	{
		int pos = inv.IndexOf(theObject);

		if(pos > -1)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
