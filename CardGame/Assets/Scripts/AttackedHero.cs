using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class AttackedHero : MonoBehaviour, IDropHandler 
{
    public enum HeroType
    {
        ENEMY,
        PLAYER
    }

    public HeroType Type; 
    public GameManegerScr GameManeger;

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManeger.IsPlayerTurn)
            return;

        CardInfoScr card = eventData.pointerDrag.GetComponent<CardInfoScr>();

        if (card &&
            card.SelfCard.CanAttack &&
            Type == HeroType.ENEMY)
        {
            card.SelfCard.CanAttack = false;
            GameManeger.DamageHero(card, true);
        }
    }
}
