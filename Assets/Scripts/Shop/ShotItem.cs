using UnityEngine;

public class ShotItem : MonoBehaviour
{
    float RandXpos, RandYpos; // Random Position
    private bool inPlayer;
    
    void Start()
    {
        RandXpos = Random.Range(-1f, 1f);
        RandYpos = Random.Range(-1f, -1.5f);

        RandXpos += transform.position.x;
        RandYpos += transform.position.y;

        inPlayer = false;

        Destroy(this, 2.5f);
    }

    void Update()
    {
        if (!inPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(RandXpos, RandYpos, 0), Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inPlayer = true;
            Debug.Log("Collision with Player detected. inPlayer set to true.");
        }
    }
}
