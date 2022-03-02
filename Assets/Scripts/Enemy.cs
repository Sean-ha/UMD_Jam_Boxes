using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float size;

	private BoxCollider2D boxCol;
	private Rigidbody2D rb;


	private void Awake()
	{
		boxCol = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
	}

	// Get distance from center to edge of collider at the given angle (radians)
	public float GetDistanceToEdge(float rangle)
	{
		rangle %= (Mathf.PI / 2);

		// Assume BoxCollider
		float width = boxCol.size.x / 2;
		float height = boxCol.size.y / 2;

		float cos = Mathf.Cos(rangle);
		float sin = Mathf.Sin(rangle);

		float dist = Mathf.Min(height / sin, width / cos);

		return dist;
	}

	public void PushBackWithForce(Vector2 pushForce)
	{
		pushForce *= rb.mass;

		rb.AddForce(pushForce);
	}

	public void InitiateWhiteFlash()
	{
		List<FlashWhite> flasherList = new List<FlashWhite>();
		GetComponentsInChildren(flasherList);

		foreach (FlashWhite flasher in flasherList)
		{
			flasher.InitiateWhiteFlash();
		}
	}
}
