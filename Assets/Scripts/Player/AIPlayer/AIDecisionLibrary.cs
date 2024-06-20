using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIDecisionLibrary 
{


    #region Helper
    public static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }
    #endregion
}
