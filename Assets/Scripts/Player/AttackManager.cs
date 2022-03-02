using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
	public GameObject attackObject;
	public float boxHitForce;
	public float enemyHitForce;
	public float attackCooldown;

	private float currAttackCooldown;

	private float distanceFromPlayer = 1.8f;

	// Every other swing is flipped over y-axis
	private bool flipSwing;

	private void Update()
	{
		if (currAttackCooldown > 0)
			currAttackCooldown -= Time.deltaTime;
	}

	public void InitiateAttack()
	{
		if (currAttackCooldown > 0)
		{
			return;
		}

		currAttackCooldown = attackCooldown;

		Vector2 mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		float dangle = HelperFunctions.GetDAngleTowards(transform.position, mousePos);
		Vector2 direction = HelperFunctions.GetDirectionTowards(transform.position, mousePos);

		GameObject swing = Instantiate(attackObject, (Vector2)transform.position + (direction * distanceFromPlayer), 
			Quaternion.Euler(new Vector3(0, 0, dangle)));
		swing.transform.parent = transform;

		swing.GetComponent<Swing>().InitiateSwing(transform, boxHitForce, enemyHitForce, direction, flipSwing);
		flipSwing = !flipSwing;

		SoundManager.instance.PlaySound(SoundManager.Sound.PlayerSwing);
	}
}
