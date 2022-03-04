using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BobUpDown : MonoBehaviour
{
	public float bobAmount = 0.2f;
	public float bobDuration = 1f;

	private void Start()
	{
		transform.DOLocalMoveY(transform.localPosition.y - bobAmount / 2, bobDuration / 2).SetEase(Ease.InOutQuad).OnComplete(() =>
		{
			BobBehavior();
		});
	}

	private void BobBehavior()
	{
		transform.DOLocalMoveY(transform.localPosition.y + bobAmount, bobDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
		{
			transform.DOLocalMoveY(transform.localPosition.y - bobAmount, bobDuration).SetEase(Ease.InOutQuad).OnComplete(() => BobBehavior());
		});
	}
}
