using UnityEngine;

public class ShotItem : MonoBehaviour
{
    float RandXpos, RandYpos; // Random Position
    
    void Start()
    {
        RandXpos = Random.Range(-1f, 1f);
        RandYpos = Random.Range(-1f, -1.5f);

        RandXpos += transform.position.x;
        RandYpos += transform.position.y;

        Destroy(this, 2.5f);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(RandXpos, RandYpos, 0), Time.deltaTime);
    }
}
