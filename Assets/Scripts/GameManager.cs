using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public HealthSystem healthSystem;
	public EnemySpawner enemySpawner;
	public PlayerController pc;
	public Box startingBox;

	public FilledBG filler;
	public GameObject pauseBG;
	public GameObject deathScreenTexts;
	public GameObject enemiesSlain, enemiesSlainNum, biggestStreak, biggestStreakNum, timeLived, timeLivedNum, totalScore, totalScoreNum, newBestText, backBtn;

	public GameObject mainMenuHolder;
	public GameObject optionsHolder;

	public GameObject highScoreTitle;
	public TextMeshPro highScoreText;

	public GameObject titleObject;
	public GameObject playButton;
	public GameObject optionsButton;
	public GameObject quitButton;

	private void Awake()
	{
		instance = this;
		Application.targetFrameRate = 60;
	}

	private void Start()
	{
		TimeManager.instance.canPause = false;
		DisableAllMainMenu();

		StartCoroutine(ShowMainMenu());
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			Time.timeScale = 7f;
		}
		else if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			Time.timeScale = 1f;
		}
#endif
	}

	private void DisableAllMainMenu()
	{
		foreach (Transform child in mainMenuHolder.transform)
		{
			child.gameObject.SetActive(false);
		}
		mainMenuHolder.SetActive(false);
		mainMenuHolder.transform.position = Vector2.zero;
	}

	public void QuitGame()
	{
#if !UNITY_EDITOR
		Application.Quit();
#endif
	}

	public void OptionsButton()
	{
		float fillTime = filler.StartFill();

		DOVirtual.DelayedCall(fillTime + 0.9f, () =>
		{
			optionsHolder.transform.position = new Vector2(0f, 17f);
			optionsHolder.SetActive(true);
			optionsHolder.transform.DOMoveY(0f, 1.0f).SetEase(Ease.InOutQuad);
		});
	}

	public void CloseOptions()
	{
		optionsHolder.transform.DOMoveY(17f, 1.0f).SetEase(Ease.InOutQuad).OnComplete(() =>
		{
			optionsHolder.SetActive(false);
			filler.UnFill();
		});
		DisableAllMainMenuButtons(false);
	}

	// Pass in false to enable them all
	public void DisableAllMainMenuButtons(bool disable)
	{
		playButton.GetComponent<OnMouseDownEvent>().disabled = disable;
		optionsButton.GetComponent<OnMouseDownEvent>().disabled = disable;
		quitButton.GetComponent<OnMouseDownEvent>().disabled = disable;

		ButtonOnHover playOnHover = playButton.GetComponent<ButtonOnHover>();
		ButtonOnHover optionsOnHover = playButton.GetComponent<ButtonOnHover>();
		ButtonOnHover quitOnHover = playButton.GetComponent<ButtonOnHover>();
		playOnHover.disabled = disable;
		optionsOnHover.disabled = disable;
		quitOnHover.disabled = disable;

		playOnHover.ResetButton();
		playOnHover.ResetButton();
		playOnHover.ResetButton();
	}

	public void InitiateDeath()
	{
		TimeManager.instance.canPause = false;
		StatsManager.instance.StopTrackingTime();
		enemySpawner.GameOver();
		SoundManager.instance.FadeOutBgm(3f);
		startingBox.dead = true;

		StartCoroutine(DeathCR());
	}

	private IEnumerator DeathCR()
	{
		bool newBest = ScoreManager.instance.OnDeath();

		CameraShaker.instance.ShakeCameraLong(0.8f, 1f);
		yield return new WaitForSeconds(6f);

		float timeToFill = filler.StartFill();

		yield return new WaitForSeconds(timeToFill + 1f);

		deathScreenTexts.SetActive(true);
		float timeBetweenTexts = 0.5f;

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
		if (newBest)
			newBestText.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		yield return new WaitForSeconds(timeBetweenTexts);


		// Back button
		backBtn.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
	}

	public void ExitDeathScreen()
	{
		StartCoroutine(ExitDeathScreenBehavior());
	}

	private IEnumerator ExitDeathScreenBehavior()
	{
		float timeToExit = 0.55f;
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

		yield return new WaitForSeconds(timeToExit + 0.6f);

		timeToExit = filler.UnFill();

		yield return new WaitForSeconds(timeToExit + 1.5f);

		// Kill all enemies
		Enemy[] aliveEnemies = FindObjectsOfType<Enemy>();
		foreach (Enemy e in aliveEnemies)
		{
			e.KillThisEnemy();
		}
		// Disable startingBox
		startingBox.gameObject.SetActive(false);
		ObjectCreator.instance.CreateExpandingExplosion(startingBox.transform.position, Quaternion.identity, Constants.lightColor, 1f, large: true);
		ObjectCreator.instance.CreateObject(Tag.PlayerDeathParticles, startingBox.transform.position, Quaternion.identity);

		SoundManager.instance.PlaySound(SoundManager.Sound.EnemyDeath);

		yield return new WaitForSeconds(0.75f);

		StartCoroutine(ShowMainMenu());
	}

	public IEnumerator ShowMainMenu()
	{
		DisableAllMainMenuButtons(true);
		mainMenuHolder.SetActive(true);

		// Set high score:
		int overallHighScore = PlayerPrefs.GetInt("HighScore", 0);
		highScoreText.text = overallHighScore.ToString("D8");

		yield return new WaitForSeconds(0.75f);

		SoundManager.instance.PlayMenuTheme();

		titleObject.transform.position = new Vector3(0f, 12.5f, 0f);
		titleObject.SetActive(true);
		titleObject.transform.DOMoveY(3f, 0.8f).SetEase(Ease.OutBack);

		yield return new WaitForSeconds(1.6f);

		playButton.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		yield return new WaitForSeconds(0.4f);
		optionsButton.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		yield return new WaitForSeconds(0.4f);
		quitButton.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		yield return new WaitForSeconds(0.4f);
		highScoreTitle.SetActive(true);
		highScoreText.gameObject.SetActive(true);
		SoundManager.instance.PlaySound(SoundManager.Sound.PauseClick);
		DisableAllMainMenuButtons(false);
	}

	public void PressPlayGame()
	{
		DOVirtual.DelayedCall(0.75f, () =>
		{
			mainMenuHolder.transform.DOMoveY(17f, 1f).SetEase(Ease.InOutQuad).OnComplete(() =>
			{
				DOVirtual.DelayedCall(2f, () =>
				{
					SetupAndStartGame();
				});
				DisableAllMainMenu();
				SoundManager.instance.FadeOutBgm(2.0f);
			});
		});
	}

	public void SetupAndStartGame()
	{
		TimeManager.instance.canPause = true;
		healthSystem.OnStartGame();

		// Setup player
		pc.SetupPlayer();
		ObjectCreator.instance.CreateExpandingExplosion(pc.transform.position, Quaternion.identity, Constants.lightColor, 1f, explosionDuration: 0.2f, large: true);
		ObjectCreator.instance.CreateObject(Tag.PlayerDeathParticles, pc.transform.position, Quaternion.identity);
		pc.gameObject.SetActive(true);

		// Setup starting box
		startingBox.SetupBox();
		ObjectCreator.instance.CreateExpandingExplosion(startingBox.transform.position, Quaternion.identity, Constants.lightColor, 1f, explosionDuration: 0.2f, large: true);
		ObjectCreator.instance.CreateObject(Tag.PlayerDeathParticles, startingBox.transform.position, Quaternion.identity);
		startingBox.gameObject.SetActive(true);

		SoundManager.instance.PlaySound(SoundManager.Sound.Explosion);

		// Setup other necessary things
		TimeManager.instance.canPause = true;
		StatsManager.instance.ResetStats();
		StatsManager.instance.StartTrackingTime();
		ScoreManager.instance.ResetScore();
		startingBox.dead = false;

		DOVirtual.DelayedCall(0.75f, () => SoundManager.instance.PlayBattleTheme());

		enemySpawner.InitiateSpawning();
	}
}
