using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Security.Cryptography.X509Certificates;

public struct Card
{
    public string Name;
    public Sprite Logo;
    public int Attack, Defence, Manacost;
    public bool CanAttack;

    public bool IsAlive
    {
        get
        {
            return Defence > 0;
        }
    }

    public Card(string name, string logoPath, int attack, int defence, int manacost)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defence = defence;
        CanAttack = false;
        Manacost = manacost;
    }

    public void ChangeAttackState(bool can)
    {
         CanAttack = can;
    }

    public void GetDamage(int dmg)
    {
        Defence -= dmg;
    }

}

public static class CardManeger
{
    public static List<Card> AllCards = new List<Card>();
}

public class CardManagerScr : MonoBehaviour
{

    public void Awake()
    {
        CardManeger.AllCards.Add(new Card("axe", "Sprites/Cards/axe", 5, 6, 4));
        CardManeger.AllCards.Add(new Card("bane", "Sprites/Cards/bane", 2, 6, 2));
        CardManeger.AllCards.Add(new Card("batrider", "Sprites/Cards/batrider", 10, 10, 6));
        CardManeger.AllCards.Add(new Card("kez", "Sprites/Cards/kez", 6, 5, 5));
        CardManeger.AllCards.Add(new Card("kunka", "Sprites/Cards/kunkka", 3, 8, 4));
        CardManeger.AllCards.Add(new Card("lina", "Sprites/Cards/lina", 7, 4, 5));
        
    }

}
