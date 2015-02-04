// ---------------------------------------------------------------------------
// MatchManager.cs
// 
// Starts new matches, spawns players, announces winner before start of next
// match etc.
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchManager : MonoBehaviour
{
	//Parts of the menu, will be removed when match
	//starts then added back if disconnection or 
	//other errors occur
	public GameObject m_screenCover;
	public GameObject m_defaultCamera;
	public Text m_messageDisplay;

	//Following variables are only used by the game
	//manager, they aren't hosting the game but they
	//are taking care of everything that a server
	//normally would handle.
	private bool m_isManager;
	private int m_connectedPlayers = 0;
	private bool m_matchActive = false;
	private bool m_matchStarting = false;
	private float m_matchCountdown = 3.0f;
	private bool m_enoughPlayers = false;
	private bool m_spreadCountdown = false;
	private float m_spreadCounter = 3.0f;
	private bool m_postMatch = false;
	private float m_postMatchCounter = 10.0f;
	private bool m_matchCompleteDelay = false;
	private float m_matchCompleteCounter = 3.0f;

	private int m_playersRemaining;

	//Used for spawning into the map
	public GameObject m_bikePrefab;
	private PhotonView m_photonView;

	void Start()
	{
		m_photonView = gameObject.GetComponent<PhotonView>();
	}

	void Update()
	{
		//Make sure only the game manager is running this code
		if(m_isManager)
		{
			//When player starts a new room, they have to wait
			//for a second player to join before they can begin
			if(!m_enoughPlayers)
			{
				if(m_connectedPlayers >= 2)
				{
					m_enoughPlayers = true;
					StartNewMatch();
				}
			}

			//Counter before finding and spreading players at the 
			//start of a new match
			if(m_spreadCountdown && (m_spreadCounter > 0.0f ))
			{
				m_spreadCounter -= Time.deltaTime;
				if ( m_spreadCounter <= 0.0f )
				{
					m_spreadCountdown = false;
					Spread ();
				}
			}

			//ITS THE FINAL COUNTDOWN
			//before the match finally starts
			if(m_matchStarting && (m_matchCountdown>0.0f))
			{
				m_matchCountdown -= Time.deltaTime;
				string l_displayMessage = "Starting in " + ((int)m_matchCountdown);
				transform.GetComponent<PhotonView>().RPC ( "SetDisplayMessage", PhotonTargets.All, l_displayMessage );
				if(m_matchCountdown < 0.0f)
					StartMatch();
			}

			//Post match countdown before new match starts
			if(m_postMatch && (m_postMatchCounter>0.0f))
			{
				m_postMatchCounter -= Time.deltaTime;
				if(m_postMatchCounter <= 5.0f)
				{
					string l_displayMessage = "Next match will begin in " + ((int)m_postMatchCounter);
					transform.GetComponent<PhotonView>().RPC ( "SetDisplayMessage", PhotonTargets.All, l_displayMessage );
				}
				if(m_postMatchCounter <= 0.0f)
				{
					Reset();
				}
			}

			//Delay before victor is announced
			if(m_matchCompleteDelay && (m_matchCompleteCounter>0.0f))
			{
				m_matchCompleteCounter -= Time.deltaTime;
				if(m_matchCompleteCounter <= 0.0f)
				{
					m_matchCompleteDelay = false;
					MatchComplete();
				}
			}
		}
	}

	//Resets all values and gets ready for the next match
	private void Reset()
	{
		//Reset all values
		m_connectedPlayers = 0;
		m_matchActive = false;
		m_matchStarting = false;
		m_matchCountdown = 3.0f;
		m_enoughPlayers = false;
		m_spreadCountdown = false;
		m_spreadCounter = 3.0f;
		m_postMatch = false;
		m_postMatchCounter = 10.0f;
		m_matchCompleteDelay = false;
		m_matchCompleteCounter = 3.0f;
		//Destroy the remaining player object
		if(GameObject.FindGameObjectWithTag("Player"))
		{
			PhotonView l_remainingPhoton = GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonView>();
			l_remainingPhoton.RPC ("GameOver", l_remainingPhoton.owner);
		}
		CleanShit ();
		StartNewMatch ();
	}

	private void MatchComplete()
	{
		PhotonView l_matchWinner = GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonView>();
		l_matchWinner.RPC ("Victory", l_matchWinner.owner);
		m_postMatch = true;
		m_postMatchCounter = 10.0f;
		GameObject[] l_players = GameObject.FindGameObjectsWithTag("Player");
		foreach ( GameObject player in l_players )
			PhotonNetwork.Destroy (player);
	}

	private void StartMatch()
	{
		//Clear all players display messages
		transform.GetComponent<PhotonView>().RPC ("SetDisplayMessage", PhotonTargets.All, "");
		//Find all the players
		GameObject[] l_players = GameObject.FindGameObjectsWithTag("Player");
		//Take a note of how many players there are in this match
		m_playersRemaining = l_players.Length;
		//Release all players
		foreach (GameObject player in l_players)
		{
			PhotonView l_photonView = player.GetComponent<PhotonView>();
			l_photonView.RPC ("BikeReady", l_photonView.owner);
		}
	}

	[RPC]
	public void PlayerDead()
	{
		if(m_isManager)
		{
			m_playersRemaining--;
			if(m_playersRemaining <= 1)
			{
				m_matchCompleteDelay = true;
				m_matchCompleteCounter = 3.0f;
			}
		}
	}

	private void Spread()
	{
		//Find all the newly spawned players
		GameObject[] l_newPlayers = GameObject.FindGameObjectsWithTag("Player");
		//Spread them out around the arena
		GameObject.Find ("System").GetComponent<SpreadPlayers>().Spread(l_newPlayers);
		//Start countdown for start of match
		m_matchStarting = true;
		m_matchCountdown = 5.0f;
		string l_displayMessage = "Starting in " + ((int)m_matchCountdown);
		transform.GetComponent<PhotonView>().RPC ( "SetDisplayMessage", PhotonTargets.All, l_displayMessage);
	}

	//Sets display message for all players
	[RPC]
	public void SetDisplayMessage( string a_displayMessage )
	{
		m_messageDisplay.text = a_displayMessage;
	}

	//Called client side when a player creates a
	//new room, they will be the manager until
	//they disconnect, then it will be given
	//to someone else
	public void SetManager()
	{
		m_isManager = true;
		m_connectedPlayers = 1;
		m_messageDisplay.text = "Need 1 more player to start";
	}

	void OnPhotonPlayerConnected(PhotonPlayer connected)
	{
		m_connectedPlayers++;
	}

	private void StartNewMatch()
	{
		if(m_isManager)
		{
			//Remove current display message
			m_messageDisplay.text = "";
			//Spawn players
			m_photonView.RPC ( "SpawnMe", PhotonTargets.All );
			//Wait 3 seconds for players to spawn before we spread them
			m_spreadCountdown = true;
		}
	}

	[RPC]
	public void SpawnMe()
	{
		//Tells each connected player to spawn a bike into the scene
		PhotonNetwork.Instantiate( "FreeTurn", Vector3.zero, Quaternion.identity, 0 );
	}

	private void CleanShit()
	{
		GameObject[] l_trails = GameObject.FindGameObjectsWithTag("Trail");
		if(l_trails != null)
		{
			foreach(GameObject trail in l_trails)
				PhotonNetwork.Destroy(trail);
		}

		GameObject[] l_colliders = GameObject.FindGameObjectsWithTag ("Collider");
		if(l_colliders != null)
		{
			foreach(GameObject collider in l_colliders)
				PhotonNetwork.Destroy(collider);
		}

		int l_trailCounter = GameObject.FindGameObjectsWithTag ("Trail").Length;
		Debug.Log ("Cleaned up trails, " + l_trailCounter + " are left for some reason...");
	}
}