// ---------------------------------------------------------------------------
// PleaseDelete.cs
// 
// Class description goes here
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class PleaseDelete : MonoBehaviour 
{
    [RPC]
    public void PleaseDestroy()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}