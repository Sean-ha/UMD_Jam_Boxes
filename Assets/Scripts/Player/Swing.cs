using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
	// Set when swing is created
	public Transform player { get; set; }

	private float boxSwingForce;
	private float enemyHitForce;
	private Vector2 direction;

	public void InitiateSwing(Transform player, float boxSwingForce, float enemyHitForce, Vector2 direction, bool isFlipped)
	{
		this.player = player;
		this.boxSwingForce = boxSwingForce;
		this.enemyHitForce = enemyHitForce;
		this.direction = direction;
		
		// Flip swing if necessary
		if (isFlipped)
		{
			Vector2 scale = transform.localScale;
			scale.y = -scale.y;
			transform.localScale = scale;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Box"))
		{
			Vector2 pointOnBox = collision.ClosestPoint(player.position);

			// Vector2 direction = HelperFunctions.GetDirectionTowards(player.position, pointOnBox);

			collision.GetComponent<Box>().PushBox(pointOnBox, direction, boxSwingForce);
			SoundManager.instance.PlaySound(SoundManager.Sound.BoxHit, volumeDelta: 0.1f);
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			// Smack enemy, knock it back
			Vector2 direction = HelperFunctions.GetDirectionTowards(player.position, collision.transform.position);

			Enemy enemy = collision.GetComponent<Enemy>();
			enemy.PushBackWithForce(direction * enemyHitForce);
			enemy.InitiateWhiteFlash();

			SoundManager.instance.PlaySound(SoundManager.Sound.SwingHitEnemy);
		}
	}
}
