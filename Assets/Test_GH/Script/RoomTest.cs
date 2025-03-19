using System;
using UnityEngine;

public class RoomTest : MonoBehaviour
{
    public String Dir = "Right";

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            RoomTestManager.Instance.NextRoom(Dir);
        }
    }
}
