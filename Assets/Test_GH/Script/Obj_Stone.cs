using System.Collections;
using UnityEngine;

public class Obj_Stone : MonoBehaviour
{
    public GameObject Stone_Bullet;
    public Transform Shot_Pos;
    Animator anim;

    bool IsFight = false;

    void Start()
    {
        anim = GetComponent<Animator>();

        IsFight = true;
        StartCoroutine(Fighting());
    }
    IEnumerator Fighting()
    {
        yield return new WaitForSeconds(1.0f);
        while(IsFight)
        {
            Attack();
            yield return new WaitForSeconds(2.0f);
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
    }

    public void Shot()
    {
        Instantiate(Stone_Bullet, Shot_Pos.position, Quaternion.identity);
    }
}
