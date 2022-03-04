using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
	public Animator animator;
	public float movementSpeed;
	public Transform spriteTransform;
	public GameObject shadowObject;

	private Rigidbody2D rb;
	private AttackManager attackManager;
	private GrappleManager grappleManager;

	private bool canMove = true;
	// If player is grappling to wall
	private bool isGrapplingWall;
	private Tween wallGrappleTween;
	private Tween velocityOffsetTween;

	private float horizontal, vertical;
	private bool isFacingRight = true;

	// If disabled, player can't do anything
	private bool disabled;

	private Vector2 velocityOffset = Vector2.zero;

	private Vector2 startingPos;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		attackManager = GetComponent<AttackManager>();
		grappleManager = GetComponent<GrappleManager>();
		startingPos = transform.position;

		gameObject.SetActive(false);
	}

	private void Update()
	{
		if (disabled)
			return;

		if (canMove)
		{
			horizontal = Input.GetAxisRaw("Horizontal");
			vertical = Input.GetAxisRaw("Vertical");
			// Flip character depending on direction of movement
			if ((horizontal > 0 && !isFacingRight) || (horizontal < 0 && isFacingRight))
			{
				Vector2 newScale = spriteTransform.localScale;
				newScale.x = -newScale.x;
				spriteTransform.localScale = newScale;
				isFacingRight = !isFacingRight;
			}
		}
		else
		{
			horizontal = 0;
			vertical = 0;
		}

		if (Input.GetMouseButtonDown(0))
		{
			attackManager.InitiateAttack();
		}
		if (Input.GetMouseButtonDown(1))
		{
			grappleManager.InitiateGrapple();
		}

		if (isGrapplingWall)
		{
			if (Input.GetMouseButtonUp(1))
			{
				wallGrappleTween.Kill();
			}
		}

		animator.SetBool("isMoving", (horizontal != 0 || vertical != 0));
	}

	private void FixedUpdate()
	{
		Vector2 velVector = new Vector2(horizontal, vertical).normalized;
		velVector *= movementSpeed;
		velVector += velocityOffset;
		rb.velocity = velVector;
	}

	public void PullSelf(Grapple grapple, Vector2 destination, float speed, float time)
	{
		canMove = false;
		isGrapplingWall = true;

		rb.velocity = Vector2.zero;

		Vector2 endVelocity = HelperFunctions.GetDirectionTowards(transform.position, destination) * speed;

		wallGrappleTween = DOTween.To(() => Vector2.zero, (Vector2 val) => velocityOffset = val, endVelocity, time)
			.SetEase(Ease.OutQuad).OnKill(() =>
			{
				canMove = true;
				isGrapplingWall = false;
				// Begin decreasing velocityOffset
				grapple.KillWallGrapple();
				velocityOffsetTween.Kill();
				velocityOffsetTween = DOTween.To(() => velocityOffset, (Vector2 val) => velocityOffset = val, Vector2.zero, 1.2f);
			});
	}

	public void OnDeath()
	{
		disabled = true;
		rb.isKinematic = true;
		animator.SetBool("isMoving", false);
		rb.velocity = Vector2.zero;
		horizontal = vertical = 0;

		spriteTransform.DOShakePosition(3f, strength: 0.5f, vibrato: 15).OnComplete(() =>
		{
			DOVirtual.DelayedCall(0.6f, () =>
			{
				ObjectCreator.instance.CreateExpandingExplosion(transform.position, Quaternion.identity, Constants.lightColor, 1f, explosionDuration: 0.2f, large: true);
				ObjectCreator.instance.CreateObject(Tag.PlayerDeathParticles, transform.position, Quaternion.identity);
				spriteTransform.gameObject.SetActive(false);
				shadowObject.SetActive(false);

				SoundManager.instance.PlaySound(SoundManager.Sound.BalloonPop);
			});
			
		});
	}

	public void SetupPlayer()
	{
		transform.position = startingPos;
		disabled = false;
		rb.isKinematic = false;
		spriteTransform.gameObject.SetActive(true);
		shadowObject.SetActive(true);
	}
}
