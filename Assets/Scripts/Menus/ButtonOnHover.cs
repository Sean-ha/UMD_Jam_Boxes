using UnityEngine;
using DG.Tweening;

public class ButtonOnHover : MonoBehaviour
{
	public Transform toRotate;

	private Tween currTween;

	private float rotateTime = 0.3f;

	public bool disabled { get; set; }

	private void OnMouseEnter()
	{
		if (disabled)
			return;

		currTween = toRotate.DORotate(new Vector3(0, 0, 10f), rotateTime).SetEase(Ease.InOutQuad).OnComplete(() => StartRotateBehavior());
		SoundManager.instance.PlaySound(SoundManager.Sound.ButtonHover);
	}

	private void StartRotateBehavior()
	{
		currTween = toRotate.DORotate(new Vector3(0, 0, -10f), 2 * rotateTime).SetEase(Ease.InOutQuad).OnComplete(() =>
		{
			currTween = toRotate.DORotate(new Vector3(0, 0, 10f), 2 * rotateTime).SetEase(Ease.InOutQuad).OnComplete(() => StartRotateBehavior());
		});
	}

	public void ResetButton()
	{
		currTween.Kill();
		toRotate.rotation = Quaternion.identity;
	}

	private void OnMouseExit()
	{
		ResetButton();
	}
}
