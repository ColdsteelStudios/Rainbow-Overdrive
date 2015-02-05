// ---------------------------------------------------------------------------
// MatchManagerB.cs
// 
// Handles spawning of players, starting of matches and cleanup at the end
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent( typeof( PhotonView ) )]
public class MatchManager : Photon.MonoBehaviour 
{
	//How many players are currently connected
	private int m_playerCount = 1;
	//How many players are currently alive
	private int m_playersAlive;
	//Turns true once a second player connects
	private bool m_gameStarted = false;
	//Displays messages on GUI letting players
	//know what is going on before the match
	//has started
	private Text m_messageDisplay;
	//Countdown for beginning of the match
	private float m_matchCountdown;
	private bool m_matchCountdownActive;

	void Start()
	{
		//Grab the scenes message display object so
		//we can display messages to players
		m_messageDisplay = GameObject.Find ("Text").GetComponent<Text> ();
	}

	void Update()
	{
		//Countdown timer and display time remaining
		//to all players when the match is about
		//to commence
		if(m_matchCountdownActive)
			MatchCountdown();
	}

	//Updates display message of all players when the
	//match is about to begin
	private void MatchCountdown()
	{
		//Calculate how much time is remaining
		m_matchCountdown -= Time.deltaTime;
		//If the counter has reached zero then we
		//will continue onto phase 3 of match start
		if(m_matchCountdown <= 0.0f)
		{
			//Clear players display message
			this.photonView.RPC ("SetDisplayMessage", PhotonTargets.All, "");
			//Disable match countdown
			m_matchCountdownActive = false;
			//Start phase three
			StartGamePhaseThree();
		}
		//Create the message to display how long until the match begins
		string l_displayMessage = "Match will begin in " + ((int)m_matchCountdown);
		//Send this string to all players so they can update their display
		this.photonView.RPC ("SetDisplayMessage", PhotonTargets.All, l_displayMessage);
	}

	//Updates display message
	[RPC]
	public void SetDisplayMessage(string a_displayMessage)
	{
		m_messageDisplay.text = a_displayMessage;
	}

	//Called from players when they died
	//Checks how many players are left, and ends
	//the match when a single player is alive
	[RPC]
	public void PlayerDead()
	{
		if(IsOwner())
		{
			m_playersAlive--;
			//Check if we have a winner
			if((m_playersAlive <= 1) && m_gameStarted)
			{
				//End the match
				m_gameStarted = false;
				//Announce the victor
				PhotonView l_winnerPV = GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonView>();
				l_winnerPV.RPC ("Victory", l_winnerPV.owner);
				//Clean up the match then get ready to start over
				//after a few seconds have passed
				Cleanup();
				GameObject.Find ("System").GetComponent<CallBack>().CreateCallback( this.gameObject, "StartGamePhaseOne", 3.0f);
			}
		}
	}

	//Cleans up the scene, getting it ready for the start of a new match
	private void Cleanup()
	{
		//Find all remaining players and tell them to clean up
		GameObject[] l_players = GameObject.FindGameObjectsWithTag ("Player");
		if(l_players != null)
		{
			foreach(GameObject Player in l_players)
			{
				//Find the PV
				PhotonView PV = Player.GetComponent<PhotonView>();
				//Tell the player to clean up
				PV.RPC ("Cleanup", PV.owner);
				//Tell the players collider spawner to clean up
				PV.RPC ("DestroyColliders", PhotonTargets.All);
			}
		}
	}

	//Loops through the passed array of objects and either destroys them
	//of sends a message to their owner to destroy them for us
	private void DestroyObjects(GameObject[] a_objects)
	{
		foreach(GameObject GO in a_objects)
		{
			//Find the objects photon view
			PhotonView PV = GO.GetComponent<PhotonView>();
			//If we are the owner destroy it
			//If not, tell the owner to destroy it for us
			if(PV.isMine)
				PhotonNetwork.Destroy(GO);
			else
				PV.RPC ("Destroy", PV.owner);
		}
	}

	//Takes ownership of this object, called by
	//another client when the owner of the match
	//manager leaves - always needs to be one
	public void RequestOwnership()
	{
		this.photonView.RequestOwnership ();
	}

