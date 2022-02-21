using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
	// Set when swing is created
	public Transform player { get; set; }

	private float swingForce;
	private Vector2 direction;

	public void InitiateSwing(Transform player, float swingForce, Vector2 direction)
	{
		this.player = player;
		this.swingForce = swingForce;
		this.direction = direction;

		StartCoroutine(SwingBehavior());
	}

	private IEnumerator SwingBehavior()
	{
		yield return new WaitForSeconds(0.07f);

		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Box layer
		if (collision.gameObject.layer == LayerMask.NameToLayer("Box"))
		{
			Vector2 pointOnBox = collision.ClosestPoint(player.position);

			// Vector2 direction = HelperFunctions.GetDirectionTowards(player.position, pointOnBox);

			collision.GetComponent<Box>().PushBox(pointOnBox, direction, swingForce);
		}
	}
}
