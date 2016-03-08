using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Dialog 
{
    public Indexer index;
	[TextArea(3,10)]
	public string Text;
	public Option[] options;
}

[System.Serializable]
public class Indexer
{
    public int index;
}