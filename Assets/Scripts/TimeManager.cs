using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private TimeManager()
    {

    }

    public static bool isTimePaused = false;

    public static void PauseTime()
    {
        isTimePaused = true;
        Time.timeScale = 0;

        //#DEBUG
        Debug.Log("TimeisPaused");
    }

    public static void ResumeTime()
    {
        isTimePaused = false;
        Time.timeScale = 1;

        //#DEBUG
        Debug.Log("TimeisUnpaused");

    }
}
