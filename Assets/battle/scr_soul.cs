using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_soul : MonoBehaviour
{
	private float speed;
	[SerializeField] private NewKrisController player;
	public Slider healthslider;
	public GameObject hptext;
	public Rigidbody2D rb;
	public Vector2 moveInput;
	Text realhptext;

	void Start()
	{
		speed = 1.2f;
		if (hptext) realhptext = hptext.GetComponent<Text>();
		if (!realhptext) realhptext = GetComponentInChildren<Text>();
		if (!player) player = FindObjectOfType<NewKrisController>();
	}

	void Update()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");
		moveInput = new Vector2(moveX, moveY).normalized;

		speed = (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.B)) ? 1f : 1.7f;

		if (realhptext && player)
			realhptext.text = player.hp.ToString("0");
		healthslider.value = player.hp;
	}

	void FixedUpdate()
	{
		if (rb) rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("attack") && player)
		{
			Debug.Log("hurt");
			player.hp -= 2.5f;
		}
	}
}
