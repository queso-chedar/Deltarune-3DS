using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeholder : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float range;
	[SerializeField] private GameObject character;

	private void Update()
	{
		float distance = Vector3.Distance(transform.position, character.transform.position);
		if (distance > range)
			transform.position = Vector3.MoveTowards(transform.position, character.transform.position, speed * Time.deltaTime);
	}
}