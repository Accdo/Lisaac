using System.Collections;
using UnityEngine;

public class FriendPooter : MonoBehaviour
{
    public GameObject BlueBullet;

    //Round
    Transform player_transform;
    public float rotateSpeed = 50f;

    private bool isRound = false;

    // WingDelay
    public float wingDuration = 3.0f;
    public float shotDelay = 2.0f;

    void Start()
    {
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
        while(true){
            Instantiate(BlueBullet, transform.position, Quaternion.identity);
            
            yield return new WaitForSeconds(shotDelay);
        }
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