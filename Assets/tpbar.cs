using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tpbar : MonoBehaviour {
	public Text TpText;
	public Slider TpSlider;
	public int TpValue = 0;

	// Use this for initialization
	void Start () {
		TpValue = 0;
	}

	// Update is called once per frame
	void Update()
	{
		TpValue = Mathf.Clamp(TpValue, 0, 100);
		TpText.text = TpValue + "%";
		TpSlider.value = TpValue;

		if (TpValue > 99)
		{
			TpText.text = "MAX";
			TpText.color = Color.yellow;
		}
		else
			TpText.color = Color.white;
	}
}
