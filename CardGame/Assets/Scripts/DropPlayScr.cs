using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public enum FieldType
{
    SELF_HAND,
    SELF_FIELD,
    ENEMY_HAND,
    ENEMY_FIELD
}

public class DropPlayScr : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FieldType Type;


    public void OnDrop(PointerEventData eventData)
    {
        if (Type != FieldType.SELF_FIELD)
            return;

        CardMovementsScr card = eventData.pointerDrag.GetComponent<CardMovementsScr>();

        if (card && card.GameManeger.PlayerFieldCards.Count < 6 &&
            card.GameManeger.IsPlayerTurn && card.GameManeger.PlayerMana >=
            card.GetComponent<CardInfoScr>().SelfCard.Manacost)
        {
            card.GameManeger.PlayerHandCards.Remove(card.GetComponent<CardInfoScr>());
            card.GameManeger.PlayerFieldCards.Add(card.GetComponent<CardInfoScr>()); 
            card.DefaultParent = transform;

            card.GameManeger.ReduceMana(true, card.GetComponent<CardInfoScr>().SelfCard.Manacost);
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || Type == FieldType.ENEMY_FIELD ||
            Type == FieldType.ENEMY_HAND || Type == FieldType.SELF_HAND)
            return;


        CardMovementsScr card = eventData.pointerDrag.GetComponent<CardMovementsScr>();

        if (card)
            card.DefaultTempCardParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;


        CardMovementsScr card = eventData.pointerDrag.GetComponent<CardMovementsScr>();

        if (card && card.DefaultTempCardParent == transform)
            card.DefaultTempCardParent = card.DefaultParent;
    }
}

