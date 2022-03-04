using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// Custom cursor script
public class CursorManager : MonoBehaviour
{
	public static CursorManager instance;

	public float spinsPerSecond;

	private float degreesPerFrame;
	private float currAngle;


	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		degreesPerFrame = spinsPerSecond * 360 / Application.targetFrameRate;
	}

	private void Update()
	{
		// visible if: not in game bounds OR application is NOT focused
		// invisible if: in game bounds AND application is focused
		Cursor.visible = !IsInGameBounds() || !Application.isFocused;
		

		// Spin regardless of delta time
		if (currAngle > 360 || currAngle < -360)
			currAngle %= 360;
		currAngle += degreesPerFrame;
		transform.localRotation = Quaternion.Euler(0, 0, currAngle);

		Vector3 mousePos = Input.mousePosition;
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
		worldPos.z = 0;

		transform.position = worldPos;
		// customCursor.anchoredPosition = mousePos;
	}

	// Returns false if the mouse is not within the bounds of the game window
	private bool IsInGameBounds()
	{
		// TEMP: Remove editor code
#if UNITY_EDITOR
		if (Input.mousePosition.x <= 0 || Input.mousePosition.y <= 0 || Input.mousePosition.x >= Handles.GetMainGameViewSize().x - 1 || Input.mousePosition.y >= Handles.GetMainGameViewSize().y - 1)
		{
			return false;
		}
#else
		if (Input.mousePosition.x <= 0 || Input.mousePosition.y <= 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1)
		{
			return false;
		}
#endif
		else
		{
			return true;
		}
	}
}