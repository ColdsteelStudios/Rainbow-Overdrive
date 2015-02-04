// ---------------------------------------------------------------------------
// ConnectionFailedInput.cs
// 
// Class description goes here
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class ConnectionFailedInput : MonoBehaviour 
{
	public GameObject m_system;

	void Update()
	{
		if(Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();
		if(Input.GetKeyDown (KeyCode.F1))
			m_system.GetComponent<MenuHandler>().RetryConnection();
	}
}