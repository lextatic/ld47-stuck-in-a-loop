using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CurveManager))]
public class CurveManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		CurveManager myScript = (CurveManager)target;
		if (GUILayout.Button("Build Scene Objects"))
		{
			myScript.BuildSceneObjects(false);
		}
	}
}

