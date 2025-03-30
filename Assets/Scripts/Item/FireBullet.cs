using System;
using System.Collections;
using UnityEngine;

public class FireBullet : PlayerBullet
{
    public float speed;
    public float life;

    void Start()
    {
        SoundManager.Instance.FireOn();

        StartCoroutine(DelayedFireOff(life));
        Destroy(gameObject, life);
    }

    IEnumerator DelayedFireOff(float DELAY)
    {
        yield return new WaitForSeconds(DELAY);
        SoundManager.Instance.FireOff();
    }

    public void Create(int DAMAGE, float SPEED, Vector3 DIRECTION, float LIFE)
    {
        damage = DAMAGE;
        speed = SPEED;
        Direction = DIRECTION;
        life = LIFE;
    }

    void Update()
    {
        transform.position += Direction * (speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Enemy"))
        {
            if (!collision.name.Contains("Trap"))
            {
                SoundManager.Instance.FireOff();
                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("Boss") || collision.CompareTag("ExoMech"))
        {
            SoundManager.Instance.FireOff();
            SoundManager.Instance.HitBoss();
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        SoundManager.Instance.FireOff();
        Destroy(gameObject);
    }
}
