using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
	public GameObject attackObject;
	public float hitForce;

	private float distanceFromPlayer = 1.5f;

	public void InitiateAttack()
	{
		Vector2 mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		Vector2 direction = HelperFunctions.GetDirectionTowards(transform.position, mousePos);
		GameObject swing = Instantiate(attackObject, (Vector2)transform.position + (direction * distanceFromPlayer), Quaternion.identity);

		swing.GetComponent<Swing>().InitiateSwing(transform, hitForce, direction);
	}
}
