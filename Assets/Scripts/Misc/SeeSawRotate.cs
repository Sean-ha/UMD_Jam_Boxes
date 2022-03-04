using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SeeSawRotate : MonoBehaviour
{
	public Transform toRotate;
	public float rotateAmount;
	public float rotateTime;

	private Tween currTween;

	

	private void Start()
	{
		StartRotate();
	}


	public void StartRotate()
	{
		currTween = toRotate.DORotate(new Vector3(0, 0, rotateAmount), rotateTime).SetEase(Ease.InOutQuad).OnComplete(() => StartRotateBehavior());
	}

	private void StartRotateBehavior()
	{
		currTween = toRotate.DORotate(new Vector3(0, 0, -rotateAmount), 2 * rotateTime).SetEase(Ease.InOutQuad).OnComplete(() =>
		{
			currTween = toRotate.DORotate(new Vector3(0, 0, rotateAmount), 2 * rotateTime).SetEase(Ease.InOutQuad).OnComplete(() => StartRotateBehavior());
		});
	}
}
