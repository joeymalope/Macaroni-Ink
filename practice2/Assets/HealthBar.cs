using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	public Image healthbar;
	public float fillAmount=1;
	public float LerpSpeed=3;
	public Color lowColor;
	public Color HighColor;
	float outMax=1,outMin=0,inMax=150,inMin=0;
	public float HP=150;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//decreaseHealth ();
		updateHealth ();
	}

	void updateHealth (){

		//healthbar.fillAmount=Mathf.Lerp(healthbar.fillAmount,fillAmount,Time.deltaTime*LerpSpeed);
		healthbar.fillAmount=Mathf.Lerp(healthbar.fillAmount,Map(HP,inMax,inMin, outMax, outMin),Time.deltaTime*LerpSpeed);
		healthbar.color = Color.Lerp (lowColor, HighColor, healthbar.fillAmount);
	}


	public void decreaseHealth(float damage){
		HP -= damage;
		if (HP < inMin)
			HP = inMin;
	}

	public void increaseHealth(float damage){
		HP += damage;
		if (HP > inMax)
			HP = inMax;
	}


	float Map(float value,float inMax,float inMin,float outMax,float outMin){
		return ((value - inMin) / (inMax - inMin)) * outMax-outMin;
	}
}
