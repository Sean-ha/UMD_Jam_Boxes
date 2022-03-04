using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;

	public TextMeshPro totalScoreText;
	public int scorePerStreak;
	public int maxStreakPerHit;

	public GameObject scoreText;

	private Transform canvas;

	public int pointAmount { get; set; }

	// If true, can't get any more points
	private bool pointsLocked;

	private void Awake()
	{
		instance = this;

		canvas = scoreText.transform.parent;

		// Move away unnecessary placeholder
		Vector2 tempPos = scoreText.transform.position;
		tempPos.x += 100f;
		tempPos.y += 100f;
		scoreText.transform.position = tempPos;
	}

	private void Start()
	{
		UpdateTotalScoreUI();
	}

	public void GainPoints(int amount)
	{
		if (pointsLocked)
			return;

		pointAmount += amount;

		UpdateTotalScoreUI();
	}

	// Also adds the score to total
	// Returns the streak number (max 15)
	public int CreateScoreText(int streakNumber, Vector2 position)
	{
		streakNumber = Mathf.Min(streakNumber, maxStreakPerHit);
		int pointAmount = streakNumber * scorePerStreak;

		GameObject scoreObject = Instantiate(scoreText, canvas, true);
		TextMeshProUGUI scoreObjectText = scoreObject.GetComponent<TextMeshProUGUI>();

		scoreObject.transform.position = position;
		scoreObjectText.text = $"+{pointAmount}";

		scoreObject.transform.DOMoveY(scoreObject.transform.position.y + 0.5f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
		{
			// StartCoroutine(BlinkScoreText(scoreObjectText));
			scoreObject.transform.DOScaleY(0f, 0.5f).SetEase(Ease.InOutQuad).OnComplete(() =>
			{
				Destroy(scoreObject);
			});
		});

		GainPoints(pointAmount);

		return streakNumber;
	}

	private IEnumerator BlinkScoreText(TextMeshProUGUI toBlink)
	{
		int blinkCount = 2;
		for (int i = 0; i < blinkCount; i++)
		{
			toBlink.enabled = false;
			yield return new WaitForSeconds(0.08f);
			toBlink.enabled = true;
			yield return new WaitForSeconds(0.08f);
		}
		toBlink.enabled = false;

		Destroy(toBlink.gameObject);
	}

	private void UpdateTotalScoreUI()
	{
		totalScoreText.text = pointAmount.ToString("D8");
	}

	public void ResetScore()
	{
		pointsLocked = false;
		pointAmount = 0;
		UpdateTotalScoreUI();
	}

	// Returns whether this was a new best high score or not
	public bool OnDeath()
	{
		pointsLocked = true;
		int overallHighScore = PlayerPrefs.GetInt("HighScore", 0);
		if (pointAmount > overallHighScore)
		{
			PlayerPrefs.SetInt("HighScore", pointAmount);
			return true;
		}
		return false;
	}
}
