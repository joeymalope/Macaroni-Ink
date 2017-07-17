using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tooltip : MonoBehaviour {

	[System.Serializable]
	public class AnimationSettings
	{
		public enum OpenStyle {WidthToHeight,HeightToWidth,WidthTAndHeight};//OS == open style
		public OpenStyle openStyle;
		public float widthSmooth = 4.6f,heightSmooth= 4.6f;
		public float textSmooth = 0.1f;

		[HideInInspector]
		public bool widthOpen = false,HeightOpen=false;
		public void initialize(){
			widthOpen = false;
			HeightOpen = false;
		}
	}

	[System.Serializable]
	public class UISettings{
		public Image textbox; //will contain text content
		public Text text; // the text
		public Vector2 initialBoxSize  = new Vector2(0.25f,0.25f);
		public Vector2 OpenBoxSize  = new Vector2(400,200);
		public float snapToSizeDistance = 0.25f;
		public float lifeSpan =5;

		[HideInInspector]
		public bool opening = false;
		[HideInInspector]
		public Color textColor ;
		[HideInInspector]
		public Color textBoxColor;
		[HideInInspector]
		public RectTransform textBoxRect;
		[HideInInspector]
		public Vector2 currentSize;

		public void initialize(){
			textBoxRect = textbox.GetComponent<RectTransform> ();
			textBoxRect.sizeDelta = initialBoxSize; //size delta will have strange results if anchors aren't aligned
			currentSize = textBoxRect.sizeDelta;
			opening = false;
			//text color alpha to 0
			textColor = text.color;
			textColor.a = 0;
			text.color = textColor;
			textBoxColor = textbox.color;
			textBoxColor.a = 1;
			textbox.color = textBoxColor;

			textbox.gameObject.SetActive (false);
			text.gameObject.SetActive (false);
		}
	}

	public AnimationSettings animSettings = new AnimationSettings ();
	public UISettings UIsettings = new UISettings ();
	// Use this for initialization
	float lifeTimer=0;

	void OpenToolTip(){
		switch (animSettings.openStyle) {
		case Tooltip.AnimationSettings.OpenStyle.HeightToWidth:
			HeightToWidth ();
			break;
		case  Tooltip.AnimationSettings.OpenStyle.WidthToHeight:
			WidthToHeight ();
			break;
		case  Tooltip.AnimationSettings.OpenStyle.WidthTAndHeight:
			WidthAndHeight ();
			break;
		default:
			Debug.Log ("We cant find you open style!!");
			break;
		}

		UIsettings.textBoxRect.sizeDelta = UIsettings.currentSize;
	}

	public void HeightToWidth(){
		if (!animSettings.HeightOpen)
			openHeight ();
		else
			openWidth ();
	}

	public void WidthToHeight(){
		if (!animSettings.widthOpen)
			openWidth ();
		else
			openHeight ();
	}

	public void WidthAndHeight(){
			if (!animSettings.widthOpen)
				openWidth ();
		else if (!animSettings.HeightOpen)
				openHeight ();
	}

	void openWidth(){
		UIsettings.currentSize.x = Mathf.Lerp (UIsettings.currentSize.x, UIsettings.OpenBoxSize.x,animSettings.widthSmooth*Time.deltaTime);

		if (Mathf.Abs (UIsettings.currentSize.x - UIsettings.OpenBoxSize.x) < UIsettings.snapToSizeDistance) {
			UIsettings.currentSize.x = UIsettings.OpenBoxSize.x;
			animSettings.widthOpen = true;
		}
	}

	void openHeight(){
		UIsettings.currentSize.y = Mathf.Lerp (UIsettings.currentSize.y, UIsettings.OpenBoxSize.y,animSettings.heightSmooth*Time.deltaTime);

		if (Mathf.Abs (UIsettings.currentSize.y - UIsettings.OpenBoxSize.y) < UIsettings.snapToSizeDistance) {
			UIsettings.currentSize.y = UIsettings.OpenBoxSize.y;
			animSettings.HeightOpen = true;
		}
	}

	void Start () {
		UIsettings.initialize ();
		animSettings.initialize ();
	}

	void StartOpen () {//called when button is clicked
		UIsettings.opening = true;
		UIsettings.textbox.gameObject.SetActive (true);
		UIsettings.text.gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (UIsettings.opening) {
			OpenToolTip ();

			if (animSettings.widthOpen && animSettings.HeightOpen) {
				lifeTimer += Time.deltaTime;
				if (lifeTimer > UIsettings.lifeSpan) 
					fadeTextIn ();
				else 
					fadeToolTipOut ();
			}
		}
	}

	public void fadeTextIn(){
		UIsettings.textColor.a = Mathf.Lerp (UIsettings.textColor.a, 1,animSettings.textSmooth*Time.deltaTime);
		UIsettings.text.color = UIsettings.textColor;
	}

	public void fadeToolTipOut (){
		UIsettings.textColor.a = Mathf.Lerp (UIsettings.textColor.a, 0,animSettings.textSmooth*Time.deltaTime);
		UIsettings.text.color = UIsettings.textColor;
		UIsettings.textBoxColor.a = Mathf.Lerp (UIsettings.textBoxColor.a, 0,animSettings.textSmooth*Time.deltaTime);
		UIsettings.textbox.color = UIsettings.textBoxColor;

		if (UIsettings.textBoxColor.a < 0.01f) {
			UIsettings.opening = false;
			animSettings.initialize ();
			UIsettings.initialize ();
			lifeTimer = 0;
		}
	}
}
