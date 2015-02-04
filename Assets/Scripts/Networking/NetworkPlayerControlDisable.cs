// ---------------------------------------------------------------------------
// NetworkPlayerControlDisable.cs
// 
// Disables scripts on the players that should only be run on the client side
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class NetworkPlayerControlDisable : Photon.MonoBehaviour
{
	void Start () 
	{
		if(!photonView.isMine)
		{
			gameObject.GetComponent<BikeControlB>().enabled = false;
			gameObject.GetComponent<CollisionDetection>().enabled = false;
			gameObject.GetComponent<ColliderSpawner>().enabled = false;
		}
	}
}