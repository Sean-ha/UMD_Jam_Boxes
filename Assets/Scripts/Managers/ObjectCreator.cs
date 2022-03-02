using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
	public static ObjectCreator instance;

	[System.Serializable]
	private struct TagToPrefab {
		public Tag tag;
		public GameObject obj;
	}

	[SerializeField]
	private List<TagToPrefab> objectsList;

	private Dictionary<Tag, GameObject> objectsDict = new Dictionary<Tag, GameObject>();

	private void Awake()
	{
		instance = this;

		// Build Dictionary using the public List (this is a hacky solution to no Dictionary serializing in Unity editor)
		foreach (TagToPrefab ttp in objectsList)
		{
			objectsDict.Add(ttp.tag, ttp.obj);
		}
	}

	public GameObject CreateObject(Tag tag, Vector2 position, Quaternion rotation)
	{
		GameObject created = Instantiate(objectsDict[tag], position, rotation);

		return created;
	}

	public GameObject CreateExpandingExplosion(Vector2 position, Quaternion rotation, Color color, float radius, float explosionDuration = 0.15f, bool large = false)
	{
		GameObject created;
		if (large)
			created = CreateObject(Tag.ExpandingExplosionLarge, position, rotation);
		else
			created = CreateObject(Tag.ExpandingExplosion, position, rotation);

		created.GetComponent<ExpandingExplosion>().SetExplosion(color, radius, explosionDuration);

		return created;
	}
}

public enum Tag
{
	ExpandingExplosion = 0,
	EnemyDeathParticles = 1,
	ExpandingExplosionLarge = 2,
	PlayerDeathParticles = 3,
}
