// ---------------------------------------------------------------------------
// SpreadPlayers.cs
// 
// Takes in a center position and a distance from the center to spread players
// Spreads them evenly around the outside of a circle around the center positions
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class SpreadPlayers : MonoBehaviour
{
	public GameObject m_arenaCenter;
	public GameObject m_arenaEdge;
	private float m_spawnDistance;

	void Start()
	{
		m_spawnDistance = Vector3.Distance ( m_arenaCenter.transform.position, m_arenaEdge.transform.position );
	}

	public void Spread ( GameObject[] a_playerArray )
	{
		float a_rotationAmount = 360.0f / a_playerArray.Length;

		for ( int i = 0; i < a_playerArray.Length; i++ )
		{
			//We are going to move the System object around to figure out where we are going
			//to place the players in their spread positions
			transform.position = m_arenaCenter.transform.position;
			transform.Translate ( Vector3.forward * m_spawnDistance );
			transform.RotateAround ( m_arenaCenter.transform.position, new Vector3 ( 0, 1, 0 ), a_rotationAmount * (i+2) );
			//This System object is now in the location where we want to move this player
			//Send that player a message telling them to move to this location
			PhotonView l_photonView = a_playerArray[i].GetComponent<PhotonView>();
			l_photonView.RPC ( "MoveTo", l_photonView.owner, transform.position );
			l_photonView.RPC ( "SetCamera", l_photonView.owner );
		}
	}
}