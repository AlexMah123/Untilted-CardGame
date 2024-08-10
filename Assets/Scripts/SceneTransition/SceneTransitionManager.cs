using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public enum Scene
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

    private void Awake()
    {
        if(Instance != null && Instance == this)
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

    public void LoadScene(Scene scene, Transition transitionType)
    {
        SFXManager.Instance.PlaySoundFXClip("SceneTransition", transform);
        StartCoroutine(LoadSceneAsync(scene, transitionType));
    }

    private IEnumerator LoadSceneAsync(Scene sceneType, Transition transitionType)
    {
        if (TimeManager.isTimePaused)
        {
            TimeManager.ResumeTime();
        }

        if(sceneType == Scene.Exit)
        {
            Application.Quit();

            //#DEBUG
            Debug.Log("Quitting Game");
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
    }
}
