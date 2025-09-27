using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewKrisController : MonoBehaviour
{
	private float speed;
	public float hp;
	public Animator animator;
	public Rigidbody2D rb;
	public Vector2 moveInput;
	private float deltaTime = 0.0f;
	// Use this for initialization
	void Start()
	{
		speed = 1.2f;
		hp = 90;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
	}

	// Update is called once per frame
	void Update()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");
		moveInput = new Vector2(moveX, moveY).normalized;
		Vector2 circlePad = UnityEngine.N3DS.GamePad.CirclePad;

		if (Input.GetKey(KeyCode.X) || UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.B))
		{
			speed = 3;
		}

		if (!Input.GetKey(KeyCode.X) && !UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.B))
		{
			speed = 1.4f;
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
