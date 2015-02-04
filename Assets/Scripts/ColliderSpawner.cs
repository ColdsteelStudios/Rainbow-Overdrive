using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColliderSpawner : MonoBehaviour 
{
	public float m_spawnInterval;
	public float m_boostSpawnInterval;
	private float m_nextSpawn;

	public GameObject m_colliderPrefab;

	private List<GameObject> m_colliderObjects;

	void Start () 
	{
		m_nextSpawn = 0.0f;
		m_colliderObjects = new List<GameObject> ();
	}
	
	void Update () 
	{
		m_nextSpawn -= Time.deltaTime;
		
		if(m_nextSpawn <= 0.0f)
		{
			SpawnCollider();
			
			if((Input.GetKey (KeyCode.LeftShift))||
			   (Input.GetKey (KeyCode.RightShift)))
				m_nextSpawn = m_boostSpawnInterval;
			else
				m_nextSpawn = m_spawnInterval;
		}
	}

	//This function will only be called by a player when they die, this will
	//destroy all of their trail colliders across the network.
	public void DestroyColliders()
	{
		if(m_colliderObjects != null)
		{
			if(m_colliderObjects.Count > 0)
			{
				foreach(GameObject collider in m_colliderObjects)
					PhotonNetwork.Destroy(collider);
			}
		}
	}

	private void SpawnCollider()
	{
		//Calculate the correct position to spawn our new collider
		Vector3 l_colliderSpawnPosition = transform.position + (-transform.forward * 2.1f);
		//Add a new collider to the scene, place is just behind the player
		GameObject l_newSpawn = PhotonNetwork.Instantiate("trailCollider", l_colliderSpawnPosition, Quaternion.identity, 0) as GameObject;
		//Rotate the collider so its facing the right direction and lines up with the bikes trail
		l_newSpawn.transform.LookAt(l_newSpawn.transform.position + transform.right);
		//Add the collider to our list of spawned colliders so we know which ones we have added to the scene.
		//We need this list so we can remove all of our colliders from the scene when we die
		m_colliderObjects.Add(l_newSpawn);
	}
}