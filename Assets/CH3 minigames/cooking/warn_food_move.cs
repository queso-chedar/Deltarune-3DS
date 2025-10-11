using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warn_food_move : MonoBehaviour {

    public List<Sprite> allSprites;
	private SpriteRenderer spriteRenderer;
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = allSprites[Random.Range(0, allSprites.Count)];		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 3.58f, transform.position.z), Time.deltaTime * 4f);

		if (Mathf.Approximately(transform.position.y, 3.58f) && gameObject.name.Contains("(Clone)"))
        {
			Destroy(gameObject);
        }
	}
}
