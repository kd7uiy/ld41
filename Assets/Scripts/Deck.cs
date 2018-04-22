/* Copyright 2018 Old Ham Media/ Ben Pearson
 * For more information, see http://www.whereisroadster.com
 * All rights reserved
 **/
using UnityEngine;

public class Deck : MonoBehaviour {

    public Card card;
    public Hand hand;

    [SerializeField]
    public CardType[] cards;

    public void AddCard()
    {
        int sum = 0;
        for (int i=0;i<cards.Length;i++) {
            sum += cards[i].numInDeck;
        }
        int cardSelected=Random.Range(0,sum);

        Card card = Instantiate(this.card, hand.transform, false);
        sum = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            sum += cards[i].numInDeck;
            if (sum > cardSelected)
            {
                cards[i].SetCardType(card, hand);
                break;
            }
        }
    }
}

[System.Serializable]
public class CardType
{
    public int numInDeck=1;    //Totally random, but more common cards show up more
    public int minMass=1;
    public int maxMass=10;
    public int minCash=1;
    public int maxCash=10;
    public int minAmount=1;
    public int maxAmount=10;

    public SharedFloat FloatToChange;

    public string symbol;

    public Card SetCardType(Card card,Hand hand)
    {
        int amountToChange = Random.Range(minAmount, maxAmount);
        //TODO Figure out how to do other types!!!
        //TODO Figure out a more logical way to make this work!
        card.Initialize(Random.Range(minMass, maxMass), Random.Range(minCash, maxCash), "" + amountToChange + symbol, FloatToChange, amountToChange, hand);

        return card;
    }
}
