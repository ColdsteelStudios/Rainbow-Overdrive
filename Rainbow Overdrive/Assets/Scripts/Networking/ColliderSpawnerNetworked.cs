// ---------------------------------------------------------------------------
// ColliderSpawnerNetworked.cs
// 
// Spawns invisible colliders behind they player as they move along so we have
// something to collide with in order to know when a player hits a trail
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColliderSpawnerNetworked : MonoBehaviour 
{
	public float m_spawnInterval;
	public float m_boostSpawnInterval;
	private float m_nextSpawn;
	public GameObject m_colliderPrefab;
	public GameObject m_colliderParentPrefab;
	private GameObject m_colliderParent;
	private PhotonView m_PV;

	void Start ()
	{
		m_nextSpawn = 0.0f;
		m_PV = transform.GetComponent<PhotonView> ();
	}
	
	void Update () 
	{
		if(m_PV.isMine)
		{
			m_nextSpawn -= Time.deltaTime;
			
			if(m_nextSpawn <= 0.0f)
			{
				m_PV.RPC ("SpawnCollider", PhotonTargets.All);
				m_nextSpawn = m_spawnInterval;
			}
		}
	}

	public void CreateNewColliderParent()
	{
		m_colliderParent = GameObject.Instantiate (m_colliderParentPrefab, Vector3.zero, Quaternion.identity) as GameObject;
	}

	[RPC]
	public void DestroyColliders()
	{
		GameObject.Destroy (m_colliderParent);
	}

	[RPC]
	public void SpawnCollider()
	{
        if(m_colliderParent!=null)
        {
            //Calculate the correct position to spawn our new collider
            Vector3 l_colliderSpawnPosition = transform.position + (-transform.forward * 2.1f);
            //Add a new collider to the scene, place is just behind the player
            GameObject l_newSpawn = GameObject.Instantiate(m_colliderPrefab, l_colliderSpawnPosition, Quaternion.identity) as GameObject;
            //Rotate the collider so its facing the right direction and lines up with the bikes trail
            l_newSpawn.transform.LookAt(l_newSpawn.transform.position + transform.right);
            //Set its parent
            l_newSpawn.transform.parent = m_colliderParent.transform;
        }
	}
}