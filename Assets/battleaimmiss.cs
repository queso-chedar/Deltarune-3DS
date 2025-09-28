using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleaimmiss : MonoBehaviour
{
	public FightBar battleaimscript;
	void Update()
	{
		transform.position += new Vector3(battleaimscript.velocidad, 0f, 0f);
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag("Miss"))
		{
			battleaimscript.Collider.enabled = true;
		}
	}
}
