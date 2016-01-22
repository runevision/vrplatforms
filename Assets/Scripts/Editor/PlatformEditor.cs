using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (Platform))]
public class PlatformEditor : Editor {

	public override void OnInspectorGUI () {
		Platform platform = target as Platform;

		DrawDefaultInspector();

		EditorGUILayout.Space();

		if (GUILayout.Button("Switch Ends")) {
			Vector3 temp = platform.endA.position;
			platform.endA.position = platform.endB.position;
			platform.endB.position = temp;
			Event.current.Use ();
			SceneView.RepaintAll ();
		}
	}
}
