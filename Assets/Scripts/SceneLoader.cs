using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    void Start()
    {

    }


    void Update()
    {

    }

    public void OnClickScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
