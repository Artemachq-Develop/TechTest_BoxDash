using System;
using Mirror;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    private int _score;

    private SetNamePlayer _setNamePlayer;

    private void Awake()
    {
        _setNamePlayer = GetComponent<SetNamePlayer>();
    }

    public void CheckScore(int addValue)
    {
        _score += addValue;
        UpdateScoreText();

        StatsManager.Instance.CheckWinPlayer(_score, this.gameObject);
    }

    public void UpdateScoreText()
    {
        _setNamePlayer.scoreTextMesh.SetText(_score.ToString());
    }

    public void Respawn(Transform pos)
    {
        transform.position = pos.position;
        transform.rotation = pos.rotation;

        _score = 0;
        UpdateScoreText();
    }
}
