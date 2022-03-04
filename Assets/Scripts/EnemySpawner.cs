using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public static EnemySpawner instance;

	public List<Transform> enemySpots;

	public float startingSpawnTime;
	public GameObject ghostEnemy;
	public GameObject batEnemy;
	public GameObject goblinEnemy;

	private Coroutine spawnCR;
	private Coroutine managerCR;

	private float currSpawnTime;
	// Transform is position. If value = 1, then it is spawnable. Else it is not
	private Dictionary<Transform, int> availableSpotDict = new Dictionary<Transform, int>();
	private HashSet<Transform> availableSpots = new HashSet<Transform>();
	private List<GameObject> availableEnemies = new List<GameObject>();

	private int internalTimer;

	private void Awake()
	{
		instance = this;

		foreach (Transform t in enemySpots)
		{
			availableSpotDict.Add(t, 1);
			availableSpots.Add(t);
		}
	}

	public void InitiateSpawning()
	{
		availableEnemies = new List<GameObject>() { ghostEnemy };
		currSpawnTime = startingSpawnTime;
		internalTimer = 0;

		spawnCR = StartCoroutine(SpawnEnemy());
		managerCR = StartCoroutine(ManagerCR());
	}

	public void GameOver()
	{
		StopCoroutine(spawnCR);
		StopCoroutine(managerCR);
	}

	private IEnumerator SpawnEnemy()
	{
		while(true)
		{
			int rand = Random.Range(0, availableSpots.Count);
			GameObject enemy = availableEnemies[Random.Range(0, availableEnemies.Count)];

			Instantiate(enemy, availableSpots.ElementAt(rand).position, Quaternion.identity);

			yield return new WaitForSeconds(currSpawnTime);
		}
	}

	private IEnumerator ManagerCR()
	{
		internalTimer = 0;
		while (true)
		{
			yield return new WaitForSeconds(1);
			internalTimer += 1;

			// Every 10 seconds, speed up spawning a little bit
			if (internalTimer % 10 == 0)
			{
				currSpawnTime = Mathf.Max(0.1f, currSpawnTime - 0.05f);
			}

			if (internalTimer == 25)
			{
				availableEnemies.Add(batEnemy);
			}
			else if (internalTimer == 60)
			{
				availableEnemies.Add(goblinEnemy);
			}
			
		}
	}

	public void RemoveDoor(Transform door)
	{
		availableSpotDict[door]--;
		if (availableSpots.Contains(door))
			availableSpots.Remove(door);
	}

	public void AddDoor(Transform door)
	{
		int val = availableSpotDict[door] + 1;
		availableSpotDict[door] = val;
		if (val == 1)
		{
			availableSpots.Add(door);
		}
	}
}
