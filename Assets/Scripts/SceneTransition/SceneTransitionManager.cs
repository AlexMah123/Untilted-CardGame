using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public enum SceneType
{
    Exit = -1,
    Menu,
    LevelSelect,
    Game,
    Loadout,
}

public enum Transition
{
    CrossFade,
    CircleWipe,
}

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public GameObject transitionsContainer;

    private SceneTransition[] transitions;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance != null && Instance == this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>();
    }

    public void LoadScene(SceneType scene, Transition transitionType)
    {
        //early return to prevent spamming
        if (isTransitioning) return;

        SFXManager.Instance.PlaySoundFXClip("SceneTransition", transform);
        StartCoroutine(LoadSceneAsync(scene, transitionType));
    }

    private IEnumerator LoadSceneAsync(SceneType sceneType, Transition transitionType)
    {
        //set flag to true
        isTransitioning = true;

        if (TimeManager.isTimePaused)
        {
            TimeManager.ResumeTime();
        }

        if (sceneType == SceneType.Exit)
        {
            Application.Quit();

            //#DEBUG
            Debug.Log("Quitting Game");

            //early reset
            isTransitioning = false;
            yield break;
        }

        //get the correct sceneObject
        SceneTransition transition = transitions.First(t => t.transitionType == transitionType);

        //load scene
        AsyncOperation scene = SceneManager.LoadSceneAsync((int)sceneType);
        scene.allowSceneActivation = false;

        //play animation to transition into new scene
        yield return transition.AnimateTransitionIn();

        //#TODO: Add progressbar?

        scene.allowSceneActivation = true;

        //play animation to transition out of new scene
        yield return transition.AnimateTransitionOut();

        //reset flag
        isTransitioning = false;
    }
}
