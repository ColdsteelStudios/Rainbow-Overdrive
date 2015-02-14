// ---------------------------------------------------------------------------
// MatchManagerLocal.cs
// 
// Class description goes here
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchManagerLocal : MonoBehaviour 
{
    //How many players are in this match
    private int m_playerCount = 0;
    //How many players are alive
    private int m_playersAlive = 0;
    //GUI Message
    private Text m_messageDisplay;
    //Countdown for beginning of match
    private float m_matchCountdown;
    private bool m_matchCountdownActive;
    private bool m_gameStarted = false;
    //Prefab for spawning players
    public GameObject m_playerPrefab;

    void Update()
    {
        if (m_matchCountdownActive)
            MatchCountdown();
    }

    //Updates display message indicating how long
    //until the next match begins
    private void MatchCountdown()
    {
        //Calculate how much time is remaining
        m_matchCountdown -= Time.deltaTime;
        //Once counter reaches zero, begin the match
        if(m_matchCountdown<=0.0f)
        {
            //Disable match countdown
            m_matchCountdownActive = false;
            //Start phase three of match start
            StartGamePhaseThree();
            return;
        }
    }

    //Called from players when they die
    public void PlayerDead()
    {
        m_playersAlive--;
        //Check if we have a winner
        if((m_playersAlive<=1)&& m_gameStarted)
        {
            //End the match
            m_gameStarted = false;
            //Announce the victor
            int winner = AnnounceVictor();
            Cleanup();
            GameObject.Find("System").GetComponent<CallBack>().CreateCallback(this.gameObject, "StartGamePhaseOne", 3.0f);
        }
    }

    private int AnnounceVictor()
    {
        string winner = GameObject.Find("SplitScreenCamera(Clone)").transform.tag;
        if (winner == "P1CAM")
            return 1;
        if (winner == "P2CAM")
            return 2;
        if (winner == "P3CAM")
            return 3;
        if (winner == "P4CAM")
            return 4;
        return 420;
    }

    //Cleans up the scene, getting it ready for the next match
    private void Cleanup()
    {
        //Find all remaining players and tell them to clean up
        GameObject[] l_players = GameObject.FindGameObjectsWithTag("Player");
        if(l_players!=null)
        {
            foreach(GameObject P in l_players)
            {
                P.SendMessage("Cleanup");
                P.SendMessage("DestroyColliders");
            }
        }
    }

    //Spreads all players evenly around the outside of the arena
    private void SpreadPlayers()
    {
        //Find the positions of the arena center and arena
        //edge objects, there are used to spawn players in
        //the correct positions at the start of a new match
        Vector3 l_arenaCenter = GameObject.Find("ArenaCenter").transform.position;
        Vector3 l_arenaEdge = GameObject.Find("ArenaEdge").transform.position;
        //Calculate the distance between these two objects
        //This is how far we will spawn the players from the
        //centre of the arena
        float l_spawnDistance = Vector3.Distance(l_arenaCenter, l_arenaEdge);
        //Find all players in the scene
        GameObject[] l_players = GameObject.FindGameObjectsWithTag("Player");
        //We spawn them in a circle around the arena center
        //They will be evenly spaced around the edge of
        //this circle, figure out the space to put between
        float l_rotationAmount = 360.0f / l_players.Length;
        //Create some temp variables used in the placement
        //of the players
        int iter = 1;
        Vector3 yaxis = new Vector3(0, 1, 0);
        GameObject G = GameObject.Instantiate(new GameObject()) as GameObject;
        //Loop through the players and move them to the
        //correct starting positions around the circle
        foreach (GameObject Player in l_players)
        {
            //Create a temp transform for calculating where to
            //place this player and place it on the arena edge
            Transform T = G.transform;
            T.position = l_arenaEdge;
            //Rotate it around the center in the desired amount
            T.RotateAround(l_arenaCenter, yaxis, l_rotationAmount * iter);
            //Send the player to this position
            Player.SendMessage("MoveTo", T.position);
            //Increment iterator so next placement will be
            //in the correct placement
            iter++;
        }
        GameObject.Destroy(G);
    }

    //Spawns players and sets their cameras up for them
    private void SpawnPlayers()
    {
        for (int i = 1; i < m_playerCount+1; i++)
        {
            GameObject P = GameObject.Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            GameObject Cam = GameObject.FindGameObjectWithTag("P" + i + "CAM");
            Cam.GetComponent<SmoothFollower>().target = P.transform;
            P.SendMessage("SetPlayerNumber", i);
        }
    }

    //Called when all players indicate they are ready to play
    private void StartGamePhaseOne()
    {
        m_gameStarted = true;
        SpawnPlayers();
        StartGamePhaseTwo();
    }

    private void StartGamePhaseTwo()
    {
        //Note player count so we know when to end the match
        m_playersAlive = m_playerCount;
        SpreadPlayers();
        m_matchCountdown = 5.0f;
        m_matchCountdownActive = true;
    }

    private void StartGamePhaseThree()
    {
        //Find all players
        GameObject[] l_players = GameObject.FindGameObjectsWithTag("Player");
        //Release their controls
        foreach (GameObject Player in l_players)
            Player.SendMessage("StartMatch");
    }

    public void SetPlayerCount(int a_newPlayerCount)
    {
        m_playerCount = a_newPlayerCount;
    }
}