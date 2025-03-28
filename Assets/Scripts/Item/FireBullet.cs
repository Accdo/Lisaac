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

    void Update()
    {
        transform.Translate(Direction * (speed * Time.deltaTime));
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Enemy"))
        {
            if (!collision.name.Contains("Trap"))
            {
                SoundManager.Instance.FireOff();
                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("Boss"))
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
