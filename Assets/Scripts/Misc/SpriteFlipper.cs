using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Flips based on rigidbody velocity
// Assumes default sprite is facing right
[RequireComponent(typeof(Rigidbody2D))]
public class SpriteFlipper : MonoBehaviour
{
	public Transform sprite;

	private Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (rb.velocity.x > 0)
		{
			sprite.localScale = new Vector2(1f, 1f);
		}
		else if (rb.velocity.x < 0)
		{
			sprite.localScale = new Vector2(-1f, 1f);
		}
	}
}
