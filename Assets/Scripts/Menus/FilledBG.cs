using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilledBG : MonoBehaviour
{
	public GameObject fillLine;

	private Vector2 leftStartPos = new Vector2(-23f, 0f);
	private Vector2 rightStartPos = new Vector2(23f, 0f);

	// Offset of next line from previous
	private float offset = 2.8f;
	private float timeBetweenLines = 0.13f;
	private int totalLinesPerSide = 9;

	private Stack<FilledBGFiller> fillStack = new Stack<FilledBGFiller>();


	// Returns the amount of time in seconds that this will take to finish
	public float StartFill()
	{
		StartCoroutine(FillBehavior());

		return totalLinesPerSide * timeBetweenLines;
	}

	private IEnumerator FillBehavior()
	{
		Vector2 currLeftPos = leftStartPos;
		Vector2 currRightPos = rightStartPos;

		for (int i = 0; i < totalLinesPerSide; i++)
		{
			GameObject left = Instantiate(fillLine, currLeftPos, Quaternion.Euler(new Vector3(0, 0, -45f)));
			FilledBGFiller leftFiller = left.GetComponent<FilledBGFiller>();
			leftFiller.StartLeftToRight();
			left.transform.parent = transform;
			fillStack.Push(leftFiller);
			currLeftPos.x += offset;

			GameObject right = Instantiate(fillLine, currRightPos, Quaternion.Euler(new Vector3(0, 0, -45f)));
			FilledBGFiller rightFiller = right.GetComponent<FilledBGFiller>();
			rightFiller.StartRightToLeft();
			right.transform.parent = transform;
			fillStack.Push(rightFiller);
			currRightPos.x -= offset;
			yield return new WaitForSeconds(timeBetweenLines);
		}
	}

	public float UnFill()
	{
		StartCoroutine(UnFillBehavior());

		return totalLinesPerSide * timeBetweenLines;
	}

	private IEnumerator UnFillBehavior()
	{
		while (fillStack.Count > 0)
		{
			fillStack.Pop().PlayBackwards();
			fillStack.Pop().PlayBackwards();

			yield return new WaitForSeconds(timeBetweenLines);
		}
	}
}
