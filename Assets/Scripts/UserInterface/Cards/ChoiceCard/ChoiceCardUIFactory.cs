using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public struct ChoiceCardCreationInfo
{
    public ChoiceCardCreationInfo(GameChoice choice, Transform spawnParent)
    {
        gameChoice = choice;
        parent = spawnParent;
    }

    public GameChoice gameChoice;
    public Transform parent;
}

public class ChoiceCardUIFactory : MonoBehaviour
{
    public GameObject choiceCardUIPrefab;
    public Sprite rockChoice;
    public Sprite paperChoice;
    public Sprite scissorChoice;

    public GameObject CreateCard(ChoiceCardCreationInfo creationInfo)
    {
        GameObject cardUIGO = Instantiate(choiceCardUIPrefab, creationInfo.parent);
        Image imageComponent = cardUIGO.GetComponent<Image>();

        switch (creationInfo.gameChoice)
        {
            case GameChoice.ROCK:
                imageComponent.sprite = rockChoice;

                break;

            case GameChoice.PAPER:
                imageComponent.sprite = paperChoice;
                break;

            case GameChoice.SCISSOR:
                imageComponent.sprite = scissorChoice;
                break;
        }

        return cardUIGO;
    }
}
