using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ExoMech : MonoBehaviour
{

    public GameObject apolloPrefab;
    /*public GameObject artemis;
    public GameObject ares;*/

    private GameObject apollo;
    /*private GameObject artemis;
    private GameObject ares;*/

    public EnemyHp enemyHp = null;

    private Vector3 apolloSpacing;

    [Header("하위 객체")]
    [SerializeField] private RawImage intro;
    [SerializeField] private Image healthFill;

    private AudioSource mainCamAudio;
    [SerializeField] private VideoPlayer video;

    void Start()
    {
        mainCamAudio = Camera.main?.GetComponent<AudioSource>();

        enemyHp = GetComponent<EnemyHp>();
        apolloSpacing = new Vector3(7, 0, 0);
        StartCoroutine(SpawnApollo());
    }

    IEnumerator SpawnApollo()
    {
        // 시간 정지 후 인트로 재생
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(Mathf.Max((float)video.length, 0.1f));
        intro.gameObject.SetActive(false);
        Time.timeScale = 1;
        yield return new WaitForSeconds(.2f);
        mainCamAudio.enabled = true;

        apollo = Instantiate(apolloPrefab, transform.position + apolloSpacing, Quaternion.identity);

        Debug.Log("ㅁㅁㅁㅁ");
    }

    public void ExoMechsOnHit(int damage)
    {
        
        enemyHp.TakeDamage(damage);
        healthFill.fillAmount = (float)enemyHp.currentHp / enemyHp.maxHp;

        if (enemyHp.currentHp <= 0)
        {
            Destroy(apollo);
            Destroy(gameObject);
        }
    }
}
