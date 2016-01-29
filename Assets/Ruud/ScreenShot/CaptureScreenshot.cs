using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Diagnostics;
using System;
using System.Collections;

public class CaptureScreenshot : MonoBehaviour {

	public int superSizeFactor = 4;
	public string Filename;
	public bool DatePostFix = true;
	public string Key = "s";

	void LateUpdate () {
		if (Input.GetKeyDown(Key)) {
			string theFileName = Filename;
			
			if (DatePostFix)
				theFileName = theFileName + "_" + System.DateTime.Now.ToLongTimeString();
			
			theFileName += ".png";
			Application.CaptureScreenshot(Application.persistentDataPath + "/" + theFileName, superSizeFactor);
			UnityEngine.Debug.Log("screenshot saved to:\n" + Application.persistentDataPath + "/" + theFileName);		
		}
	}
	
	[ContextMenu("open screenshots")]
	public void OpenScreenshotsFolder() {
	#if UNITY_EDITOR
		Process p = new Process();
		p.StartInfo.FileName = "open";
		p.StartInfo.Arguments = ".";
		p.StartInfo.WorkingDirectory =  Application.persistentDataPath;
		p.StartInfo.UseShellExecute = true;
		p.Start();
	#endif
	}
	
}
