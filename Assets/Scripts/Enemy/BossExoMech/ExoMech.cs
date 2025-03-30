using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ExoMech : MonoBehaviour
{
    // 보스 프리팹
    public GameObject apolloPrefab;
    public GameObject artemisPrefab;
    /*public GameObject ares;*/
    public GameObject endingPortal;

    // 보스 오브젝트
    private GameObject apollo;
    private GameObject artemis;
    /*private GameObject ares;*/

    // 스테이터스
    public EnemyHp enemyHp = null;
    private bool phase2ApolloArtemis;

    // 보스 소환 여백
    private Vector3 apolloSpacing;
    private Vector3 artemisSpacing;

    // 오디오소스
    private AudioSource mainCamAudio;

    [Header("하위 컴포넌트")]
    [SerializeField] private RawImage intro;
    [SerializeField] private Image healthFill;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private AudioSource exoMechBgm;


    void Start()
    {
        mainCamAudio = Camera.main.GetComponent<AudioSource>();
        enemyHp = GetComponent<EnemyHp>();

        Vector3 camPosition = Camera.main.transform.position;
        apolloSpacing = new Vector3(camPosition.x + 7, Camera.main.transform.position.y, 0);
        artemisSpacing = new Vector3(camPosition.x - 7, Camera.main.transform.position.y, 0);

        phase2ApolloArtemis = false;

        // 배경 음악 중지
        mainCamAudio.Stop();

        StartCoroutine(SpawnApolloAndArtemis());
    }

    private void Update()
    {
        healthFill.fillAmount = (float)enemyHp.currentHp / enemyHp.maxHp;
    }

    IEnumerator SpawnApolloAndArtemis()
    {
        // 시간 멈춘후 연출
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(Mathf.Max((float)video.length, 2.3f));
        intro.gameObject.SetActive(false);
        Time.timeScale = 1;
        yield return new WaitForSeconds(.2f);

        // 배경음악 재생
        exoMechBgm.Play();

        apollo = Instantiate(apolloPrefab, apolloSpacing, Quaternion.identity);
        artemis = Instantiate(artemisPrefab, artemisSpacing, Quaternion.identity);
    }

    public void ExoMechsOnHit(int damage)
    {
        enemyHp.TakeDamage(damage);

        if (enemyHp.currentHp <= enemyHp.maxHp / 2 && !phase2ApolloArtemis)
        {
            phase2ApolloArtemis = true;
            StartCoroutine(apollo.GetComponent<Apollo>().PhaseChange());
            StartCoroutine(artemis.GetComponent<Artemis>().PhaseChange());
        }

        if (enemyHp.currentHp <= 0)
        {
            Invoke("EndingPortalCreate", 2);
            Destroy(apollo);
            Destroy(artemis);
            SoundManager.Instance.ExoDeath();
            exoMechBgm.Stop();
        }
    }

    private void EndingPortalCreate() {
        Vector3 camPosition = Camera.main.transform.position;
        camPosition.z = 0;
        Instantiate(endingPortal, camPosition, Quaternion.identity);
        Destroy(gameObject);
    }
}
