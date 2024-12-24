using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CardInfoScr : MonoBehaviour
{
    public Card SelfCard;
    public Image Logo;
    public TextMeshProUGUI Name, Attack, Defence, Manacost;
    public GameObject HideObj, HighlightedObj;
    public bool IsPlayer;

    public void HideCardinfo(Card card)
    {
        SelfCard = card;
        Logo.sprite = null;        
        HideObj.SetActive(true);
        IsPlayer = false;
        Manacost.text = "";
    }

    public void ShowCardInfo(Card card, bool isPlayer) 
    {
        IsPlayer = isPlayer;
        HideObj.SetActive(false);
        SelfCard = card;

        Logo.sprite = card.Logo;
        Logo.preserveAspect = true;
        Name.text = card.Name;

        RefreshData();
    }
    //private void Start()
    //{
    //    ShowCardInfo(CardManeger.AllCards[transform.GetSiblingIndex()]);
    //}

    public void RefreshData()
    {
        Attack.text = SelfCard.Attack.ToString();
        Defence.text = SelfCard.Defence.ToString();
        Manacost.text = SelfCard.Manacost.ToString();
    }

    public void HighlightCard()
    {
        HighlightedObj.SetActive(true);
    }
    public void DeHighlightCard()
    {
        HighlightedObj.SetActive(false);
    }
}