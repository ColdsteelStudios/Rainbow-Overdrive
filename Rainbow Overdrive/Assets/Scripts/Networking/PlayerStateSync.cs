// ---------------------------------------------------------------------------
// PlayerStateSync.cs
// 
// Class description goes here
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class PlayerStateSync : MonoBehaviour 
{
	private float m_lastSynchronizationTime = 0f;
	private float m_syncDelay = 0f;
	private float m_syncTime = 0f;
	private Vector3 m_syncStartPosition = Vector3.zero;
	private Vector3 m_syncEndPosition = Vector3.zero;
	private Quaternion m_syncStartRotation = Quaternion.identity;
	private Quaternion m_syncEndRotation = Quaternion.identity;
	public GameObject m_bikeChild;
	public GameObject m_lightChild;
	private Quaternion m_syncBikeStart = Quaternion.identity;
	private Quaternion m_syncLightStart = Quaternion.identity;
	private Quaternion m_syncBikeEnd = Quaternion.identity;
	private Quaternion m_syncLightEnd = Quaternion.identity;
	private PhotonView m_photonView;

	void Start()
	{
		m_photonView = gameObject.GetComponent<PhotonView> ();
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Vector3 l_syncPosition = Vector3.zero;
		Quaternion l_syncRotation = Quaternion.identity;
		Quaternion l_syncBike = Quaternion.identity;
		Quaternion l_syncLight = Quaternion.identity;

		if(stream.isWriting)
		{
			//We own this player, send others our data
			l_syncPosition = transform.position;
			l_syncRotation = transform.rotation;
			l_syncBike = m_bikeChild.transform.rotation;
			l_syncLight = m_lightChild.transform.rotation;
			stream.SendNext(l_syncPosition);
			stream.SendNext(l_syncRotation);
			stream.SendNext (l_syncBike);
			stream.SendNext (l_syncLight);
		}
		else
		{
			//Network player, receive data
			l_syncPosition = (Vector3)stream.ReceiveNext();
			l_syncRotation = (Quaternion)stream.ReceiveNext();
			l_syncBike = (Quaternion)stream.ReceiveNext();
			l_syncLight = (Quaternion)stream.ReceiveNext();

			m_syncTime = 0f;
			m_syncDelay = Time.time - m_lastSynchronizationTime;
			m_lastSynchronizationTime = Time.time;

			m_syncStartPosition = transform.position;
			m_syncEndPosition = l_syncPosition;
			m_syncStartRotation = transform.rotation;
			m_syncEndRotation = l_syncRotation;
			m_syncBikeStart = m_bikeChild.transform.rotation;
			m_syncBikeEnd = l_syncBike;
			m_syncLightStart = m_lightChild.transform.rotation;
			m_syncLightEnd = l_syncLight;
		}
	}

	void Update()
	{
		if(!m_photonView.isMine)
		{
			m_syncTime += Time.deltaTime;
			transform.position = Vector3.Lerp (m_syncStartPosition, m_syncEndPosition, m_syncTime / m_syncDelay);
			transform.rotation = Quaternion.Lerp (m_syncStartRotation, m_syncEndRotation, m_syncTime / m_syncDelay);
			m_bikeChild.transform.rotation = Quaternion.Lerp (m_syncBikeStart, m_syncBikeEnd, m_syncTime / m_syncDelay);
			m_lightChild.transform.rotation = Quaternion.Lerp (m_syncLightStart, m_syncLightEnd, m_syncTime / m_syncDelay);
		}
	}
}