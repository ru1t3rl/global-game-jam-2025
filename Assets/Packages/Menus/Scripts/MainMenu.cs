using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private SceneLoader sceneLoader;

    [SerializeField]
    private SceneField[] scenesToLoadOnPlay;
    
    public void PlayGame()
    {
        sceneLoader.LoadScenes(scenesToLoadOnPlay); 
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
