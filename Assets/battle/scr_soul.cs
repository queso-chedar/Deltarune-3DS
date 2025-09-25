using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_soul : MonoBehaviour
{
	private float speed;
	public Rigidbody2D rb;
	public Vector2 moveInput;

	// Use this for initialization
	void Start()
	{
		speed = 1.2f;
	}

	// Update is called once per frame
	void Update()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");
		moveInput = new Vector2(moveX, moveY).normalized;
		Vector2 circlePad = UnityEngine.N3DS.GamePad.CirclePad;

		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.B)){
			speed = 1f;
		}
		if (!Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.B)){
			speed = 1.7f;
		}
	}
	void FixedUpdate()
	{
		// Fisicas
		rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
	}
}
