using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoblinBehavior : MonoBehaviour
{
	// Description: Walk adjacent to player for a while, and then sprint towards player, then go back to adjacent, ...
	public float adjacentForce;
	public float adjacentDuration;
	public float adjacentWait;
	public float toPlayerForce;
	public float toPlayerDuration;
	public float toPlayerWait;

	private PlayerController player;
	private Rigidbody2D rb;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		player = FindObjectOfType<PlayerController>();

		StartCoroutine(GoblinBehaviorCR());
	}

	private IEnumerator GoblinBehaviorCR()
	{
		while (true)
		{
			AdjacentMovement();

			yield return new WaitForSeconds(adjacentWait);

			TowardsPlayerMovement();

			yield return new WaitForSeconds(toPlayerWait);
		}
	}

	private void AdjacentMovement()
	{
		Vector2 playerPos = player.transform.position;

		Vector2 dir = HelperFunctions.GetDirectionTowards(transform.position, playerPos);
		
		// Get random number (-1 or 1). Because goblin can walk either adjacent in either direction
		int rand = Random.Range(0, 2);
		if (rand == 0)
			rand = -1;

		Vector2 perp = rand * Vector2.Perpendicular(dir).normalized;
		DOTween.To(() => 0f, (float val) => rb.AddForce(perp * val), adjacentForce, adjacentDuration).SetUpdate(UpdateType.Fixed);
	}

	private void TowardsPlayerMovement()
	{
		Vector2 playerPos = player.transform.position;

		Vector2 dir = HelperFunctions.GetDirectionTowards(transform.position, playerPos);

		DOTween.To(() => 0f, (float val) => rb.AddForce(dir * val), toPlayerForce, toPlayerDuration).SetUpdate(UpdateType.Fixed);
	}
}
