using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerGold playerGold;

    // Scene Reload Time
    private float holdTime = 0f;
    public float requiredHoldTime = 2f; 

    // FadeOut
    public CanvasGroup fadeCanvas; 
    public float fadeDuration = 1f;

    public GameObject PauseMenu;

    void Start()
    {
        playerGold = FindAnyObjectByType<PlayerGold>();

        
        StartCoroutine(FadeOut());
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

        if (Input.GetKey(KeyCode.R)) 
        {
            holdTime += Time.deltaTime; 
            if (holdTime >= requiredHoldTime) 
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            holdTime = 0f;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            PauseMenu.SetActive(true);
        }
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        fadeCanvas.alpha = 1f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvas.alpha = 1 - (timer / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = 0f; 
    }
}
