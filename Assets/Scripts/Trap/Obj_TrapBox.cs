using System.Collections;
using UnityEngine;

enum TrapState
{
    Right = 1,
    Down,
    Left,
    Up
}

public class Obj_TrapBox : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rigid;
    public Sprite SafeBox;

    public float move_horinozontal = 3;
    public float move_vertical = 3;
    public float move_speed = 10;
    public float move_turnDelay = 1f;

    [SerializeField]
    private TrapState startTrapState;

    TrapState trapState;
    Vector3 Startpos;
    public bool IsFight = false;
    private bool TrapOn = true;
    private Room room;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        room = GetComponentInParent<Room>();

        IsFight = true;
    }

    IEnumerator Trap()
    {
        yield return new WaitForSeconds(move_turnDelay);
        Debug.Log($"{gameObject.name} 코루틴 안에 들어옴!");
        while(IsFight)
        {
            Startpos = transform.position;

            for(int i = (int)startTrapState; i <= 4; i++)
            {
                trapState = (TrapState)i;
                yield return new WaitForSeconds(move_turnDelay);
            }
            for(int i = 1; i < (int)startTrapState; i++)
            {
                trapState = (TrapState)i;
                yield return new WaitForSeconds(move_turnDelay);
            }
        }
        
        sprite.sprite = SafeBox;
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void Update()
    {
        if (room.isInPlayer)
        {
            if (TrapOn)
            {
                StartCoroutine(Trap());
                TrapOn = false;
            }

            if (RoomManager.Instance.nonMonster)
            {
                gameObject.tag = "Wall";
                IsFight = false;
            }
            else
            {
                // 충돌 감지 및 방향 전환
                DetectWallAndChangeDirection();
                MoveTrap();
            }
        }
    }

    void MoveTrap()
    {
        Vector3 moveDirection = Vector3.zero;

        switch (trapState)
        {
            case TrapState.Right:
                moveDirection = Vector3.right;
                break;
            case TrapState.Down:
                moveDirection = Vector3.down;
                break;
            case TrapState.Left:
                moveDirection = Vector3.left;
                break;
            case TrapState.Up:
                moveDirection = Vector3.up;
                break;
        }

        transform.position += moveDirection * move_speed * Time.deltaTime;
    }

    void DetectWallAndChangeDirection()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, GetDirectionVector(), 0.5f, LayerMask.GetMask("Wall"));
        if (hit.collider != null)
        {
            // 방향 전환
            switch (trapState)
            {
                case TrapState.Right:
                    trapState = TrapState.Down;
                    break;
                case TrapState.Down:
                    trapState = TrapState.Left;
                    break;
                case TrapState.Left:
                    trapState = TrapState.Up;
                    break;
                case TrapState.Up:
                    trapState = TrapState.Right;
                    break;
            }
        }
    }

    Vector3 GetDirectionVector()
    {
        switch (trapState)
        {
            case TrapState.Right: return Vector3.right;
            case TrapState.Down: return Vector3.down;
            case TrapState.Left: return Vector3.left;
            case TrapState.Up: return Vector3.up;
            default: return Vector3.zero;
        }
    }

}
