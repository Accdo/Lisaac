using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerGold playerGold;

    void Start()
    {
        playerGold = FindAnyObjectByType<PlayerGold>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            playerGold.AddGold(100);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                EnemyHp enemyHp = go.GetComponent<EnemyHp>();
                if (enemyHp != null)
                    enemyHp.TakeDamage(enemyHp.currentHp);
            }

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Boss"))
            {
                EnemyHp bossHp = go.GetComponent<EnemyHp>();
                if (bossHp != null)
                    bossHp.currentHp = 1;
            }
        }
    }
}
