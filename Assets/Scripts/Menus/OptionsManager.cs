using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsManager : MonoBehaviour
{
	public TextMeshPro sfxVolumeText;
	public TextMeshPro bgmVolumeText;

	private float sfxVolume = 0.4f;
	private float bgmVolume = 0.4f;

	private void Start()
	{
		SoundManager.instance.ChangeSFXVolume(sfxVolume);
		SoundManager.instance.ChangeBGMVolume(bgmVolume);

		UpdateBgmUI();
		UpdateSfxUI();
	}

	public void IncrementSfx(float delta)
	{
		sfxVolume = Mathf.Clamp(sfxVolume + delta, 0f, 1f);

		SoundManager.instance.ChangeSFXVolume(sfxVolume);
		UpdateSfxUI();
	}

	public void IncrementBgm(float delta)
	{
		bgmVolume = Mathf.Clamp(bgmVolume + delta, 0f, 1f);

		SoundManager.instance.ChangeBGMVolume(bgmVolume);
		UpdateBgmUI();
	}

	private void UpdateSfxUI()
	{
		sfxVolumeText.text = Mathf.RoundToInt(100 * sfxVolume) + "%";
	}

	private void UpdateBgmUI()
	{
		bgmVolumeText.text = Mathf.RoundToInt(100 * bgmVolume) + "%";
	}
}
