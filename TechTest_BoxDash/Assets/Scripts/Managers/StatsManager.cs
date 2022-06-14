using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : NetworkBehaviour
{
    public float timeToRestart = 5f;
    public int scoreForWin = 3;
    
    public GameObject winPanel;
    private Text _winText;

    [SerializeField]
    private List<GameObject> player = new List<GameObject>();

    #region Singleton
    
    private static StatsManager _instance;

    public static StatsManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

        _winText = winPanel.GetComponentInChildren<Text>();
    }

    #endregion

    public void CheckWinPlayer(int nowScore, GameObject namePlayer)
    {
        if (nowScore >= scoreForWin)
        {
            StartCoroutine(Respawn(namePlayer));
        }
    }
    
    IEnumerator Respawn(GameObject namePlayer)
    {
        winPanel.SetActive(true);
        _winText.text = "Победил - " + namePlayer.name;

        yield return new WaitForSeconds(timeToRestart);
        
        for (int i = 0; i < player.Count; i++)
        {
            Transform newPos = NetworkManager.singleton.GetStartPosition();
            player[i].GetComponent<PlayerStats>().Respawn(newPos);
            
            yield return new WaitForSeconds(0.2f);
        }
        
        winPanel.SetActive(false);
    }

    public void AddToListPlayer(GameObject obj)
    {
        player.Add(obj);
    }

    public void RemoveFromListPlayer(GameObject obj)
    {
        player.Remove(obj);
    }

    public int CountPlayersList()
    {
        return player.Count;
    }
}
