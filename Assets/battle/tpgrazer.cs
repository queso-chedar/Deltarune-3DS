using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tpgrazer : MonoBehaviour
{

	public tpbar tpbar;
	public GameObject Soul;
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
		transform.position = new Vector3(Soul.transform.position.x, Soul.transform.position.y, transform.position.z);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("attack"))
		{
			AttackGraze();
		}
	}
	public void AttackGraze()
	{
		alpha = 1;
		tpbar.TpValue += 2;
	}
}
