using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOutor : MonoBehaviour
{
    private string room;
	private float playerx;
	private float playery;
    public bool FadeOut;
    public CanvasGroup CanvasGroup;
    private float currentValue = 0;
	private float fadespeed = 0.20f;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
		if (FadeOut)
		{
			CanvasGroup.alpha = currentValue;
			currentValue += fadespeed;

			if (currentValue >= 1)
			{
				FadeOut = false;
				StartCoroutine(LoadAndMove());
			}
		}
        if (!FadeOut)
        {
            CanvasGroup.alpha = currentValue;
            currentValue -= fadespeed;

			if (currentValue <= 0)
			{
				Destroy(gameObject);
			}
        }
    }
	IEnumerator LoadAndMove()
	{
		SceneManager.LoadScene(room);
		yield return null;

		NewKrisController[] allKris = (NewKrisController[])FindObjectsOfType(typeof(NewKrisController));
		foreach (NewKrisController kris in allKris)
		{
			Vector3 pos = kris.transform.position;
			pos.x = playerx;
			pos.y = playery;
			kris.transform.position = pos;
		}

		currentValue = 1f;
		if (CanvasGroup) CanvasGroup.alpha = 1f;
	}

	public void pass_on_values(string targetRoom, float _x, float _y, float thyadespeed)
	{
		room = targetRoom;
		fadespeed = thyadespeed;
		playerx = _x;
		playery = _y;
    }
}
