using UnityEngine;

public class BoomEff : MonoBehaviour
{
    void EndTrigger()
    {
        SoundManager.Instance.AresNukeExplosion();
        Destroy(gameObject);
    }
}
