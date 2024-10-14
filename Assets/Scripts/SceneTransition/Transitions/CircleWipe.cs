using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SceneTransition.Transitions
{
    public class CircleWipe : Base.SceneTransition
    {
        [SerializeField] CanvasGroup circleWipe;
        [SerializeField] Image circle;
        [SerializeField] float distance;
        [SerializeField] float durationOfTransition = 1f;

        public override IEnumerator AnimateTransitionIn()
        {
            circleWipe.blocksRaycasts = true;

            //resets to the left
            circle.rectTransform.anchoredPosition = new Vector2(-distance, 0f);

            var tweener = circle.rectTransform.DOAnchorPosX(0f, durationOfTransition);
            yield return tweener.WaitForCompletion();
        }

        public override IEnumerator AnimateTransitionOut()
        {
            circleWipe.blocksRaycasts = false;

            var tweener = circle.rectTransform.DOAnchorPosX(distance, durationOfTransition);
            yield return tweener.WaitForCompletion();
        }
    }
}
