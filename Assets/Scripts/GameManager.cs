using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Game Buttons
    public Button dealButton;
    public Button hitButton;
    public Button standButton;
    public Button betButton;
    private int standClicks = 0;
    // Access the player and dealer's hand
    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    // public Text to access and update - hud
    public Text scoreText;
    public Text dealerScoreText;
    public Text betsText;
    public Text cashText;
    public Text mainText;
    public Text standButtonText;
    
    // Card hiding dealer's 2nd Card
    public GameObject hideCard;
    // How much is bet
    int pot = 0;
    void Start()
    {
        //Add on click listeners to the buttons
        dealButton.onClick.AddListener(() => DealClicked());
        hitButton.onClick.AddListener(() => HitClicked());
        standButton.onClick.AddListener(() => StandClicked());
        betButton.onClick.AddListener(() => BetClicked());
    }

    private void DealClicked()
    {
        // Reset round, hide text, setup for next hand
        playerScript.ResetHand();
        dealerScript.ResetHand();
        // Hide dealer hand score at start of the deal
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        // Update the score displayed
        scoreText.text = "Hand: " + playerScript.handValue;
        dealerScoreText.text = "Hand: " + dealerScript.handValue;
        // Enable to hide one of the dealer's cards
        hideCard.GetComponent<Renderer>().enabled = true;
        // Adjust buttons visibility
        dealButton.gameObject.SetActive(false);
        hitButton.gameObject.SetActive(true);
        standButton.gameObject.SetActive(true);
        standButtonText.text = "Stand";
        // Set standard pot size
        pot = 40;
        betsText.text = "Bets: $" + pot;
        playerScript.AdjustMoney(-20);
        cashText.text = "$" + playerScript.GetMoney();
    }

    private void HitClicked()
    {
        // Check that there is still room on the table
        if (playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            scoreText.text = "Hand: " + playerScript.handValue;
            if(playerScript.handValue > 20) RoundOver();
        }
    }

    private void StandClicked()
    {
        standClicks++;
        if(standClicks > 1) RoundOver();
        HitDealer();
        standButtonText.text = "Call";
    }

    private void HitDealer()
    {
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            dealerScoreText.text = "Hand: " + dealerScript.handValue;
            if (dealerScript.handValue > 20) RoundOver();

        }
    }
    
    // Check for winner and loser, hand is over
    void RoundOver()
    {
        // Booleans for bust and blackjack
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;
        // If stand is clicked less than twice, no 21 or bust, end function
        if (standClicks < 2 && !playerBust && !player21 && !dealerBust && !dealer21) return;
        bool roundOver = true;
        // All bust, bets returned
        if(playerBust && dealerBust)
        {
            mainText.text = "All Bust: Bets Returned";
            playerScript.AdjustMoney(pot/2);
        }
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer Wins!";
        }
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You win!";
            playerScript.AdjustMoney(pot);
        }
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Push: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }
        // Set up UI for next turn
        if (roundOver)
        {
            hitButton.gameObject.SetActive(false);
            standButton.gameObject.SetActive(false);
            dealButton.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            cashText.text = "$" + playerScript.GetMoney();
            standClicks = 0;
        }
    }
    
    // Add money to pot if bet clicked
    void BetClicked()
    {
        // grabs text of bet button to use as bet
        Text newBet = betButton.GetComponentInChildren(typeof(Text)) as Text;
        int inBet = int.Parse(newBet.text.ToString().Remove(0, 1));
        playerScript.AdjustMoney(-inBet);
        cashText.text = "$" + playerScript.GetMoney();
        pot += (inBet * 2);
        betsText.text = "Bets: $" + pot;
    }
}