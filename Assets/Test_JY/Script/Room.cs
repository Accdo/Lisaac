using TMPro;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject[] doorObjects; // 상하좌우 4개의 문 프리팹

    public void Setup(bool[] doorStates)
    {
        for(int i = 0; i< doorObjects.Length; i++)
        {
            doorObjects[i].SetActive(doorStates[i]);
        }
    }
}
