using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleManager : MonoBehaviour
{
	public GameObject grappleObject;
	public float boxPullSpeed;
	public float debugVal;
	public float playerPullSpeed;

	private PlayerController pc;

	private ContactFilter2D filter;
	// Used in Raycast
	private RaycastHit2D[] results = new RaycastHit2D[2];

	// Total number of box grapples you have
	private int boxGrapples = 1;

	private int currGrapples = 1;


	private void Awake()
	{
		filter.SetLayerMask(LayerMask.GetMask("Box", "Wall"));
		pc = GetComponent<PlayerController>();
	}

	public void InitiateGrapple()
	{
		Vector2 mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		Vector2 direction = HelperFunctions.GetDirectionTowards(transform.position, mousePos);

		int count = Physics2D.Raycast(transform.position, direction, filter, results, 50f);
		if (count == 0)
			return;

		// Check if hit box
		Box box = results[0].transform.GetComponent<Box>();
		if (box != null)
		{
			if (currGrapples > 0)
			{
				// Create grapple onto box
				GameObject createdGrapple = Instantiate(grappleObject, transform.position, Quaternion.identity);
				createdGrapple.GetComponent<Grapple>().StartBoxGrapple(this, box, transform.position, boxPullSpeed, debugVal);
				currGrapples--;
			}
		}
		else if (results[0].transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
		{
			Vector2 wallPos = results[0].point;

			GameObject createdGrapple = Instantiate(grappleObject, transform.position, Quaternion.identity);
			createdGrapple.GetComponent<Grapple>().StartWallGrapple(pc, wallPos, playerPullSpeed);
		}


		/*
		// TODO: Find closest box
		int count = Physics2D.OverlapCircle(mousePos, 2f, filter, results);
		if (count == 0)
			return;
		*/
	}

	// Called when box grapple breaks. Get back another grapple
	public void GainGrapple()
	{
		currGrapples++;
	}
}
