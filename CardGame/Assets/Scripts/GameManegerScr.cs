using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


public class Game
{
    public List<Card> EnemyDeck, PlayerDeck;

    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard(); 

        //EnemyHand = new List<Card>();
        //PlayerHand = new List<Card>();

        //EnemyField = new List<Card>();
        //PlayerField = new List<Card>();
    } 

    List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < 10; i++)
            list.Add(CardManeger.AllCards[Random.Range(0, CardManeger.AllCards.Count)]);
        return list;
    }
}


public class GameManegerScr : MonoBehaviour
{
    public Game Currentgame;
    public Transform EnemyHand, PlayerHand,
                     EnemyField, PlayerField;

    public GameObject CardPref;
    int Turn, TurnTime = 30;
     
    public TextMeshProUGUI TurnTimeTxt;
    public Button EndTimeBtn;

    public int PlayerMana = 10, EnemyMana = 10;
    public TextMeshProUGUI PlayerManaTxt, EnemyManaTxt;

    public int PlayerHP, EnemyHP;
    public TextMeshProUGUI PlayerHPTxt, EnemyHPTxt;

    public GameObject ResultGO;
    public TextMeshProUGUI ResultTxt;

    public List<CardInfoScr> PlayerHandCards = new List<CardInfoScr>(),
                             PlayerFieldCards = new List<CardInfoScr>(),
                             EnemyHandCards = new List<CardInfoScr>(),
                             EnemyFieldCards = new List<CardInfoScr>();

