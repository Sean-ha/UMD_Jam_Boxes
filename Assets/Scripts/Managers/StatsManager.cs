using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
	public static StatsManager instance;

	// Note these stats are only for the current run and are reset afterwards
	public int totalEnemiesKilled { get; set; }
	public int largestKillStreak { get; set; }
	public int timeLived { get; set; }  // In seconds

	private Coroutine timeTrackCR;

	private void Awake()
	{
		instance = this;
	}

	private IEnumerator TrackTimeLivedCR()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);
			timeLived++;
		}
	}

	public void ResetStats()
	{
		totalEnemiesKilled = 0;
		largestKillStreak = 0;
		timeLived = 0;
	}

	public void StartTrackingTime()
	{
		timeTrackCR = StartCoroutine(TrackTimeLivedCR());
	}

	public void StopTrackingTime()
	{
		StopCoroutine(timeTrackCR);
	}

	// Sets largestKillStreak if the provided streak is larger than the current largestKillStreak
	public void CheckLargestKillStreak(int toCheck)
	{
		if (toCheck > largestKillStreak)
			largestKillStreak = toCheck;
	}
}
