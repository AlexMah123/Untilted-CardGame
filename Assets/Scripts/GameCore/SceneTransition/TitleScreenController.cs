using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UserInterface.Buttons;

namespace GameCore.SceneTransition
{
    public class TitleScreenController : MonoBehaviour
    {
        private bool hasStarted = false;
        private SceneTransitionHandler handler;

        private void Awake()
        {
            handler = GetComponent<SceneTransitionHandler>();

            if (handler == null)
            {
                Debug.LogError("There is no scene transition handler");
            }
        }

        void Update()
        {
            if (hasStarted) return;

            if (Input.anyKeyDown)
            {
                hasStarted = true;
                handler.LoadScene();
            }
        }
    }
}
