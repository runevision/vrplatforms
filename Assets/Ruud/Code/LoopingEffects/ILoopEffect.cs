using UnityEngine;
using System.Collections;

public interface ILoopEffect {
	void Init();
	void SetReverse(bool value);
	void SetProgress(float value);
	float GetProgress();
	void Play();
	void Stop();
	bool IsLooping
	{
		get;
	}
	
	bool IsReversed
	{
		get;
	}
}
