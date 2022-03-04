using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSpawnZone : MonoBehaviour
{
	public List<Transform> doorTransforms;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// Disable door spawning here
			foreach (Transform t in doorTransforms)
			{
				EnemySpawner.instance.RemoveDoor(t);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// Re-enable door spawning here
			foreach (Transform t in doorTransforms)
			{
				EnemySpawner.instance.AddDoor(t);
			}
		}
	}
}
