using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
	private Rigidbody2D rb;

	private int squisherLayer;
	private RaycastHit2D[] hits = new RaycastHit2D[1];

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		squisherLayer = LayerMask.GetMask("Wall", "Box", "Enemy");
	}

	public void PushBox(Vector2 point, Vector2 direction, float force)
	{
		rb.AddForceAtPosition(direction * force, point);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			Vector2 direction = HelperFunctions.GetDirectionTowards(transform.position, collision.transform.position);
			if (collision.collider.Raycast(direction, hits, collision.gameObject.GetComponent<Enemy>().size / 2 + 0.3f, squisherLayer) != 0)
			{
				Destroy(collision.gameObject);
			}
		}
	}
}
