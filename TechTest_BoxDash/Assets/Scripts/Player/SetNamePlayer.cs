using System;
using Mirror;
using UnityEngine;
using TMPro;

public class SetNamePlayer : NetworkBehaviour
{
    public string thisPlayerName;
    
    public TextMeshProUGUI scoreTextMesh;
    public TextMeshProUGUI nameTextMesh;
    
    private void Start()
    {
        nameTextMesh.SetText(thisPlayerName);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        SetName();
        CmdCheckName();
    }

    private void OnConnectedToServer()
    {
        RpcCheckName();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        DeleteName();
    }

    private void SetName()
    {
        if (isServer)
        {
            StatsManager.Instance.AddToListPlayer(this.gameObject);
            thisPlayerName = "Player" + StatsManager.Instance.CountPlayersList();
            transform.name = thisPlayerName;
        }
        else
        {
            StatsManager.Instance.AddToListPlayer(this.gameObject);
            thisPlayerName = "Player" + StatsManager.Instance.CountPlayersList();
            transform.name = thisPlayerName;
        }
    }

    private void DeleteName()
    {
        StatsManager.Instance.RemoveFromListPlayer(this.gameObject);
    }
    
    [ClientRpc]
    void RpcCheckName()
    {
        nameTextMesh.SetText(thisPlayerName);
    }
    
    [Command]
    void CmdCheckName()
    {
        RpcCheckName();    
    }
}
