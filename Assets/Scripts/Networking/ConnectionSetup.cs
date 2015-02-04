﻿// ---------------------------------------------------------------------------
// MatchHandler.cs
// 
// 1st - Attempts to connect to Photon Servers, times out after 10 seconds
// Gives player option to quit or retry on timeout.
// 2nd - Attempts to connect to a random active game, 10s timeout
// Creates a new game if none could be found
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConnectionSetup : MonoBehaviour
{
	public Text m_messageDisplay;
	private bool m_createGameFailed = false;
	private bool m_startedNewGame = false;

	//Connecting to Photon
	private float m_connectionTimeout;
	private bool m_connectedToPhoton;
	private bool m_photonConnectionFailed;

	//Joining an active room
	private float m_roomTimeout;
	private bool m_connectedToRoom;

	void Start()
	{
		TryConnectPhoton();
	}

	void Update()
	{
		//Trying to connect to photon servers
		if ( (!m_connectedToPhoton) && (m_connectionTimeout > 0.0f))
		{
			m_connectionTimeout -= Time.deltaTime;
			m_messageDisplay.text = "Connecting..." + "\n" + ((int)m_connectionTimeout).ToString();
			if ( m_connectionTimeout <= 0.0f )
				FailConnectPhoton();
		}

		//Trying to find an active game room to join
		if ( (m_connectedToPhoton) && (!m_connectedToRoom) && (m_roomTimeout > 0.0f))
		{
			if(Input.GetKeyDown (KeyCode.Escape))
				FailJoinRandomRoom();
			m_roomTimeout -= Time.deltaTime;
			m_messageDisplay.text = "Searching for room..." + "\n" + ((int)m_roomTimeout).ToString();
			if (m_roomTimeout <= 0.0f)
				FailJoinRandomRoom();
		}

		//Allow the user some options if everything failed
		if(m_photonConnectionFailed)
		{
			if(Input.GetKeyDown (KeyCode.F1))
				TryConnectPhoton();
			if(Input.GetKeyDown (KeyCode.Escape))
				Application.Quit();
		}

		//Allow the user some options if they failed to create a new game
		if(m_createGameFailed)
		{
			if(Input.GetKeyDown (KeyCode.F1))
				TryAgainCreateNewRoom();
			if(Input.GetKeyDown (KeyCode.Escape))
				Application.Quit();
		}
	}

	//Called on start, spends up to 10 seconds trying to establish a
	//connection with the Photon servers
	private void TryConnectPhoton()
	{
		m_connectionTimeout = 10.0f;
		m_connectedToPhoton = false;
		m_photonConnectionFailed = false;
		m_messageDisplay.text = "Connecting..." + "\n" + ((int)m_connectionTimeout).ToString();
		PhotonNetwork.ConnectUsingSettings("v1.0");
	}

	//Called when our 10s of connection time ran out
	private void FailConnectPhoton()
	{
		m_photonConnectionFailed = true;
		m_messageDisplay.text = "Connection timed out" + "\n" + "F1 to retry" + "\n" + "Escape to quit";
	}

	private void TryJoinRandomRoom()
	{
		m_startedNewGame = false;
		m_connectedToPhoton = true;
		m_roomTimeout = 10.0f;
		m_connectedToRoom = false;
		m_messageDisplay.text = "Searching for room..." + "\n" + ((int)m_roomTimeout).ToString();
		PhotonNetwork.JoinRandomRoom();
	}

	//Called when our 10s of searching for an active game runs out
	private void FailJoinRandomRoom()
	{
		TryCreateNewRoom();
	}

	private void TryCreateNewRoom()
	{
		m_messageDisplay.text = "Unable to find an active game" + "\n" + "Trying to create a new game";
		PhotonNetwork.CreateRoom("RainbowOverdrive");
		m_startedNewGame = true;
	}

	private void TryAgainCreateNewRoom()
	{
		m_messageDisplay.text = "Trying to create a new game";
		PhotonNetwork.CreateRoom("RainbowOverdrive");
		m_startedNewGame = true;
	}

	void OnJoinedRoom()
	{
		m_messageDisplay.text = "";
		m_createGameFailed = false;
		m_photonConnectionFailed = false;
		m_connectedToPhoton = true;
		m_connectedToRoom = true;

		//When joining a room, check for m_startedNewGame
		//If true, this player will be run the manager
		//script until they disconnect
		if(m_startedNewGame)
			GameObject.Find ("MatchManager").SendMessage ("SetManager");
	}

	void OnPhotonCreateGameFailed()
	{
		m_createGameFailed = true;
		m_messageDisplay.text = "Failed to create a new game" + "\n" + "Check your connection and firewall settings" + "\n" + "F1 to retry" + "\n" + "Escape to quit";
	}

	void OnConnectedToPhoton()
	{
		m_connectedToPhoton = true;
		TryJoinRandomRoom ();
	}
}