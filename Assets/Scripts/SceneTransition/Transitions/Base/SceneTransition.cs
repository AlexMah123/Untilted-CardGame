using System.Collections;
using UnityEngine;

public abstract class SceneTransition : MonoBehaviour
{
    public Transition transitionType;

    public abstract IEnumerator AnimateTransitionIn();
    public abstract IEnumerator AnimateTransitionOut();

}
