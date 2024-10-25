using System.Collections;
using UnityEngine;

namespace GameCore.SceneTransition.Transitions.Base
{
    public abstract class SceneTransition : MonoBehaviour
    {
        public Transition transitionType;

        public abstract IEnumerator AnimateTransitionIn();
        public abstract IEnumerator AnimateTransitionOut();
    }
}