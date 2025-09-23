using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movepositionxseconds : MonoBehaviour
{
	public Transform player;   // El objeto a mover
	public GameObject point;
	public float time;         // Duración en segundos
	public float timecorroutine;
	public float timecorroutineidle;
	private Vector3 startPos;
	private float elapsedTime;
	public Animator animator;
	public AfterimageTrail afterimagescript;
	public CameraFollow camerascript;

	void Start()
	{
		afterimagescript.enabled = true;
		startPos = player.position; // Guardamos la posición inicial
		elapsedTime = 0f;
		StartCoroutine(Slash());
		camerascript.enabled = false;
	}
	void Update()
	{
		if (elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / time; // Normalizar entre 0 y 1
			player.position = Vector3.Lerp(startPos, point.transform.position, t);
		}
	}
	IEnumerator Slash()
	{
		yield return new WaitForSeconds(timecorroutine);
		afterimagescript.enabled = false;
		animator.Play("Slash");
		StartCoroutine(Idle());
		yield return null;
	}
	IEnumerator Idle()
	{
		yield return new WaitForSeconds(timecorroutineidle);
		animator.Play("FightIdle");
		yield return null;
    }
}