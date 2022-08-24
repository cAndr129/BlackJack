using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    int currentIndex = 0;
    void Start()
    {
        GetCardValues();
    }

    // Update is called once per frame
    void GetCardValues()
    {
        int num = 0;
        //Loop to assign vals to cards
        for (int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            //Count up to amount of cards
            num %= 13;
            //Allows cards to reset when they hit 13
            //except if value is over 10, use 10
            if (num > 10 || num == 0)
            {
                num = 10;
            }

            cardValues[i] = num++;
        }
    }

    public void Shuffle()
    {
        // Standard array data swapping technique
        //Consider changing later
        for (int i = cardSprites.Length - 1; i > 0; --i)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * (cardSprites.Length - 1)) + 1;
            Sprite face = cardSprites[i];
            cardSprites[i] = cardSprites[j];
            cardSprites[j] = face;

            int value = cardValues[i];
            cardValues[i] = cardValues[j];
            cardValues[j] = value;
        }

        currentIndex = 1;
    }

    public int DealCard(CardScript cardScript)
    {
        cardScript.SetSprite(cardSprites[currentIndex]);
        cardScript.SetValue(cardValues[currentIndex++]);
        return cardScript.GetValueOfCard();
    }

    public Sprite GetCardBack()
    {
        return cardSprites[0];
    }
}
