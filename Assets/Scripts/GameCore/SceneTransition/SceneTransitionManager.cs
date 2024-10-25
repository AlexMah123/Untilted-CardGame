using System;
using System.Collections;
using System.Linq;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.SceneTransition
{
    [Serializable]
    public enum SceneType
    {
        Exit = -1,
        Title,
        Menu,
        LevelSelect,
        Game,
        Loadout,
        Reward
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

        private Transitions.Base.SceneTransition[] transitions;
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
            transitions = transitionsContainer.GetComponentsInChildren<Transitions.Base.SceneTransition>();
        }

        public void LoadScene(SceneType scene, Transition transitionType, bool isAdditive = false)
        {
            //early return to prevent spamming
            if (isTransitioning) return;

            SFXManager.Instance.PlaySoundFXClip("SceneTransition", transform);
            StartCoroutine(LoadSceneAsync(scene, transitionType, isAdditive));
        }

        private IEnumerator LoadSceneAsync(SceneType sceneType, Transition transitionType, bool isAdditive)
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
            Transitions.Base.SceneTransition transition = transitions.First(t => t.transitionType == transitionType);
            AsyncOperation scene;

            if (isAdditive)
            {
                scene = SceneManager.LoadSceneAsync((int)sceneType, LoadSceneMode.Additive);
            }
            else
            {
                //load scene normally
                scene = SceneManager.LoadSceneAsync((int)sceneType);
            }

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
}