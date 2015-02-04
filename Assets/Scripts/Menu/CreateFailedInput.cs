// ---------------------------------------------------------------------------
// CreateFailedInput.cs
// 
// When creation of new room failed allows player to retry or return to
// the ConnectNameInput menu section
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class CreateFailedInput : MonoBehaviour 
{
	void Update () 
	{
		if(Input.GetKeyDown (KeyCode.F1))
			GameObject.Find ("Canvas").GetComponent<MenuHandler>().RetryCreateRoom();
		if(Input.GetKeyDown (KeyCode.Escape))
			GameObject.Find ("Canvas").GetComponent<MenuHandler>().CreateFailedBack();
	}
}