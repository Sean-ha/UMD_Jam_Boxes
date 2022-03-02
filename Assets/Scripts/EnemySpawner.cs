using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public List<Transform> enemySpots;

	public float spawnTime;
	public GameObject ghostEnemy;

	public void InitiateSpawning()
	{
		StartCoroutine(SpawnEnemy());
	}


	public IEnumerator SpawnEnemy()
	{
		while(true)
		{
			int rand = Random.Range(0, enemySpots.Count);

			Instantiate(ghostEnemy, enemySpots[rand].position, Quaternion.identity);

			yield return new WaitForSeconds(spawnTime);
		}
	}
}
