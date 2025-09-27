using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyhealth : MonoBehaviour
{
	public Canvas myCanvas;
	public Canvas myCanvasDown;
	public NewKrisController krisController;
	public CanvasGroup canvasGroup;
	public CanvasGroup canvasGroupDown;
	public float duracion = 2f; // tiempo total en segundos
	private float tiempo = 0f;
	public GameObject BatallaPrefab;
	//string healthstring = health.ToString();
	private float elapsedTime;
	public GameObject Player;
	private Animator playeranimator;
	public bool Finishtbattlebool;

	Text changetext;
	public GameObject funnytext;
	void Start()
	{
		changetext = funnytext.GetComponent<Text>();
		elapsedTime = 0f;
		playeranimator = Player.GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Finishtbattlebool == true)
		{
			FinishBattle();
		}
	}
	public void FinishBattle()
	{
         if (tiempo < duracion)
			{
				tiempo += .1f;
				float t = tiempo / duracion;
				canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
				canvasGroupDown.alpha = Mathf.Lerp(1f, 0f, t);
				if (canvasGroup.alpha <= 0)
				{
					BatallaPrefab.SetActive(false);
					myCanvasDown.gameObject.SetActive(false);
					myCanvas.gameObject.SetActive(false);
					krisController.enabled = true;
					playeranimator.Play("Idle_Right", 0, 0f);
				}
			}
	}
}
