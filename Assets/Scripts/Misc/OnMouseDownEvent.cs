using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnMouseDownEvent : MonoBehaviour
{
	public UnityEvent onMouseDown;

	private void OnMouseDown()
	{
		onMouseDown.Invoke();
	}

	public void PlaySound(int soundId)
	{
		SoundManager.instance.PlaySound((SoundManager.Sound)soundId);
	}
}
