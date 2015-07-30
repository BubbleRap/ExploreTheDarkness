using UnityEditor;
using UnityEngine;

using System;
using System.IO;

public class MenuTest : MonoBehaviour {

	[MenuItem ("Utility/Remove Empty Folders")]
	static void RemoveEmptyFolders () 
	{
		RemoveDirectory(Application.dataPath);
		AssetDatabase.Refresh();
	}

	static void RemoveDirectory( string path )
	{
		string[] subDirs = Directory.GetDirectories( path );

		foreach( string dir in subDirs )
		{
			RemoveDirectory( dir );
		}

		string[] files = Directory.GetFiles( path );
		
		if( files.Length == 0 )
		{
			Directory.Delete(path);
			File.Delete(path + ".meta");

			Debug.Log(path + " removed");
		}
	}
}
