using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LoadoutCardFactory))]
public class LoadoutLayoutManager : MonoBehaviour
{
    public static LoadoutLayoutManager Instance;

    [Header("Layout Config")]
    public Transform spawnContainer;
    public float radiusFromCenter = 5f; 
    public float angleBetweenCards = 10f; // temp
    [Range(1, 9)] public int displayAmount;

    [Header("Layout Offset Config")]
    [Tooltip("This value is relative to World Position")] public Vector3 offsetPosition;

    //private
    private List<LoadoutCardGO> cardSlots = new();

    //factory
    private LoadoutCardFactory upgradeCardFactory;

    //event
    public event Action OnLoadoutSelectedEvent;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        upgradeCardFactory = GetComponent<LoadoutCardFactory>();
    }

    private void Start()
    {
        InitialiseLayout();
        ArrangeCardsInArc();
    }

    #region public methods
    public void HandleOnCardEndInteract()
    {
        ArrangeCardsInArc();
    }

    public void HandleOnCardSelected()
    {
        //#TODO: supose to be binded to addselected...
        OnLoadoutSelectedEvent?.Invoke();
        LoadoutManager.Instance.AddSelectedUpgradeToActive();

        UpdateCardSlots();
        UpdateActiveCardSlots();
    }

    #endregion

    #region internal methods
    private void InitialiseLayout()
    {
        cardSlots.Clear();

        for(int i = 0; i < displayAmount; i++)
        {
            LoadoutCardCreationInfo creationInfo = new(spawnContainer);
            GameObject card = upgradeCardFactory.CreateUpgradeCard(creationInfo);
            LoadoutCardGO upgradeCard = card.GetComponent<LoadoutCardGO>();
            cardSlots.Add(upgradeCard);
        }

        BindOnCardEndInteractEvent(GetCardSlots());
        BindOnCardSelectedEvent(GetCardSlots());
    }

    private void ArrangeCardsInArc()
    {
        int cardCount = cardSlots.Count;

        // Calculate the starting angle
        float totalAngle = (cardCount - 1) * angleBetweenCards;
        float startAngle = -totalAngle / 2;

        for (int i = 0; i < cardCount; i++)
        {
            float angle = startAngle + i * angleBetweenCards;
            float radian = angle * Mathf.Deg2Rad;

            // Calculate the card's position
            float x = spawnContainer.position.x + Mathf.Cos(radian) * radiusFromCenter;
            float y = spawnContainer.position.y + Mathf.Sin(radian) * radiusFromCenter;

            //adjust position
            cardSlots[i].gameObject.transform.localPosition = new Vector3(x, y, 0);
            cardSlots[i].gameObject.transform.position += new Vector3(offsetPosition.x, offsetPosition.y, offsetPosition.z);

            //rotate cards outwards
            cardSlots[i].gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle + 90);
            cardSlots[i].GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }

    private List<LoadoutCardGO> GetCardSlots()
    {
        return cardSlots;
    }

    private void UpdateCardSlots()
    {
        //#TODO: have the card slots be updatable.
        Debug.Log("Updating Card Slots");
    }

    private void UpdateActiveCardSlots()
    {
        //#TODO: have the active card slots be updatable.
        Debug.Log("Updating Active Card Slots");
    }
    #endregion

    #region Bind LoadoutCard Event
    public void BindOnCardEndInteractEvent(List<LoadoutCardGO> cardSlots)
    {
        foreach (var card in cardSlots)
        {
            card.OnCardEndInteractEvent += HandleOnCardEndInteract;
        }
    }

    public void UnbindOnCardEndInteractEvent(List<LoadoutCardGO> cardSlots)
    {
        foreach (var card in cardSlots)
        {
            card.OnCardEndInteractEvent -= HandleOnCardEndInteract;
        }
    }

    public void BindOnCardSelectedEvent(List<LoadoutCardGO> cardSlots)
    {
        foreach (var card in cardSlots)
        {
            card.OnCardSelectedEvent += HandleOnCardSelected;
        }
    }

    public void UnbindOnCardSelectedEvent(List<LoadoutCardGO> cardSlots)
    {
        foreach (var card in cardSlots)
        {
            card.OnCardSelectedEvent -= HandleOnCardSelected;
        }
    }


    #endregion
}
