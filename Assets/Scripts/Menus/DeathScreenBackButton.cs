using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreenBackButton : MonoBehaviour
{
	private void OnMouseDown()
	{
		GameManager.instance.ExitDeathScreen();
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
	}
}
