using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public enum SCENE
{
    EXIT = -1,
    MENU,
    GAMESCENE
}


public static class SceneLoader
{
    public static void Load(SCENE scene)
    {
        if (TimeManager.isTimePaused)
        {
            TimeManager.ResumeTime();
        }

        switch (scene)
        {
            case SCENE.EXIT:
                Application.Quit();

                //#DEBUG
                Debug.Log("Quiting");
                break;

            default:
                SceneManager.LoadScene((int)scene);
                break;
        }
    }
}

