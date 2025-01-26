using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField, Tooltip("All scenes will be loaded additive")]
    private SceneField[] scenes;
    [SerializeField]
    private bool loadOnAwake = true;

    private void Awake()
    {
        if (!loadOnAwake)
        {
            return;
        }

        LoadScenes();
    }

    public void LoadScenes()
    {
        LoadScenes(scenes);
    }

    public async void LoadScenes(SceneField[] scenes)
    {
        var tasks = new AsyncOperation[scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            tasks[i] = SceneManager.LoadSceneAsync(scenes[i], LoadSceneMode.Additive);
        }

        bool done;
        while (true)
        {
            done = true;
            for (int i = 0; i < scenes.Length; i++)
            {
                if (!tasks[i].isDone)
                {
                    done = false;
                    break;
                }
            }

            if (done)
            {
                break;
            }

            await Task.Delay(100);
        }
    }

    public void UnloadScenes(SceneField scene)
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            SceneManager.UnloadSceneAsync(scenes[i]);
        }
    }
}