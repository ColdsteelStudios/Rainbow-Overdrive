// ---------------------------------------------------------------------------
// CollisionDetectionLocal.cs
// 
// Checks for collisions against other players, walls and enemy trail colliders
// Kills the player if they collider with any of these
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class CollisionDetectionLocal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //Collision with a wall = death
        if (other.transform.tag == "Wall")
            KillMe();
        //Collision with trail colliders = death
        if (other.transform.tag == "Collider")
            KillMe();
    }

    private void KillMe()
    {
        //Send RPC message to match manager letting it know were dead
        GameObject.Find("MatchManager").SendMessage("PlayerDead");
        //Tell our player to clean up
        transform.SendMessage("DestroyColliders");
        transform.SendMessage("Cleanup");
    }

    public void GameOver()
    {
        //Tell our player to clean up
        transform.SendMessage("DestroyColliders");
        transform.SendMessage("DestroyColliders");
    }
}