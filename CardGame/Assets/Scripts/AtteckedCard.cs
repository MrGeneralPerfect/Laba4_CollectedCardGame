using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class AtteckedCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (!GetComponent<CardMovementsScr>().GameManeger.IsPlayerTurn)
            return;


        CardInfoScr card = eventData.pointerDrag.GetComponent<CardInfoScr>();

        if (card &&
            card.SelfCard.CanAttack &&
            transform.parent == GetComponent<CardMovementsScr>().GameManeger.EnemyField)
        {
            card.SelfCard.ChangeAttackState(false);

            if(card.IsPlayer)
                card.DeHighlightCard();

            GetComponent<CardMovementsScr>().GameManeger.CardFight(card, GetComponent<CardInfoScr>());
        }


        
    }


}
