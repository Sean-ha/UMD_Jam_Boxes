using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnMouseDownEvent : MonoBehaviour
{
	public UnityEvent onMouseDown;

	public bool disabled { get; set; }

	private void OnMouseDown()
	{
		if (disabled)
			return;

		onMouseDown.Invoke();
	}

	public void PlaySound(int soundId)
	{
		SoundManager.instance.PlaySound((SoundManager.Sound)soundId);
	}
}
