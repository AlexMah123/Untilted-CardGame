using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public enum Scene
{
    MENU,
    GAMESCENE
}


public static class SceneLoader
{
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene((int)scene);

        if(TimeManager.isTimePaused)
        {
            TimeManager.ResumeTime();
        }
    }
}

