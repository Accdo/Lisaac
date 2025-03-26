using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using static RoomType;


public class Room : MonoBehaviour
{

    private const int WORM = 1;
    private const int GUT = 2;

    public GameObject[] doorObjects = null; // »óÇÏÁÂ¿ì 4°³ÀÇ ¹® ÇÁ¸®ÆÕ
    public GameObject[] spanwPos;
    public int[] enemyType;


    public void Setup(bool[] doorStates, RoomTypeEnum roomType)
    {
        for (int i = 0; i < doorObjects.Length; i++)
        {
            doorObjects[i].GetComponent<Door>().DoorTypeSelect(roomType);
            doorObjects[i].SetActive(doorStates[i]);
        }
    }

    public void DoorChange(int doorDire, RoomTypeEnum doorType)
    {
        doorObjects[doorDire].GetComponent<Door>().DoorTypeSelect(doorType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SpanwEnemy();
        }
    }
    private void SpanwEnemy()
    {
        for (int i = 0; i < spanwPos.Length; i++)
        {
            switch (enemyType[i])
            {
                case WORM:
                    SpawnManager.Instance.SpawnWorm(spanwPos[i].transform.position);
                    break;
                case GUT:
                    SpawnManager.Instance.SpawnGut(spanwPos[i].transform.position);
                    break;
            }
        }
    }

}
