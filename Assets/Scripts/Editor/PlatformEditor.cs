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

		EditorGUILayout.Space();

		EditorGUI.BeginChangeCheck ();
		float size = EditorGUILayout.Slider ("Size", Controller.instance.size, 0.5f, 2);
		if (EditorGUI.EndChangeCheck ()) {
			Controller.instance.size = size;
			SceneView.RepaintAll ();
		}

		Controller.instance.duration = EditorGUILayout.Slider ("Duration", Controller.instance.duration, 4, 32);
	}
}
