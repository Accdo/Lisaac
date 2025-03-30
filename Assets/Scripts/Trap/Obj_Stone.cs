using System.Collections;
using UnityEngine;

public class Obj_Stone : MonoBehaviour
{
    public GameObject Stone_Bullet;
    public Transform Shot_Pos;
    Animator anim;

    bool IsFight = false;
    private bool TrapOn = true;
    private Room room;

    void Start()
    {
        anim = GetComponent<Animator>();
        room = GetComponentInParent<Room>();

        IsFight = true;
    }

    private void Update() 
    {
        if (room.isInPlayer)
        {
            if (TrapOn)
            {
                StartCoroutine(Fighting());
                TrapOn = false;
            }
        }
    }

    IEnumerator Fighting()
    {
        yield return new WaitForSeconds(1.0f);
        while(IsFight)
        {
            Attack();
            yield return new WaitForSeconds(2.0f);
            if(RoomManager.Instance.nonMonster)
            {
                gameObject.tag = "Wall";
                IsFight = false;
            }
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
