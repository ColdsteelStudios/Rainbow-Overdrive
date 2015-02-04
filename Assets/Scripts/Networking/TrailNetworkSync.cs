// ---------------------------------------------------------------------------
// TrailNetworkSync.cs
// 
// Class description goes here
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class TrailNetworkSync : MonoBehaviour
{
	private float m_lastSynchronizationTime = 0f;
	private float m_syncDelay = 0f;
	private float m_syncTime = 0f;
	private Vector3 m_syncStartPosition = Vector3.zero;
	private Vector3 m_syncEndPosition = Vector3.zero;
	private Quaternion m_syncStartRotation = Quaternion.identity;
	private Quaternion m_syncEndRotation = Quaternion.identity;
	PhotonView m_photonView;

	void Start()
	{
		m_photonView = gameObject.GetComponent<PhotonView> ();
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Vector3 l_syncPosition = Vector3.zero;
		Quaternion l_syncRotation = Quaternion.identity;
		
		if(stream.isWriting)
		{
			//We own this player, send others our data
			l_syncPosition = transform.position;
			l_syncRotation = transform.rotation;
			stream.SendNext(l_syncPosition);
			stream.SendNext(l_syncRotation);
		}
		else
		{
			//Network player, receive data
			l_syncPosition = (Vector3)stream.ReceiveNext();
			l_syncRotation = (Quaternion)stream.ReceiveNext();
			
			m_syncTime = 0f;
			m_syncDelay = Time.time - m_lastSynchronizationTime;
			m_lastSynchronizationTime = Time.time;
			
			m_syncStartPosition = transform.position;
			m_syncEndPosition = l_syncPosition;
			m_syncStartRotation = transform.rotation;
			m_syncEndRotation = l_syncRotation;
		}
	}
	
	void Update()
	{
		if(!m_photonView.isMine)
		{
			m_syncTime += Time.deltaTime;
			transform.position = Vector3.Lerp (m_syncStartPosition, m_syncEndPosition, m_syncTime / m_syncDelay);
			transform.rotation = Quaternion.Lerp (m_syncStartRotation, m_syncEndRotation, m_syncTime / m_syncDelay);
		}
	}
}