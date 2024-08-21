using System.Collections;
using UnityEngine;

public class SceneTransitionHandler : MonoBehaviour
{
    public SceneType sceneToTransition;
    public Transition transitionType;

    private Coroutine coroutine;

    public void LoadScene()
    {
        if (coroutine != null) return;

        var audioClip = SFXManager.Instance.sfxLibrary.GetAudioClip("ButtonClick");
        SFXManager.Instance.PlaySoundFXClip(audioClip, transform);

        coroutine = StartCoroutine(HandleLoadScene(audioClip.length));
    }

    private IEnumerator HandleLoadScene(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        SceneTransitionManager.Instance.LoadScene(sceneToTransition, transitionType);
        coroutine = null;
    }
}
