using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public bool EmpezarBatalla = false; // el jugador toca al enemigo y la batalla empieza
	public bool JugadorEnRango = false; // el jugador esta en rango de vision del enemigo
	public float RangoVisionDistancia = 5f; // distancia del raycast
	public GameObject BatallaPrefab; // prefab del objeto con todo lo necesario para la batalla
	public GameObject wawa;
	public AudioSource audio;
	public LayerMask RangoVisionLayerMask;
	[Header("Configuracion de la ia del personaje")]
	[SerializeField] private float speed;
	[SerializeField] private float range;
	[SerializeField] private GameObject character;

	[Header("Variables De la ruta Snowgrave")]
	public bool Congelado = false; // el enemigo esta congelado
	public bool Asustado = false; // el enemigo huye del jugador si este se acerca xddd
	[Header("Otros")]
	public Animator animator;
	public KrisController krisController;
	public Animator KrisAnimator;
	public Collider2D enemyCollider; // Uso eso en lugar de getcomponent porque es más eficiente

	private bool audioEjecutado = false; // bandera para ejecutar audio solo una vez
	private bool sexo = false;
	private bool graahhhh = false;



	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (EmpezarBatalla == true)
		{
			// Empezar batalla
			Debug.Log("Empezando batallaaa");
			BatallaPrefab.SetActive(true);
			EmpezarBatalla = false; // tiene un pequeño bug al moverse que hace que la variable se active 2 veces casi al instante (arreglado)
		}
		if (JugadorEnRango == true)
		{
			RangoVisionDistancia = 999;
			animator.applyRootMotion = true;
			StartCoroutine(coroutine());
			JugadorEnRango = false;
		}


		// RaycastCircular para detectar al jugador en el rango de vision
			float radio = RangoVisionDistancia;
		RaycastHit2D hit = Physics2D.CircleCast(transform.position, radio, Vector2.zero, 0f, RangoVisionLayerMask);
		if (!JugadorEnRango && hit.collider != null && hit.collider.CompareTag("Player"))
		{
			JugadorEnRango = true;
			if (!audioEjecutado)
			{
				audio = GetComponent<AudioSource>();
				audio.PlayOneShot(audio.clip);
				audioEjecutado = true;
				wawa.SetActive(true);
			}
			if (sexo)
			{
				animator.Play("Ruddindamage");
				speed = 0;
				krisController.enabled = false;
			string idleAnim = krisController.GetIdleAnimation();
			KrisAnimator.Play(idleAnim);
			}
			else if (graahhhh)
			{
				animator.Play("RuddinMovement");
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			EmpezarBatalla = true;
			enemyCollider.enabled = false; // ya no detecta más colisiones
			JugadorEnRango = false; // nooo la politziaaaa
			sexo = true;
		}

	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, RangoVisionDistancia);
	}
	IEnumerator coroutine()
	{
		yield return new WaitForSeconds(0.75f); // Wait for 0.1 seconds
		wawa.SetActive(false);
        graahhhh = true;
		if (character != null)
		{
			Vector3 direccion = character.transform.position - transform.position;
			if (direccion.x != 0)
			{
				Vector3 escala = transform.localScale;
				escala.x = direccion.x > 0 ? Mathf.Abs(escala.x) : -Mathf.Abs(escala.x);
				transform.localScale = escala;
			}
		}
		float distance = Vector3.Distance(transform.position, character.transform.position);
		if (distance > range)
			transform.position = Vector3.MoveTowards(transform.position, character.transform.position, speed * Time.deltaTime);
	}
}
