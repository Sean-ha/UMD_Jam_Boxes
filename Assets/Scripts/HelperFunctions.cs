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
	public static float GetRAngleTowards(Vector2 startPos, Vector2 endPos)
	{
		Vector2 diff = endPos - startPos;

		return Mathf.Atan2(diff.y, diff.x);
	}

	// Return a normalized vector from the startPos pointing towards endPos
	public static Vector2 GetDirectionTowards(Vector2 startPos, Vector2 endPos)
	{
		float rangle = GetRAngleTowards(startPos, endPos);

		return new Vector2(Mathf.Cos(rangle), Mathf.Sin(rangle)).normalized;
	}
}
