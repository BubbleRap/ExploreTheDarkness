using UnityEngine;
using UnityEditor;
using System.IO;

public class MaterialsAlphaUtility : Editor 
{
	[MenuItem("Utility/Set All Alphas To Zero")]
	static public void SetMaterialsAlphaToZero()
	{
		string[] materialsInProject = Directory.GetFiles( "Assets\\", "*.mat", SearchOption.AllDirectories );

		foreach( string materialAssetPath in materialsInProject )
		{
			Material targetMaterial = (Material)AssetDatabase.LoadAssetAtPath( materialAssetPath.Replace( "\\", "/" ), typeof(Material) );
			string propertyName = "";

			if( targetMaterial.HasProperty( "_Tint" ) )
			   propertyName = "_Tint";
			
			if( targetMaterial.HasProperty( "_Color" ) )
				propertyName = "_Color";

			if( string.IsNullOrEmpty(propertyName) )
				continue;

			Color materialColor = targetMaterial.GetColor(propertyName);

			materialColor.a = 0f;

			targetMaterial.SetColor(propertyName, materialColor );
		}
	}

	[MenuItem("Utility/Set All Alphas To One")]
	static public void SetMaterialsAlphaToOne()
	{		
		string[] materialsInProject = Directory.GetFiles( "Assets\\", "*.mat", SearchOption.AllDirectories );
		
		foreach( string materialAssetPath in materialsInProject )
		{
			Material targetMaterial = (Material)AssetDatabase.LoadAssetAtPath( materialAssetPath.Replace( "\\", "/" ), typeof(Material) );
			string propertyName = "";
			
			if( targetMaterial.HasProperty( "_Tint" ) )
				propertyName = "_Tint";
			
			if( targetMaterial.HasProperty( "_Color" ) )
				propertyName = "_Color";
			
			if( string.IsNullOrEmpty(propertyName) )
				continue;
			
			Color materialColor = targetMaterial.GetColor(propertyName);
			
			materialColor.a = 1f;
			
			targetMaterial.SetColor(propertyName, materialColor );
		}
	}
}
