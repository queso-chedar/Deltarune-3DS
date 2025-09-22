using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutorIn : MonoBehaviour
{

	public bool FadeOut;
	public bool FadeInAtStart;
	public CanvasGroup CanvasGroup;
	private float currentvalue = 0;
	void Start()
	{
		if (FadeInAtStart == true)
		{
			currentvalue = 1;
		}
	}
	void Update()
	{
		if (FadeInAtStart == true)
		{
			CanvasGroup.alpha = currentvalue;
			currentvalue -= 0.05f;
			if (currentvalue <= 0)
			{
				FadeInAtStart = false;
			}
		}
		if (FadeOut == true)
		{
			CanvasGroup.alpha = currentvalue;
			currentvalue += 0.05f;
			if (currentvalue >= 1)
			{
				FadeOut = false;
			}
		}
	}
}
