using System.Collections;
using UnityEngine;

public class FriendPooter : MonoBehaviour
{
    private Animator animator;

    // Shot Bullet
    public GameObject BlueBullet;
    public Transform AttackPos;

    [SerializeField] bool IsCrossShot = true;
    Vector3[] dirArr;
    Vector3[] dirCross = {  Vector3.up, Vector3.down, Vector3.left, Vector3.right   };
    Vector3[] dirDiagonal = {  (Vector3.up + Vector3.right).normalized, (Vector3.down + Vector3.left).normalized, 
                            (Vector3.up + Vector3.left).normalized, (Vector3.down + Vector3.right).normalized   };

    //Round
    Transform player_transform;
    public float rotateSpeed = 50f;

    private bool isRound = false;

    // WingDelay
    public float wingDuration = 3.0f;
    public float shotDelay = 2.0f;

    void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(SoundWing());
    }

    void Update()
    {
        if(isRound)
        {
            transform.RotateAround(player_transform.position, Vector3.forward, rotateSpeed * Time.deltaTime);

            transform.rotation = Quaternion.identity;
        }
    }

    IEnumerator SoundWing()
    {
        while(true){
            SoundManager.Instance.Fly_Buzz();
            yield return new WaitForSeconds(wingDuration);
        }
    }

    IEnumerator ShotStart()
    {
        yield return new WaitForSeconds(3.0f);
        while(true){
            animator.SetTrigger("Attack");
            
            yield return new WaitForSeconds(shotDelay);
        }
    }

    public void Attack()
    {
        GameObject bullet;

        if(IsCrossShot)
            dirArr = dirCross;
        else
            dirArr = dirDiagonal;


        foreach(var dir in dirArr)
        {
            bullet = Instantiate(BlueBullet, AttackPos.position, Quaternion.identity);
            bullet.GetComponent<PlayerBullet>().Direction = dir;
        }

        IsCrossShot = !IsCrossShot;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isRound)
        {
            player_transform = collision.transform;
            transform.SetParent(player_transform);
            
            StartCoroutine(ShotStart());
            isRound = true;
        }
    }
}