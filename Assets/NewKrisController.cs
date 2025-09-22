using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewKrisController : MonoBehaviour
{
	public float speed;
	public Animator animator;
	public Rigidbody2D rb;
	public Vector2 moveInput;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");
		moveInput = new Vector2(moveX, moveY).normalized;
		Vector2 circlePad = UnityEngine.N3DS.GamePad.CirclePad;

		animator.SetFloat("Horizontal", moveX);
		animator.SetFloat("Vertical", moveY);
		animator.SetFloat("Speed", moveInput.sqrMagnitude);
	}
	void FixedUpdate()
	{
		// Fisicas
		rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
	}
}
