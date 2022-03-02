using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles player health and the UI for it. Also handles player collision with enemies
public class HealthSystem : MonoBehaviour
{
	public Sprite filledHeart, emptyHeart;
	public SpriteRenderer mySpriteRenderer;
	public List<SpriteRenderer> hearts;
	[Tooltip("Duration the player will be invincible after taking damage (seconds)")]
	public float invincibilityTime;

	private bool isInvincible;

	private int maxHealth;
	// currentHealth-1 gives index in hearts list of current heart
	private int currentHealth;


	private void Start()
	{
		maxHealth = hearts.Count;
		currentHealth = maxHealth;
	}

	// Decrease currentHealth by 1 and update UI
	public void DecreaseHealth()
	{
		EmptyCurrentHeart();
		currentHealth--;

		if (currentHealth == 0)
		{
			// Die
			TimeManager.instance.canPause = false;
			// SoundManager.instance.PlaySound(SoundManager.Sound.PlayerDeath);
			GetComponent<PlayerController>().OnDeath();

			GameManager.instance.InitiateDeath();
		}
	}

	// Change current heart to an empty one
	public void EmptyCurrentHeart()
	{
		hearts[currentHealth - 1].sprite = emptyHeart;
	}

	// Change current heart to a filled one
	public void FillCurrentHeart()
	{
		hearts[currentHealth - 1].sprite = filledHeart;
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			if (currentHealth == 0)
				return;

			if (isInvincible)
				return;

			// Take damage

			DecreaseHealth();

			// Make player invincible for a short time
			StartCoroutine(MakeInvincible(invincibilityTime));
			StartCoroutine(BlinkSprite(invincibilityTime));

			CameraShaker.instance.ShakeCamera(0.4f, 0.5f);

			SoundManager.instance.PlaySound(SoundManager.Sound.PlayerGetHit);
		}
	}

	// Make the player invincible for time
	private IEnumerator MakeInvincible(float time)
	{
		isInvincible = true;

		yield return new WaitForSeconds(time);

		isInvincible = false;
	}

	// Blink the player to show invincible duration
	private IEnumerator BlinkSprite(float time)
	{
		// Time that sprite remains off / on during blinking
		float blinkTime = 0.06f;

		int count = Mathf.RoundToInt(time / (blinkTime * 2));
		for (int i = 0; i < count; i++)
		{
			mySpriteRenderer.enabled = false;
			yield return new WaitForSeconds(blinkTime);
			mySpriteRenderer.enabled = true;
			yield return new WaitForSeconds(blinkTime);
		}
	}
}
