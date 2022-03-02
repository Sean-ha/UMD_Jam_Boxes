using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public EnemySpawner enemySpawner;
	public PlayerController pc;

	public GameObject pauseBG;
	public GameObject deathScreenTexts;
	public GameObject enemiesSlain, enemiesSlainNum, biggestStreak, biggestStreakNum, timeLived, timeLivedNum, totalScore, totalScoreNum, backBtn;

	public GameObject mainMenuHoldler;
	public GameObject titleObject;
	public GameObject playButton;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		Application.targetFrameRate = 60;
	}

	public void InitiateDeath()
	{
		StartCoroutine(DeathCR());
	}

	private IEnumerator DeathCR()
	{
		CameraShaker.instance.ShakeCameraLong(0.8f, 1f);
		yield return new WaitForSeconds(6f);

		pauseBG.transform.position = new Vector3(0f, 17f, 0f);
		pauseBG.SetActive(true);
		pauseBG.transform.DOMoveY(0f, 0.7f).SetEase(Ease.InOutQuad);

		yield return new WaitForSeconds(1.5f);

		deathScreenTexts.SetActive(true);
		float timeBetweenTexts = 0.65f;

		// Enemies slain
		enemiesSlain.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		yield return new WaitForSeconds(timeBetweenTexts);

		enemiesSlainNum.GetComponent<TextMeshPro>().text = StatsManager.instance.totalEnemiesKilled.ToString();
		enemiesSlainNum.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		yield return new WaitForSeconds(timeBetweenTexts);


		// Biggest streak
		biggestStreak.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		yield return new WaitForSeconds(timeBetweenTexts);

		biggestStreakNum.GetComponent<TextMeshPro>().text = StatsManager.instance.largestKillStreak.ToString();
		biggestStreakNum.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		yield return new WaitForSeconds(timeBetweenTexts);


		// Time lived
		timeLived.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		yield return new WaitForSeconds(timeBetweenTexts);

		TimeSpan time = TimeSpan.FromSeconds(StatsManager.instance.timeLived);
		timeLivedNum.GetComponent<TextMeshPro>().text = time.ToString(@"mm\:ss");
		timeLivedNum.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		yield return new WaitForSeconds(timeBetweenTexts);


		// Total points
		totalScore.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		yield return new WaitForSeconds(timeBetweenTexts);

		totalScoreNum.GetComponent<TextMeshPro>().text = ScoreManager.instance.pointAmount.ToString("D8");
		totalScoreNum.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		yield return new WaitForSeconds(timeBetweenTexts);


		// Back button
		backBtn.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
	}

	public void ExitDeathScreen()
	{
		float timeToExit = 0.55f;
		pauseBG.transform.DOMoveY(17f, timeToExit).SetEase(Ease.InOutQuad).OnComplete(() =>
		{
			// Reset this object
			pauseBG.SetActive(false);
			pauseBG.transform.position = Vector2.zero;
		});
		deathScreenTexts.transform.DOMoveY(17f, timeToExit).SetEase(Ease.InOutQuad).OnComplete(() =>
		{
			// Reset this object
			foreach (Transform child in deathScreenTexts.transform)
			{
				child.gameObject.SetActive(false);
			}
			deathScreenTexts.SetActive(false);
			deathScreenTexts.transform.position = Vector2.zero;
		});
	}

	public void PressPlayGame()
	{
		DOVirtual.DelayedCall(0.75f, () =>
		{
			mainMenuHoldler.transform.DOMoveY(17f, 1f).SetEase(Ease.InOutQuad).OnComplete(() =>
			{
				DOVirtual.DelayedCall(0.5f, () =>
				{
					SetupAndStartGame();
				});
			});
		});
	}

	public void SetupAndStartGame()
	{
		// TODO
		TimeManager.instance.canPause = true;

		pc.SetupPlayer();

		enemySpawner.InitiateSpawning();
	}
}
