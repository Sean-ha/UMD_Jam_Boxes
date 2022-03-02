using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
	public static StatsManager instance;

	// Note these stats are only for the current run and are reset afterwards
	public int totalEnemiesKilled { get; set; }
	public int largestKillStreak { get; set; }
	public int timeLived { get; set; }	// In seconds


	private void Awake()
	{
		instance = this;

		StartCoroutine(TrackTimeLivedCR());
	}

	private IEnumerator TrackTimeLivedCR()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);
			timeLived++;
		}
	}

	// Sets largestKillStreak if the provided streak is larger than the current largestKillStreak
	public void CheckLargestKillStreak(int toCheck)
	{
		if (toCheck > largestKillStreak)
			largestKillStreak = toCheck;
	}
}
