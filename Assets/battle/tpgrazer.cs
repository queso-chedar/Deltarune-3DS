using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tpgrazer : MonoBehaviour
{

	public tpbar tpbar;
	SpriteRenderer m_SpriteRenderer;

	private float alpha;
	void Start()
	{
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		alpha -= 0.07f;
		m_SpriteRenderer.color = new Color(1f, 1f, 1f, alpha);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("attack"))
		{
			alpha = 1;
			tpbar.TpValue += 2;
			//Debug.Log(tpbar.TpValue.ToString());
		}
	}
}
