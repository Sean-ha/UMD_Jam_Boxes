using UnityEngine;
using DG.Tweening;

public class FilledBGFiller : MonoBehaviour
{
	private LineRenderer lr;

	private float fillTime = 0.85f;

	private Tween myTween;

	private void Awake()
	{
		lr = GetComponent<LineRenderer>();
	}

	public void StartLeftToRight()
	{
		lr.SetPosition(0, new Vector3(-15f, 0, 0));
		lr.SetPosition(1, new Vector3(-15f, 0, 0));
		myTween = DOTween.To(() => -15, (float val) => lr.SetPosition(1, new Vector3(val, 0, 0)), 15f, fillTime).SetEase(Ease.InOutCubic).SetAutoKill(false);
	}

	public void StartRightToLeft()
	{
		lr.SetPosition(0, new Vector3(15f, 0, 0));
		lr.SetPosition(1, new Vector3(15f, 0, 0));
		myTween = DOTween.To(() => 15, (float val) => lr.SetPosition(1, new Vector3(val, 0, 0)), -15f, fillTime).SetEase(Ease.InOutCubic).SetAutoKill(false);
	}

	public void PlayBackwards()
	{
		myTween.OnRewind(() => Destroy(gameObject)).PlayBackwards();
	}
}
