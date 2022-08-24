using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    int currentIndex = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void GetCardValues()
    {
        int num = 0;
        //Loop to assign vals to cards
        for(int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            //Count up to amount of cards
            num %= 13;
            //Allows cards to reset when they hit 13
            //except if value is over 10, use 10
            if(num > 10 || num == 0)
            {
                num = 10;
            }
            cardValues[i] = num++;
        }
        currentIndex = 1;
    }
}
