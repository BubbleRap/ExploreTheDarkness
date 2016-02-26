using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Dialog {

	[TextArea(3,10)]
	public string Text;
	public Option[] options;
}
