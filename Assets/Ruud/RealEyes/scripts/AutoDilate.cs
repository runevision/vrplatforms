using UnityEngine;
using System.Collections;

public class AutoDilate : MonoBehaviour {
	public float maxDilation = 2f;
	public float minDilation = 0f;
	public float lightFactor = 1f;
	public Light[] lights; //array of lights used to determine total light level;
	
	private float totalLightLevel;
	private Renderer[] eyes;
	
	void Start (){
		lights = FindObjectsOfType(typeof(Light)) as Light[];
		eyes = GetComponentsInChildren<Renderer>();
	}
	
	void Update () {
	
		totalLightLevel = 0f;
		
		foreach ( Light light in lights ){
			
			if (light){
				
					if (light.enabled){
					
					float thisLightLevel = 0f;
					Vector3 n = transform.forward;
					
					if (light.type == LightType.Point){
						Vector3 l = light.transform.position - transform.position;
						float r = Mathf.Min(1,l.magnitude/light.range);
						float nDotL = Mathf.Max(Vector3.Dot(n, l.normalized),0);
						float attenuation = 1.0f / (1.0f + 25.0f*r*r); //quadratic attenuation 
						thisLightLevel =  light.intensity * nDotL * attenuation;
					}
					
					else if (light.type == LightType.Directional){
						
						Vector3 l = -light.transform.forward;
						thisLightLevel = light.intensity * Mathf.Max(Vector3.Dot(n, l),0);
					}
					
					else if (light.type == LightType.Spot){
						Vector3 l = light.transform.position - transform.position;
						//determine if the eye is in the spotlight cone;
						float cone = Mathf.Cos(light.spotAngle/2 * Mathf.Deg2Rad);
						if (Vector3.Dot(-light.transform.forward, l.normalized) > cone) {
							float distance = l.magnitude/light.range;
							float nDotL = Mathf.Max(Vector3.Dot(n, l.normalized),0);
							float attenuation =  Mathf.Lerp(light.intensity, 0, distance);
							thisLightLevel = light.intensity * nDotL * attenuation;
						}
					}
					
					totalLightLevel += thisLightLevel * light.color.grayscale;
				}
			}
		}
		
		totalLightLevel += RenderSettings.ambientLight.grayscale * .5f;
		
		float dilation = Mathf.Lerp(maxDilation, minDilation, totalLightLevel/lightFactor);
		foreach (Renderer eye in eyes) {
			eye.material.SetFloat("_Dilation", dilation);
		}
	}
	
	void OnGUI(){
		GUI.Box(new Rect(0, 0, 100, 50), totalLightLevel.ToString());
	}
}