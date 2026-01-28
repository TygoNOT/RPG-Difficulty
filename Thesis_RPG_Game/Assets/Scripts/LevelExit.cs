using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public int nextSceneIndex;
    private Collider2D col;


    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
    }

    public void ActivateExit()
    {
        col.enabled = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENTER EXIT TRIGGER");

        if (collision.CompareTag("Player"))
            if (GameSession.Instance != null)
            {
                PlayerStats stats = collision.GetComponent<PlayerStats>();
                Player_Lvl lvl = collision.GetComponent<Player_Lvl>();
                Player_Combat combat = collision.GetComponent<Player_Combat>();

                GameSession.Instance.SavePlayer(stats, lvl, combat);
                GameSession.Instance.FinishLevel();
                SceneManager.LoadScene(nextSceneIndex);
            }
    }
}
