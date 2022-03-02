using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Grapple : MonoBehaviour
{
	private LineRenderer lr;

	private GrappleManager myManager;

	private void Awake()
	{
		lr = GetComponent<LineRenderer>();
	}

	// Destination = where the box will end up (player's location)
	public void StartBoxGrapple(GrappleManager myManager, Box box, Vector2 destination, float speed, float debugVal)
	{
		this.myManager = myManager;

		// Dist between player and box
		float distance = Vector2.Distance(box.transform.position, destination);
		// Adjust speed based on distance
		speed = Mathf.Pow((distance * speed), debugVal);
		float time = distance / speed;

		lr.SetPosition(0, destination);

		box.PullBox(this, destination, speed, time);

		StartCoroutine(FollowTransform(box.transform, time, () => KillBoxGrapple()));
	}

	// Destination = where the player will end up
	public void StartWallGrapple(PlayerController player, Vector2 destination, float speed)
	{
		// Dist between player and box
		float distance = Vector2.Distance(player.transform.position, destination);
		// Adjust speed based on distance
		speed = Mathf.Pow((distance * speed), 0.5f);
		float time = distance / speed;

		lr.SetPosition(0, destination);

		player.PullSelf(this, destination, speed, time);

		StartCoroutine(FollowTransform(player.transform, time, () => KillWallGrapple()));
	}

	public void KillWallGrapple()
	{
		Destroy(gameObject);
	}

	public void KillBoxGrapple()
	{
		myManager.GainGrapple();
		Destroy(gameObject);
	}

	private IEnumerator FollowTransform(Transform toFollow, float time, Action onComplete)
	{
		float currTime = 0f;
		while (currTime < time)
		{
			lr.SetPosition(1, toFollow.position);
			currTime += Time.deltaTime;
			yield return null;
		}
		onComplete.Invoke();
	}
}
