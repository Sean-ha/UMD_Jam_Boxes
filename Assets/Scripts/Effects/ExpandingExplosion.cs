using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExpandingExplosion : MonoBehaviour
{
	public Transform explosionMask;

	private float radiusOffset = 0.1f;

	private List<Tween> ongoingTweens = new List<Tween>();

	public void SetExplosion(Color color, float radius, float explosionDuration = 0.15f)
	{
		// Cancel all previous tweens, and set initial values (this is a pooled object)
		foreach (Tween t in ongoingTweens)
			t.Kill();

		explosionMask.localScale = new Vector3(0f, 0f, 1f);

		GetComponent<SpriteRenderer>().color = color;
		transform.localScale = new Vector3(radius, radius, 1f);

		ongoingTweens.Add(transform.DOScale(new Vector3(radius + radiusOffset, radius + radiusOffset, 1f), explosionDuration));
		ongoingTweens.Add(explosionMask.DOScale(new Vector3(1f, 1f, 1f), explosionDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
		{
			Destroy(gameObject);
		}));
	}
}
