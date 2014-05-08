using UnityEngine;
using System.Collections.Generic;

public class MenuOption : MonoBehaviour {

	public bool highlighted = false;
	public string action;

	public Material regularMaterial;
	public Material highlightedMaterial;

	public void ToggleHighlight(){

		highlighted = !highlighted;
		if (highlighted)
			GetComponent<MeshRenderer>().materials = new Material[]{highlightedMaterial};
		else
			GetComponent<MeshRenderer>().materials = new Material[]{regularMaterial};
	}

}
