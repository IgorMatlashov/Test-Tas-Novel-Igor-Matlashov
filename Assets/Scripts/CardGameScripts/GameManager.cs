using System.Collections;
using System.Linq;
using Naninovel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool isGameStarted = false;

    public float durationFlip;
    
    public int repetitions = 2; 
    public Card cardPrefabs;
    public CardData[] cardDataArray;


    private Card[] tableCards;
    private Card firstCard;
    private Card secondCard;
    private bool canFlip = true;
    
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        if (!isGameStarted)
        {
            GenerateCards();
            isGameStarted = true;
        }
        
    }

    void GenerateCards()
    {
        int totalCards = cardDataArray.Length * repetitions;
        
        tableCards = new Card[totalCards];
        
        CardData[] tempCardData = cardDataArray
            .SelectMany(cardData => Enumerable.Repeat(cardData, repetitions))
            .OrderBy(_ => Random.value)
            .ToArray();
        
        for (int i = 0; i < totalCards; i++)
        {
            Card card = Instantiate(cardPrefabs, gameObject.transform);
            card.cardData = tempCardData[i];
            tableCards[i] = card;
        }
    }

    public void CardFlipped(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        canFlip = false;
        yield return new WaitForSeconds(durationFlip);

        if (firstCard.cardData.frontSprite == secondCard.cardData.frontSprite)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();
        }
        else
        {
            firstCard.Flip();
            secondCard.Flip();
            yield return new WaitForSeconds(GameManager.Instance.durationFlip);
        }

        firstCard = null;
        secondCard = null;
        canFlip = true;

        CheckGameOver();
    }

    public bool CanFlip()
    {
        return canFlip;
    }

    private void CheckGameOver()
    {
        foreach (Card card in tableCards)
        {
            if (!card.IsMatched()) return;
        }
    
        isGameOver = true;
    
        if (Engine.Initialized)
        {
            Engine.GetService<ICustomVariableManager>()?
                .SetVariableValue("cardGameComplete", "true");
        }
    
        foreach (Card card in tableCards)
        {
            Destroy(card.gameObject);
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}