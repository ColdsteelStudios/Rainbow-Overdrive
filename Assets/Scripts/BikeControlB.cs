// ---------------------------------------------------------------------------
// BikeControlB.cs
// 
// Control Script for the free roaming type Tron Bikes
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BikeControlB : MonoBehaviour 
{
	public float m_bikeSpeed;
	public float m_turnSpeed;
	public float m_leanStrength;
	public float m_maxLean;

	private GameObject m_bikeChild;
	private GameObject m_lightsChild;
	private float m_childRotation;
	public GameObject m_trailPrefab;
	private GameObject m_myTrail;

	//Set to true when match starts, allows bike to move, recieve input etc.
	private bool m_bikeReady = false;

	void Start()
	{
		m_bikeChild = transform.FindChild ("Tronbike").gameObject;
		m_lightsChild = transform.FindChild ("TronLights").gameObject;
	}

	void Update () 
	{
		if(m_bikeReady)
		{
			MoveBikeForward ();
			SteerBike ();
		}
	}

	//Cleans up all our colliders, our trail and then destroys ourself
	[RPC]
	public void Cleanup()
	{
		PhotonNetwork.Destroy (m_myTrail);
		PhotonNetwork.Destroy (this.gameObject);
	}

	[RPC]
	public void Victory()
	{
		GameObject.Find ("Text").GetComponent<Text>().text = "Victory!";
	}

	//Called from host once countdown is over and match begins
	[RPC]
	public void StartMatch()
	{
		PhotonView PV = transform.GetComponent<PhotonView> ();
		if(PV.isMine)
		{
			//Activate box collider
			transform.GetComponent<BoxCollider> ().enabled = true;
			//Turn gravity back on
			transform.GetComponent<Rigidbody> ().useGravity = true;
			//Unlock controls
			m_bikeReady = true;
			//Add a trail renderer to the bike
			m_myTrail = PhotonNetwork.Instantiate("Trail", transform.position, Quaternion.identity, 0) as GameObject;
			//Parent it
			m_myTrail.transform.parent = transform;
		}
		//Activate collider spawner
		transform.GetComponent<ColliderSpawner>().enabled = true;
		//Spawn new collider spawner parent
		transform.GetComponent<ColliderSpawner> ().CreateNewColliderParent ();
	}

	//This is only ever called by the MatchManager when setting
	//up a new match and spreading players out
	[RPC]
	public void MoveTo(Vector3 a_newPosition)
	{
		transform.position = a_newPosition;
		transform.LookAt(GameObject.Find ("ArenaCenter").transform.position);
		//Tell scene camera to follow us
		GameObject.Find ("Camera").GetComponent<SmoothFollower>().target = transform;
	}

	private void MoveBikeForward()
	{
		//Get current position
		Vector3 l_bikePos = transform.position;
		//move it forward
		l_bikePos += transform.forward * (m_bikeSpeed * Time.deltaTime);
		//update position
		transform.position = l_bikePos;
	}

	private void SteerBike()
	{
		if((Input.GetKey (KeyCode.LeftArrow)) ||
			(Input.GetKey (KeyCode.A)) || (Input.GetAxis("Horizontal")<0.0f))
			TurnLeft();
		else if((Input.GetKey (KeyCode.RightArrow)) ||
		   (Input.GetKey (KeyCode.D)) || (Input.GetAxis("Horizontal")>0.0f))
			TurnRight();
		else
			ResetLeaning();
	}

	private void TurnLeft()
	{
		transform.RotateAround (transform.up, Time.deltaTime * -m_turnSpeed);
		//As we turn left we want the bike to lean into the corner
		if(m_childRotation<m_maxLean)
		{
			m_bikeChild.transform.RotateAround (m_bikeChild.transform.right, Time.deltaTime * m_leanStrength);
			m_lightsChild.transform.RotateAround (m_lightsChild.transform.right, Time.deltaTime * m_leanStrength);
			m_childRotation += Time.deltaTime * m_leanStrength;
		}
	}

	private void TurnRight()
	{
		transform.RotateAround (transform.up, Time.deltaTime * m_turnSpeed);
		//As we turn left we want the bike to lean into the corner
		if(m_childRotation>-m_maxLean)
		{
			m_bikeChild.transform.RotateAround (m_bikeChild.transform.right, Time.deltaTime * -m_leanStrength);
			m_lightsChild.transform.RotateAround (m_lightsChild.transform.right, Time.deltaTime * -m_leanStrength);
			m_childRotation -= Time.deltaTime * m_leanStrength;
		}
	}

	private void ResetLeaning()
	{
		if((m_childRotation<0.1f)&&(m_childRotation>-0.1f))
			return;

		if(m_childRotation<0.1f)
		{
			m_bikeChild.transform.RotateAround (m_bikeChild.transform.right, Time.deltaTime * m_leanStrength);
			m_lightsChild.transform.RotateAround (m_lightsChild.transform.right, Time.deltaTime * m_leanStrength);
			m_childRotation += Time.deltaTime * m_leanStrength;
		}
		else if(m_childRotation>-0.1f)
		{
			m_bikeChild.transform.RotateAround (m_bikeChild.transform.right, Time.deltaTime * -m_leanStrength);
			m_lightsChild.transform.RotateAround (m_lightsChild.transform.right, Time.deltaTime * -m_leanStrength);
			m_childRotation -= Time.deltaTime * m_leanStrength;
		}
	}
}