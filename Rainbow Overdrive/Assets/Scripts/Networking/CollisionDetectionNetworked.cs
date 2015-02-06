// ---------------------------------------------------------------------------
// CollisionDetectionNetworked.cs
// 
// Checks for collisions against other players, walls and enemy trail colliders
// Kills the player if they collider with any of these
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class CollisionDetectionNetworked : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		if(gameObject.GetComponent<PhotonView>().isMine)
		{
			//Collision with a wall = death
			if(other.transform.tag == "Wall")
                KillMe();
            //Collision with trail colliders = death
            if (other.transform.name == "Collider")
                KillMe();
        }
	}

	private void KillMe()
	{
		//Send RPC message to match manager letting it know were dead
		PhotonView l_matchPhotonView = GameObject.Find ("MatchManager").GetComponent<PhotonView>();
		l_matchPhotonView.RPC ( "PlayerDead", PhotonTargets.All );
        //Tell our player to clean up
        PhotonView PV = transform.GetComponent<PhotonView>();
        PV.RPC("DestroyColliders", PhotonTargets.All);
        PV.RPC("Cleanup", PV.owner);
    }

	[RPC]
	public void GameOver()
	{
        //Tell our player to clean up
        PhotonView PV = transform.GetComponent<PhotonView>();
        PV.RPC("DestroyColliders", PhotonTargets.All);
        PV.RPC("Cleanup", PV.owner);
    }
}