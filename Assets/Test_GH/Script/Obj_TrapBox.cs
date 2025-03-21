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
    public Sprite SafeBox;

    public float move_horinozontal = 3;
    public float move_vertical = 3;
    public float move_speed = 10;
    public float move_turnDelay = 0.5f;

    TrapState trapState;
    Vector3 Startpos;
    bool IsFight = false;


    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        IsFight = true;
        StartCoroutine(Trap());
    }

    IEnumerator Trap()
    {
        yield return new WaitForSeconds(move_turnDelay);
        while(IsFight)
        {
            Startpos = transform.position;

            trapState = TrapState.Right;
            yield return new WaitForSeconds(move_turnDelay);
            trapState = TrapState.Down;
            yield return new WaitForSeconds(move_turnDelay);
            trapState = TrapState.Left;
            yield return new WaitForSeconds(move_turnDelay);
            trapState = TrapState.Up;
            yield return new WaitForSeconds(move_turnDelay);
        }

        sprite.sprite = SafeBox;
    }

    void Update()
    {
        if(trapState == TrapState.Right)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector2(Startpos.x + move_horinozontal, transform.position.y), move_speed);
        }
        else if(trapState == TrapState.Down)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector2(transform.position.x, Startpos.y - move_vertical), move_speed);
        }
        else if(trapState == TrapState.Left)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector2(Startpos.x, transform.position.y), move_speed);
        }
        else if(trapState == TrapState.Up)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector2(transform.position.x, Startpos.y), move_speed);
        }
    }
}
