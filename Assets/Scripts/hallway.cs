using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class hallway : MonoBehaviour
{
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private string room;
    [SerializeField] private GameObject fadeOutPrefab;
    public float fadeoutspeed = 0.20f;

    private Rigidbody2D body;
    private Animator anim;

    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("hallwaytriggerer"))
        {
            GameObject fadeObj = Instantiate(fadeOutPrefab);
            fadeObj.GetComponent<FadeOutor>().pass_on_values(room, x, y, fadeoutspeed);
            fadeObj.GetComponent<FadeOutor>().FadeOut = true;
            Destroy(gameObject);
        }
    }

    private void Update()
    {

    }
}
