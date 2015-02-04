// ---------------------------------------------------------------------------
// JoinRandomFailed.cs
// 
// Lets player retry finding a random room or go back to connection type selection
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class JoinRandomFailed : MonoBehaviour 
{
	void Update () 
	{
		if(Input.GetKeyDown (KeyCode.F1))
			GameObject.Find ("Canvas").GetComponent<MenuHandler>().RetryJoinRandom();
		if(Input.GetKeyDown (KeyCode.Escape))
			GameObject.Find ("Canvas").GetComponent<MenuHandler>().RetryFailedBack();
	}
}