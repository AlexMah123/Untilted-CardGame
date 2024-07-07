using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceCardUIFactory : MonoBehaviour
{
    public GameObject cardUIPrefab;
    public Sprite rockChoice;
    public Sprite paperChoice;
    public Sprite scissorChoice;


    public GameObject CreateCard(GameChoice gameChoice, Transform parent)
    {
        GameObject cardUIGO = Instantiate(cardUIPrefab, transform);
        Image imageComponent = cardUIGO.GetComponent<Image>();

        switch (gameChoice)
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
