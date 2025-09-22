using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movepositionxseconds : MonoBehaviour {
public Transform player;   // El objeto a mover
    public Vector3 point;      // Hacia dónde moverse
    public float time;         // Duración en segundos

    private Vector3 startPos;
    private float elapsedTime;
    public Animator animator;
	void Start()
	{
		startPos = player.position; // Guardamos la posición inicial
		elapsedTime = 0f;
		
    }
    void Update()
    {
		animator.SetBool("slash", true);
		animator.Play("Slash");
        if (elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / time; // Normalizar entre 0 y 1
			player.position = Vector3.Lerp(startPos, point, t);
		}
    }
}