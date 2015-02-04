// ---------------------------------------------------------------------------
// CollisionDetection.cs
// 
// Checks for collisions against other players, walls and enemy trail colliders
// Kills the player if they collider with any of these
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		if(gameObject.GetComponent<PhotonView>().isMine)
		{
			//Collision with a wall = death
			if(other.transform.tag == "Wall")
				KillMe();
			//Collision with trail colliders = death
			if(other.transform.name == "trailCollider(Clone)")
				KillMe ();
		}
	}

	private void KillMe()
	{
		//Send RPC message to match manager letting it know were dead
		PhotonView l_matchPhotonView = GameObject.Find ("MatchManager").GetComponent<PhotonView>();
		l_matchPhotonView.RPC ( "PlayerDead", PhotonTargets.All );
		//Destroy our colliders
		gameObject.SendMessage ("DestroyColliders");
		//Destroy our trail renderer
		if(transform.FindChild ("Trail(Clone))"))
		{
			GameObject l_trail = transform.FindChild ("Trail(Clone)").gameObject;
			PhotonNetwork.Destroy(l_trail);
		}
		//Destroy our player
		PhotonNetwork.Destroy (gameObject);
	}

	[RPC]
	public void GameOver()
	{
		//Destroy our colliders
		gameObject.SendMessage ("DestroyColliders");
		//Destroy our player
		PhotonNetwork.Destroy (gameObject);
	}
}