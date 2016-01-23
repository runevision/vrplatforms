using UnityEngine;
using System.Collections;
using Rucrede;

public class Eye : MonoBehaviour {
	
	public Transform EyeBall;
	
	protected Material EyeBallMaterial;
	protected float _normalDilation;
	protected Tween _dilationTween;
	protected EyeStatesEnum State = EyeStatesEnum.HAPPY;
	
	public float Dilation {
		get {
			return EyeBallMaterial.GetFloat("_Dilation");
		}
		set {
			EyeBallMaterial.SetFloat("_Dilation", value);
		}
	}

	public void SetState(EyeStatesEnum state) {
		State = state;
		InitEyeState(State);
	}

	// Use this for initialization
	void Awake () {
		EyeBallMaterial = EyeBall.GetComponent<MeshRenderer>().material;
		_normalDilation = Dilation; 	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateEyeState();
	}
		
	protected void InitEyeState(EyeStatesEnum state) {
		switch(state){
			case EyeStatesEnum.HAPPY:
				_dilationTween = Tween.to(_normalDilation, _normalDilation * 1.3f, 4.0f, Tween.EaseType.easeInCubicOutSine,
					(Tween tween) => {Dilation = (float)tween.Value;}, null, 0f, true, -1);
				break;
			case EyeStatesEnum.DRUGGED:
				Dilation = _normalDilation * 1.6f;
				break;
			default:
			
			break;
		}
	}	
		
	protected void UpdateEyeState() {
	
	}
	
	
	
}
