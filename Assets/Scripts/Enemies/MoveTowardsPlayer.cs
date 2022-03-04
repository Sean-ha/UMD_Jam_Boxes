using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

// AI for basic moving towards player behavior
public class MoveTowardsPlayer : MonoBehaviour
{
	public float speed;
	[Tooltip("Enemy will move towards player if distance between them is less than this value")]
	public float maxDistance;

	private PlayerController player;
	private Rigidbody2D rb;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		player = FindObjectOfType<PlayerController>();
	}

	private void FixedUpdate()
	{
		Vector2 playerPos = player.transform.position;

		if (Vector2.Distance(playerPos, transform.position) > maxDistance)
		{
			Vector2 diff = playerPos - (Vector2)transform.position;
			rb.AddForce(diff.normalized * speed);
		}
	}
}