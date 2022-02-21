using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float movementSpeed;

	private Rigidbody2D rb;
	private AttackManager attackManager;

	private float horizontal, vertical;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		attackManager = GetComponent<AttackManager>();
	}

	private void Update()
	{
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");

		if (Input.GetMouseButtonDown(0))
		{
			attackManager.InitiateAttack();
		}
	}

	private void FixedUpdate()
	{
		Vector2 velVector = new Vector2(horizontal, vertical).normalized;
		velVector *= movementSpeed;
		rb.velocity = velVector;
	}
}
