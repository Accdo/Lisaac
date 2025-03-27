using UnityEngine;

public class WaitEffect : MonoBehaviour
{
    public void DestroyEffect()
    {
        Destroy(this.gameObject, 0.1f);
    }
}
