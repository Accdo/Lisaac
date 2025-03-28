using System.Collections;
using UnityEngine;

public class FriendPooter : MonoBehaviour
{
    public float wingDuration = 3.0f;

    void Start()
    {
        StartCoroutine(SoundWing());
    }

    IEnumerator SoundWing()
    {
        while(true){
            SoundManager.Instance.Fly_Buzz();
            yield return new WaitForSeconds(wingDuration);
        }
    }
}
