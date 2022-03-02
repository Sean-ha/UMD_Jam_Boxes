using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions
{
	// Returns the angle in degrees from startPos to endPos
	public static float GetDAngleTowards(Vector2 startPos, Vector2 endPos)
	{
		Vector2 diff = endPos - startPos;

		return Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
	}

	// Returns the angle in radians from startPos to endPos
	// Range: [-pi, pi]
	public static float GetRAngleTowards(Vector2 startPos, Vector2 endPos)
	{
		Vector2 diff = endPos - startPos;

		return Mathf.Atan2(diff.y, diff.x);
	}

	// Returns the angle in radians from startPos to endPos
	// Range: [0, 2pi]
	public static float GetRAngleTowardsPositive(Vector2 startPos, Vector2 endPos)
	{
		Vector2 diff = endPos - startPos;

		float rangle = Mathf.Atan2(diff.y, diff.x);
		return (rangle + (2 * Mathf.PI)) % (2 * Mathf.PI);
	}

	// Return a normalized vector from the startPos pointing towards endPos
	public static Vector2 GetDirectionTowards(Vector2 startPos, Vector2 endPos)
	{
		float rangle = GetRAngleTowards(startPos, endPos);

		return (new Vector2(Mathf.Cos(rangle), Mathf.Sin(rangle))).normalized;
	}
}
