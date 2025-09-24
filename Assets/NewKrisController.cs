using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewKrisController : MonoBehaviour
{
	private float speed;
	public Animator animator;
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
			if (speed <= 3){
				speed += 0.2f;
			}
		}
		if (!Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.B)){
			if (speed > 1.2){
				speed -= 0.2f;
			}
		}

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
