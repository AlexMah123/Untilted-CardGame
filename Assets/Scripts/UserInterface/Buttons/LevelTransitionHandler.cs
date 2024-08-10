using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelTransitionHandler : MonoBehaviour
{
    public Scene sceneToTransition;
    public Transition transitionType;

    public void LoadLevel()
    {
        var audioClip = SFXManager.Instance.sfxLibrary.GetAudioClip("ButtonClick");
        SFXManager.Instance.PlaySoundFXClip(audioClip, transform);

        StartCoroutine(HandleLoadScene(audioClip.length));
    }

    private IEnumerator HandleLoadScene(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        SceneTransitionManager.Instance.LoadScene(sceneToTransition, transitionType);
    }
}