    public bool IsPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
    }


    void Start()
    {
        StartGame();
    }

    void GiveHandCards(List<Card> deck, Transform hand)
    {

        //if (deck == null || hand == null)
            //return;

        int i = 0;
        while (i++ < 4)
            GiveCardsToHand(deck, hand);
    }


    public void RestartGame()
    {
        StopAllCoroutines();

        foreach (var card in PlayerHandCards)
            Destroy(card.gameObject);
        foreach (var card in PlayerFieldCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyFieldCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyHandCards)
            Destroy(card.gameObject);

        PlayerHandCards.Clear();
        EnemyFieldCards.Clear();
        PlayerFieldCards.Clear();
        EnemyHandCards.Clear();


        StartGame();
    }

    public void StartGame()
    {
        Turn = 0;
        EndTimeBtn.interactable = true; 

        Currentgame = new Game();

        GiveHandCards(Currentgame.EnemyDeck, EnemyHand);
        GiveHandCards(Currentgame.PlayerDeck, PlayerHand);

        PlayerMana = EnemyMana = 10;
        PlayerHP = EnemyHP = 30;

        ShowHP();
        ShowMana();

        ResultGO.SetActive(false);

        StartCoroutine(TurnFunc());
    }



    void GiveCardsToHand(List<Card> deck, Transform hand)
    {
        if (deck.Count == 0)
            return;

        Card card = deck[0];

        GameObject cardGO = Instantiate(CardPref, hand, false);

        if (hand == EnemyHand)
        {
            cardGO.GetComponent<CardInfoScr>().HideCardinfo(card);
            EnemyHandCards.Add(cardGO.GetComponent<CardInfoScr>());
        }
        else
        {
            cardGO.GetComponent<CardInfoScr>().ShowCardInfo(card, true);
            PlayerHandCards.Add(cardGO.GetComponent<CardInfoScr>());
            cardGO.GetComponent<AtteckedCard>().enabled = false;
        }
        deck.RemoveAt(0); 
    }


    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        TurnTimeTxt.text = TurnTime.ToString();

        foreach (var card in PlayerFieldCards)
            card.DeHighlightCard();

        if (IsPlayerTurn)
        {
            foreach (var card in PlayerFieldCards)
            {
                card.SelfCard.ChangeAttackState(true);
                card.HighlightCard();
            }
            while (TurnTime-- > 0)
            {
                TurnTimeTxt.text = TurnTime.ToString();
                yield return new WaitForSeconds(1);
            }
        }
        else
        {
            foreach (var card in EnemyFieldCards)
                card.SelfCard.ChangeAttackState(true);

            while (TurnTime-- > 27)
            {
                TurnTimeTxt.text = TurnTime.ToString();
                yield return new WaitForSeconds(1);
            }

            if (EnemyHandCards.Count > 0)
                EnemyTurn(EnemyHandCards);

        }
        ChangeTurn();
    }

    void EnemyTurn(List<CardInfoScr> cards)
    {
        int count = cards.Count == 1 ? 1 :
                    Random.Range(0, cards.Count);

        for (int i = 0; i < count; i++)
        {
            if (EnemyFieldCards.Count > 5)
                break;

            List<CardInfoScr> cardList = cards.FindAll(x => EnemyMana >= x.SelfCard.Manacost);

            if (cardList.Count == 0)
                break;


            ReduceMana(false, cardList[0].SelfCard.Manacost);
            //Потом замена на cards
            cardList[0].ShowCardInfo(cardList[0].SelfCard, false);
            cardList[0].transform.SetParent(EnemyField);

            EnemyFieldCards.Add(cardList[0]);
            EnemyHandCards.Remove(cardList[0]);
        }

        foreach (var activeCard in EnemyFieldCards.FindAll(x => x.SelfCard.CanAttack))
        {
            if (Random.Range(0, 2) == 0 &&
                PlayerFieldCards.Count > 0)
            {
                if (PlayerFieldCards.Count == 0)
                    return;

                var enemy = PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)];

                Debug.Log(activeCard.SelfCard.Name + " (" + activeCard.SelfCard.Attack + ";" + activeCard.SelfCard.Defence + " ) " + "--->" +
                    enemy.SelfCard.Name + " (" + enemy.SelfCard.Attack + ";" + enemy.SelfCard.Defence + ")");

                activeCard.SelfCard.ChangeAttackState(false);
                CardFight(enemy, activeCard);
            }
            else
            {
                Debug.Log(activeCard.SelfCard.Name + " (" + activeCard.SelfCard.Attack + ") Attacked Hero");

                activeCard.SelfCard.ChangeAttackState(false);
                DamageHero(activeCard, false);
            }
        }
    }





    //        deck .RemoveAt(0); 
    //    }


    public void ChangeTurn()
    {
        StopAllCoroutines();

        Turn++;

        EndTimeBtn.interactable = IsPlayerTurn;

        if (IsPlayerTurn) 
        {
            GiveNewCards();

            PlayerMana = EnemyMana = 10;
            ShowMana();

        }
        StartCoroutine(TurnFunc());
    }

    void GiveNewCards()
    {
        GiveCardsToHand(Currentgame.EnemyDeck, EnemyHand);
        GiveCardsToHand(Currentgame.PlayerDeck, PlayerHand);
    }

    public void CardFight(CardInfoScr playerCard, CardInfoScr enemyCard)
    {
        playerCard.SelfCard.GetDamage(enemyCard.SelfCard.Attack);
        enemyCard.SelfCard.GetDamage(playerCard.SelfCard.Attack);

        if (!playerCard.SelfCard.IsAlive)
            DestroyCard(playerCard);
        else
            playerCard.RefreshData();

        if (!enemyCard.SelfCard.IsAlive)
            DestroyCard(enemyCard);
        else
            enemyCard.RefreshData();
    }

    void DestroyCard(CardInfoScr card)
    {
        card.GetComponent<CardMovementsScr>().OnEndDrag(null);

        if (EnemyFieldCards.Exists(x => x == card))
            EnemyFieldCards.Remove(card);

        if (PlayerFieldCards.Exists(x => x == card))
            PlayerFieldCards.Remove(card);

        Destroy(card.gameObject);
    }

    void ShowMana()
    {
        PlayerManaTxt.text = PlayerMana.ToString();
        EnemyManaTxt.text = EnemyMana.ToString();
    }

    void ShowHP()
    {
        EnemyHPTxt.text = EnemyHP.ToString();
        PlayerHPTxt.text = PlayerHP.ToString();
    }

    public void ReduceMana(bool playerMana, int manacost)
    {
        if (playerMana)
        {
            PlayerMana = Mathf.Clamp(PlayerMana - manacost, 0, int.MaxValue);

        }
        else
        {
            EnemyMana = Mathf.Clamp(EnemyMana - manacost, 0, int.MaxValue);
        }
        ShowMana();
    }
    public void DamageHero(CardInfoScr card, bool isEnemyAttacked)
    {
        if (isEnemyAttacked)
            EnemyHP = Mathf.Clamp(EnemyHP - card.SelfCard.Attack, 0, int.MaxValue);
        else
            PlayerHP = Mathf.Clamp(PlayerHP - card.SelfCard.Attack, 0, int.MaxValue);

        ShowHP();
        card.DeHighlightCard();
        CheckForResult();
    }
    void CheckForResult()
    {
        if (EnemyHP == 0 || PlayerHP == 0)
        {
            ResultGO.SetActive(true);
            StopAllCoroutines();

            if (EnemyHP == 0)
                ResultTxt.text = "Win GG EZ GRATZ";
            else
                ResultTxt.text = "Againe - 25";
        }

    }
}
