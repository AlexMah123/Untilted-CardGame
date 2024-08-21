using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CrossFade : SceneTransition
{
    [SerializeField] CanvasGroup crossFade;

    public override IEnumerator AnimateTransitionIn()
    {
        crossFade.blocksRaycasts = true;

        var tweener = crossFade.DOFade(1f, 1f);
        yield return tweener.WaitForCompletion();
    }

    public override IEnumerator AnimateTransitionOut()
    {
        crossFade.blocksRaycasts = false;

        var tweener = crossFade.DOFade(0f, 1f);
        yield return tweener.WaitForCompletion();
    }
}
