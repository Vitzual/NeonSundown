using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    // Active instance
    public static Dealer active;

    // Player instance
    public Deck playerDeck;

    // List of card slots
    public List<Card> cardSlots;

    // Dealing options
    public int cardsToPick = 3;

    // Deal cards
    public void DealCards()
    {
        // Get copy of the scriptables list
        List<CardData> cards = new List<CardData>(Scriptables.cards);

        // Iterate through and pick cards
        for (int i = 0; i < cardsToPick; i++)
        {
            // Check to make sure enough cards are in the list
            if (cards.Count == 0 || cardSlots.Count >= i) break;

            // Take the card and remove it from the list
            CardData card = cards[Random.Range(0, cards.Count)];
            cardSlots[i].Set(card);
            cards.Remove(card);
        }
    }

    // Pick the card and add to palyer
    public void PickCard(CardData card)
    {
        
    }
}
