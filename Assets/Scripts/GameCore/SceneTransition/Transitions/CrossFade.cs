using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace GameCore.SceneTransition.Transitions
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CrossFade : Base.SceneTransition
    {
        [SerializeField] CanvasGroup crossFade;
        [SerializeField] float durationOfTransition = 1f;

        public override IEnumerator AnimateTransitionIn()
        {
            crossFade.blocksRaycasts = true;

            var tweener = crossFade.DOFade(1f, durationOfTransition);
            yield return tweener.WaitForCompletion();
        }

        public override IEnumerator AnimateTransitionOut()
        {
            crossFade.blocksRaycasts = false;

            var tweener = crossFade.DOFade(0f, durationOfTransition);
            yield return tweener.WaitForCompletion();
        }
    }
}
