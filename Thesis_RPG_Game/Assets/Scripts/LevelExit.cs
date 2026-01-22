using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public int nextSceneIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var stats = collision.GetComponent<PlayerStats>();
            var lvl = collision.GetComponent<Player_Lvl>();
            var combat = collision.GetComponent<Player_Combat>();

            GameSession.Instance.SavePlayer(stats, lvl, combat);

            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}