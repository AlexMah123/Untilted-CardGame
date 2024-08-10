using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CircleWipe : SceneTransition
{
    [SerializeField] Image circle;
    [SerializeField] float distance;

    public override IEnumerator AnimateTransitionIn()
    {
        //resets to the left
        circle.rectTransform.anchoredPosition = new Vector2(-distance, 0f);

        var tweener = circle.rectTransform.DOAnchorPosX(0f, 1f);
        yield return tweener.WaitForCompletion();
    }

    public override IEnumerator AnimateTransitionOut()
    {

        var tweener = circle.rectTransform.DOAnchorPosX(distance, 1f);
        yield return tweener.WaitForCompletion();
    }
}
