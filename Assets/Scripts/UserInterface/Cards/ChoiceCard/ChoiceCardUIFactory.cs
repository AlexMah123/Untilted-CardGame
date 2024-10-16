using PlayerCore.PlayerComponents;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.Cards.ChoiceCard
{
    public struct FChoiceCardCreation
    {
        public FChoiceCardCreation(GameChoice choice, Transform spawnParent)
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

        public GameObject CreateCard(FChoiceCardCreation creation)
        {
            GameObject cardUIObj = Instantiate(choiceCardUIPrefab, creation.parent);
            Image imageComponent = cardUIObj.GetComponent<Image>();

            switch (creation.gameChoice)
            {
                case GameChoice.Rock:
                    imageComponent.sprite = rockChoice;

                    break;

                case GameChoice.Paper:
                    imageComponent.sprite = paperChoice;
                    break;

                case GameChoice.Scissor:
                    imageComponent.sprite = scissorChoice;
                    break;
            }

            return cardUIObj;
        }
    }
}