	//Check if we are the owner of this object
	private bool IsOwner()
	{
		return this.photonView.ownerId == PhotonNetwork.player.ID;
	}

	//Called whenever a new player connects to the game
	void OnPhotonPlayerConnected(PhotonPlayer connected)
	{
		if( IsOwner () )
		{
			m_playerCount++;
			//Start game when enough players have connected
			if((!m_gameStarted) && (m_playerCount >= 2))
				StartGamePhaseOne();
		}
	}

	//Called to each player by the match manager during setup
	[RPC]
	public void SpawnMe()
	{
		PhotonNetwork.Instantiate ("FreeTurn", Vector3.zero, Quaternion.identity, 0);
	}

	//Spreads all players evenly around the outside of the arena
	private void SpreadPlayers()
	{
		//Find the positions of the arena center and arena
		//edge objects, there are used to spawn players in
		//the correct positions at the start of a new match
		Vector3 l_arenaCenter = GameObject.Find ("ArenaCenter").transform.position;
		Vector3 l_arenaEdge = GameObject.Find ("ArenaEdge").transform.position;
		//Calculate the distance between these two objects
		//This is how far we will spawn the players from the
		//centre of the arena
		float l_spawnDistance = Vector3.Distance (l_arenaCenter, l_arenaEdge);
		//Find all players in the scene
		GameObject[] l_players = GameObject.FindGameObjectsWithTag ("Player");
		//We spawn them in a circle around the arena center
		//They will be evenly spaced around the edge of
		//this circle, figure out the space to put between
		float l_rotationAmount = 360.0f / l_players.Length;
		//Create some temp variables used in the placement
		//of the players
		int iter = 1;
		Vector3 yaxis = new Vector3 (0, 1, 0);
		GameObject G = GameObject.Instantiate (new GameObject ()) as GameObject;
		//Loop through the players and move them to the
		//correct starting positions around the circle
		foreach ( GameObject Player in l_players )
		{
			//Create a temp transform for calculating where to
			//place this player and place it on the arena edge
			Transform T = G.transform;
			T.position = l_arenaEdge;
			//Rotate it around the center in the desired amount
			T.RotateAround ( l_arenaCenter, yaxis, l_rotationAmount * iter );
			//Send the player to this position
			PhotonView pv = Player.GetComponent<PhotonView>();
			pv.RPC ("MoveTo", pv.owner, T.position);
			//Increment iterator so next placement will be
			//in the correct placement
			iter++;
		}
		GameObject.Destroy (G);
	}

	//Called when enough players have connected
	private void StartGamePhaseOne()
	{
		if(IsOwner ())
		{
			//Note that we are starting the match
			m_gameStarted = true;
			//Remove current display message
			m_messageDisplay.text = "";
			//Spawn players into the scene
			photonView.RPC ("SpawnMe", PhotonTargets.All);
			//Wait 3 seconds to give some time for all players
			//to spawn into the scene, then continue onto
			//phase two of the game setup
			GameObject.Find ("System").GetComponent<CallBack>().CreateCallback(this.gameObject, "StartGamePhaseTwo", 3.0f);
		}
	}

	//Called 3 seconds after StartGamePhaseOne
	private void StartGamePhaseTwo()
	{
		//Note how many we spawned so we know when to
		//end the current match and announce victor
		m_playersAlive = GameObject.FindGameObjectsWithTag("Player").Length;
		//Spread all players evenly around the outside of the arena
		SpreadPlayers ();
		//Begin the countdown for the start of the match
		m_matchCountdown = 5.0f;
		m_matchCountdownActive = true;
	}

	//Called after all players have spawned and a
	//5 second timer has finished counting down
	private void StartGamePhaseThree()
	{
        //Remove screen message
        this.photonView.RPC("SetDisplayMessage", PhotonTargets.All, "");
        //Find all the players in the scene
        GameObject[] l_players = GameObject.FindGameObjectsWithTag ("Player");
		//Release their controls, allowing the
		//match to begin
		foreach (GameObject Player in l_players)
		{
			//Get their photon view
			PhotonView PV = Player.GetComponent<PhotonView>();
			PV.RPC ("StartMatch", PhotonTargets.All);
		}
	}
}