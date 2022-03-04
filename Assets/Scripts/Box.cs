using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Box : MonoBehaviour
{
	public Transform mySprite;
	public ParticleSystem boxHitParticles;

	private Rigidbody2D rb;

	// The grapple that's hooked onto this box, or null if it's not hooked
	private Grapple grapple;
	private Tween grappleTween;

	private int squisherLayer;
	private RaycastHit2D[] hits = new RaycastHit2D[1];

	private int currentStreak;
	private bool isStreaking;
	private float minStreakDuration;

	private Vector2 startingPos;

	// If true, box will not kill anything more
	public bool dead { get; set; }

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		squisherLayer = LayerMask.GetMask("Wall", "Box", "Enemy");
		startingPos = transform.position;

		gameObject.SetActive(false);
	}

	private void Update()
	{
		if (isStreaking)
		{
			if (minStreakDuration <= 0)
			{
				if (rb.velocity.magnitude <= 1.5f)
				{
					StopStreaking();
				}
			}
			else
			{
				minStreakDuration -= Time.deltaTime;
			}
		}
	}

	public void PushBox(Vector2 point, Vector2 direction, float force)
	{
		SetStreaking();

		mySprite.GetComponent<FlashWhite>().InitiateWhiteFlash();
		if (grapple != null)
		{
			grapple.KillBoxGrapple();
			grapple = null;
			grappleTween.Kill();
		}

		rb.velocity = direction * 1.1f;
		rb.AddForceAtPosition(direction * force, point);

		CameraShaker.instance.ShakeCamera(0.2f, 0.2f);
	}

	// Grapple is the line that's pulling this box
	public void PullBox(Grapple grapple, Vector2 destination, float speed, float time)
	{
		if (IsBeingGrappled())
			return;

		SetStreaking();
		this.grapple = grapple;
		rb.velocity = Vector2.zero;

		Vector2 endVelocity = HelperFunctions.GetDirectionTowards(transform.position, destination) * speed;

		grappleTween = DOTween.To(() => Vector2.zero, (Vector2 val) => rb.velocity = val, endVelocity, time)
			.SetUpdate(UpdateType.Fixed).SetEase(Ease.InOutExpo).OnComplete(() =>
		{
			this.grapple = null;
		});
	}

	private void SetStreaking()
	{
		isStreaking = true;
		minStreakDuration = 0.25f;
		currentStreak = 0;
	}

	private void StopStreaking()
	{
		isStreaking = false;
		minStreakDuration = 0f;
		currentStreak = 0;
	}

	public bool IsBeingGrappled()
	{
		return grapple != null;
	}

	public void SetupBox()
	{
		transform.position = startingPos;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (rb.velocity.magnitude < 1.5f)
			return;

		int collLayer = collision.gameObject.layer;
		if (collLayer == LayerMask.NameToLayer("Enemy") || collLayer == LayerMask.NameToLayer("Wall") || collLayer == LayerMask.NameToLayer("Box"))
		{
			SoundManager.instance.PlaySound(SoundManager.Sound.BoxTouchEnemy);
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		// Speed threshold of squishing stuff
		if (rb.velocity.magnitude < 1.5f || dead)
			return;

		if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			Vector2 direction = HelperFunctions.GetDirectionTowards(transform.position, collision.transform.position);
			float rangle = HelperFunctions.GetRAngleTowardsPositive(transform.position, collision.transform.position);

			if (collision.collider.Raycast(direction, hits, collision.gameObject.GetComponent<Enemy>().GetDistanceToEdge(rangle) + 0.2f, squisherLayer) != 0)
			{
				// Create score
				Vector2 topOfEnemy = (Vector2)collision.transform.position + new Vector2(0, collision.gameObject.GetComponent<BoxCollider2D>().size.y);
				currentStreak++;
				int thisStreakCount = ScoreManager.instance.CreateScoreText(currentStreak, topOfEnemy);

				SoundManager.instance.PlaySoundPitch(SoundManager.Sound.EnemyDeath, 0.6f + thisStreakCount * 0.06f);
				StatsManager.instance.totalEnemiesKilled++;
				StatsManager.instance.CheckLargestKillStreak(currentStreak);

				collision.gameObject.GetComponent<Enemy>().KillThisEnemy();
			}

			// Vector2 destination = (Vector2)collision.transform.position + (direction * (collision.gameObject.GetComponent<Enemy>().GetDistanceToEdge(rangle) + 0.2f));
			// Debug.DrawLine(collision.transform.position, destination, Color.green);
		}
	}
}
