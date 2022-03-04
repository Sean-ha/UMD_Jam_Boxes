using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OscillateAdjacentPlayer : MonoBehaviour
{
	public float force;
	public float duration;

	private PlayerController player;
	private Rigidbody2D rb;

	// Should next push be up or down?
	private bool up = true;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		player = FindObjectOfType<PlayerController>();

		StartCoroutine(OscillateBehavior());
	}

	private IEnumerator OscillateBehavior()
	{
		while (true)
		{
			PushUp();

			yield return new WaitForSeconds(2 * duration);

			PushDown();

			yield return new WaitForSeconds(2 * duration);
		}
	}

	private void PushUp()
	{
		Vector2 playerPos = player.transform.position;

		Vector2 dir = HelperFunctions.GetDirectionTowards(transform.position, playerPos);
		Vector2 perp = Vector2.Perpendicular(dir).normalized;
		DOTween.To(() => 0f, (float val) => rb.AddForce(perp * val), force, duration).SetUpdate(UpdateType.Fixed);
	}

	private void PushDown()
	{
		Vector2 playerPos = player.transform.position;

		Vector2 dir = HelperFunctions.GetDirectionTowards(transform.position, playerPos);
		Vector2 perp = -Vector2.Perpendicular(dir).normalized;
		DOTween.To(() => 0f, (float val) => rb.AddForce(perp * val), force, duration).SetUpdate(UpdateType.Fixed);
	}
}
