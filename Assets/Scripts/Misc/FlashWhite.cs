using System.Collections;
using UnityEngine;

// Script for making sprite flash white. Sprite requires DropShadow... material
public class FlashWhite : MonoBehaviour
{
	private Renderer rend;
	private MaterialPropertyBlock block;
	private SpriteRenderer sr;

	private Coroutine stopFlashCR;
	private Coroutine spriteRendererFlashCR;

	private void Awake()
	{
		rend = GetComponent<Renderer>();
		block = new MaterialPropertyBlock();
		sr = GetComponent<SpriteRenderer>();
	}

	private void OnEnable()
	{
		rend.GetPropertyBlock(block);
		block.SetFloat("_FlashAmount", 0f);
		rend.SetPropertyBlock(block);
	}

	// Default time
	public void InitiateWhiteFlash()
	{
		InitiateWhiteFlash(0.1f);
	}

	/// <summary>
	/// Sprite turns white for given amount of time before turning back to normal
	/// </summary>
	/// <param name="whiteTime"></param>
	public void InitiateWhiteFlash(float whiteTime = 0.1f)
	{
		if (stopFlashCR != null)
			StopCoroutine(stopFlashCR);

		rend.GetPropertyBlock(block);
		block.SetFloat("_FlashAmount", 1f);
		rend.SetPropertyBlock(block);

		if (gameObject.activeInHierarchy)
			stopFlashCR = StartCoroutine(StopFlash(whiteTime));
	}

	private IEnumerator StopFlash(float whiteTime)
	{
		yield return new WaitForSeconds(whiteTime);

		rend.GetPropertyBlock(block);
		block.SetFloat("_FlashAmount", 0f);
		rend.SetPropertyBlock(block);
	}

	public void InitiateWhiteFlashAllChildren()
	{
		InitiateWhiteFlashAllChildren(0.1f);
	}

	// Initiates the white flash for this object and all of its children
	public void InitiateWhiteFlashAllChildren(float whiteTime = 0.1f)
	{
		InitiateWhiteFlash(whiteTime);

		foreach (Transform child in transform)
		{
			FlashWhite flasher = child.GetComponent<FlashWhite>();
			if (flasher != null)
			{
				flasher.InitiateWhiteFlash(whiteTime);
			}
		}
	}
}