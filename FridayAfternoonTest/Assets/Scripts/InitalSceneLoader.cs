using UnityEngine;
using UnityEngine.SceneManagement;

public class InitalSceneLoader : MonoBehaviour {
    public void LoadSceneCubes()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void LoadScenePlayground1()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
