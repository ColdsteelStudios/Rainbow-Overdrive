// ---------------------------------------------------------------------------
// MenuHandler.cs
// 
// enables/disables certain parts of the game menu when asked
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuHandler : MonoBehaviour 
{
	//Screen for pending connection
	public GameObject m_connectingScreen;
	//Alerts player that connection failed
	public GameObject m_connectionFailedScreen;
	//Gives player choice to connect to random, connect to specific or create new game
	public GameObject m_connectionSelection;
	//Displayed when connecting to a random game
	public GameObject m_connectingRandom;
	//Displayed when we failed to join a random game
	public GameObject m_connectingRandomFailed;
	//Allows player to type a name and try to connect to that match
	public GameObject m_connectName;
	//Allows player to input a name and create a new game with that name
	public GameObject m_createNew;
	//Displays message letting player know we are trying to create a new game
	public GameObject m_creatingNew;
	//Allows player to retry or return to name input when room creation fails
	public GameObject m_createNewFailed;
	//Alerts player we are trying to join the game they want
	public GameObject m_connectingRoom;

	//When multiple menu's can lead into the same photon network we need to know where we came from
	private string m_previousMenu;

	void Start()
	{
		//Activate connecting screen and attempt to connect to photon
		m_connectingScreen.SetActive (true);
		PhotonNetwork.ConnectUsingSettings("v1.0");
	}

	//Failed to connect, go from root menu to connection failed screen
	public void OnFailedToConnectToPhoton()
	{
		m_connectingScreen.SetActive (false);
		m_connectionFailedScreen.SetActive (true);
	}

	//After connection failed, player wanted to retry
	public void RetryConnection()
	{
		m_connectingScreen.SetActive (true);
		m_connectionFailedScreen.SetActive (false);
	}

	//Go from connection type selection to connect random
	public void ClickConnectRandom()
	{
		m_connectionSelection.SetActive (false);
		PhotonNetwork.JoinRandomRoom ();
		m_connectingRandom.SetActive (true);
	}

	//Go from connection type selection to connect with name
	public void ClickConnectName()
	{
		m_connectionSelection.SetActive (false);
		m_connectName.SetActive (true);
	}

	//Go from connection type selection to create new
	public void ClickCreateNew()
	{
		m_connectionSelection.SetActive (false);
		m_createNew.SetActive (true);
	}

	//Gets name from input box and tries to make
	//a game with that name
	public void ClickCreateManual()
	{
		string l_gameName = m_connectName.transform.FindChild ("NameInputBox").GetComponent<InputField> ().text;
		PhotonNetwork.CreateRoom (l_gameName);
		m_connectName.SetActive (false);
		m_creatingNew.SetActive (true);
		m_previousMenu = "CreateManual";
	}

	public void ClickInputBack()
	{
		m_connectName.SetActive (false);
		m_connectionSelection.SetActive (true);
	}

	public void ClickJoin()
	{
		string l_connectName = transform.FindChild ("ConnectNameInput").FindChild ("NameInputBox").GetComponent<InputField> ().text;
		PhotonNetwork.JoinRoom (l_connectName);
	}

	public void RetryJoinRandom()
	{
		m_connectingRandomFailed.SetActive (false);
		PhotonNetwork.JoinRandomRoom ();
		m_connectingRandom.SetActive (true);
	}

	public void RetryFailedBack()
	{
		m_connectingRandomFailed.SetActive (false);
		m_connectionSelection.SetActive (true);
	}

	public void RetryCreateRoom()
	{
		m_createNewFailed.SetActive (false);
		string l_gameName = m_connectName.transform.FindChild ("NameInputBox").GetComponent<InputField> ().text;
		PhotonNetwork.CreateRoom (l_gameName);
		m_creatingNew.SetActive (true);
	}

	public void CreateFailedBack()
	{
		m_createNewFailed.SetActive (false);
		m_createNew.SetActive (true);
	}

	//Connection to Photon succeeded
	//Go from connection pending to connection selection
	void OnConnectedToPhoton ()
	{
		m_connectingScreen.SetActive (false);
		m_connectionSelection.SetActive (true);
	}

	//Connection to random game failed
	void OnPhotonRandomJoinFailed()
	{
		m_connectingRandom.SetActive (false);
		m_connectingRandomFailed.SetActive (true);
	}

	void OnPhotonCreateRoomFailed()
	{
		m_creatingNew.SetActive (false);
		m_createNewFailed.SetActive (true);
	}

	//May come here from join random, join selective or create new
	void OnJoinedRoom()
	{

	}
}