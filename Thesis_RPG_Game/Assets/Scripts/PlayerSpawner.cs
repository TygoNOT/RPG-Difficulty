using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
        }

        if (GameSession.Instance != null)
        {
            GameSession.Instance.SaveCheckpoint();
        }

    }
}
