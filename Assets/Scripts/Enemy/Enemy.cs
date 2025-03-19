using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float attackSpeed = 2f;
    public GameObject bullet;
    public Transform pos;
    public float fireDelay = 2;



    void Start()
    {
        StartCoroutine(FireBust());
    }
    void Update()
    {

    }

    IEnumerator FireBust()
    {
        while (true)
        {
            EnemyFire();
            yield return new WaitForSeconds(fireDelay);
        }


    }
    void EnemyFire()
    {
        Instantiate(bullet, pos.position, Quaternion.identity);
    }

}
