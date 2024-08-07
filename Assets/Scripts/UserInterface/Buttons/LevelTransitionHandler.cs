using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionHandler : MonoBehaviour
{
    public SCENE sceneToTransition;

    public void LoadLevel()
    {
        HandleLoadScene();
    }

    private void HandleLoadScene()
    {
        SceneLoader.Load(sceneToTransition);
    }
}
