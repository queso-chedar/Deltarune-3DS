using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cookingkrismove : MonoBehaviour
{
	private float speed;
	public Animator animator;
	public Rigidbody2D rb;
	public Vector2 moveInput;
	private float deltaTime = 0.0f;

    private Rigidbody2D body;
    private Animator anim;
	public bool grounded;
	private float stored_speed;
	private bool hurt;

	// Use this for initialization
	void Start()
	{
		hurt = false;
		speed = 1.4f;
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (hurt == false)
		{
			if (grounded)
			{
				stored_speed = Input.GetAxisRaw("Horizontal") * 0.1f;
				if (stored_speed != 0)
				{
					if (!anim.GetCurrentAnimatorStateInfo(0).IsName("move"))
						anim.Play("move", 0, 0f);
				}
				else
				{
					anim.Play("idle", 0, 0f);
				}
			}
			else
			{
				if (!anim.GetCurrentAnimatorStateInfo(0).IsName("jump"))
					anim.Play("jump", 0, 0f);
			}


			transform.Translate(stored_speed, 0, 0);
			if (Input.GetKeyDown(KeyCode.Z) && grounded || UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.B) || UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.A))
			{
				body.velocity = new Vector2(body.velocity.x, 4);
			}
		}
        else
        {
            if (grounded)
            {
				if (!anim.GetCurrentAnimatorStateInfo(0).IsName("hurt_land"))
					anim.Play("hurt_land", 0, 0f);               

				if (anim.GetCurrentAnimatorStateInfo(0).IsName("hurt_land") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !anim.IsInTransition(0))
                {
					hurt = false;
                }
            }
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.tag == ("solidcooking"))
		{
			grounded = true;
		}
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("solidcooking"))
        {
            grounded = false;
        }
    }

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("fire"))
		{
			body.velocity = new Vector2(body.velocity.x, 3);
			transform.Translate(0, transform.position.y + .7f, 0);
			hurt = true;
			anim.Play("hurt_fall", 0, 0f);
			Debug.Log("hurt!");
			Destroy(collision.gameObject);
		}
	}
}
