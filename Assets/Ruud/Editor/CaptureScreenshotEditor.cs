using UnityEngine;
using UnityEditor;
using System.Collections;
using Rucrede.SwipeStory;

[CustomEditor(typeof(CaptureScreenshot))]
public class CaptureScreenshotEditor : Editor  {

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		CaptureScreenshot capture = (CaptureScreenshot)target;
		if (GUILayout.Button("Open screenshots folder")) {
			capture.OpenScreenshotsFolder();
		}
	}
}