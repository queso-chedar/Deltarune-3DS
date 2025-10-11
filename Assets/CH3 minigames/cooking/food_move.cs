using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food_move : MonoBehaviour
{

	public List<Sprite> allSprites;
	private SpriteRenderer spriteRenderer;
	private string state;
	public GameObject Player;
	private float yoffset;
	private static int stackCount = 0;
	private int myStackIndex = -1;
	private bool stacked = false;
	void Start()
	{
		yoffset = 0;
		state = "fall";
		if (gameObject.name.Contains("(Clone)"))
			transform.position = new Vector3(Random.Range(-3.37f, 3.6f), transform.position.y, transform.position.z);

		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = allSprites[Random.Range(0, allSprites.Count)];
	}

	// Update is called once per frame
	void Update()
	{
		if (state == "fall")
		{
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -1f, transform.position.z), Time.deltaTime * 1.6f);

			if (Mathf.Approximately(transform.position.y, -1f) && gameObject.name.Contains("(Clone)"))
			{
				Destroy(gameObject);
			}
		}

	if (state == "carry")
	{
		float baseY = Player.transform.position.y + 0.6f + (myStackIndex * 0.3f);

		float x = Player.transform.position.x;
		if (myStackIndex > 0)
		{
			float shakeAmount = 0.02f + (myStackIndex * 0.015f);
			float shakeSpeed  = 6f + (myStackIndex * .5f);
			x += Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
		}

		transform.position = new Vector3(x, baseY, transform.position.z);
	}
	}


	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && state != "carry")
		{
			state = "carry";
			Player = collision.gameObject;
			myStackIndex = stackCount;
			stackCount++;
			stacked = true;
		}
		if (collision.CompareTag("Food") && !stacked)
		{
			food_move other = collision.GetComponent<food_move>();
			if (other != null && other.state == "carry" && other.Player != null)
			{
				state = "carry";
				Player = other.Player;
				myStackIndex = stackCount;
				stackCount++;
				stacked = true;
			}
		}
	}
}
