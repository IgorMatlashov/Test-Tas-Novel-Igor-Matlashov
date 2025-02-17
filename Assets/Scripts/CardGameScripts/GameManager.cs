using System.Collections;
using Naninovel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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
        if (instance == null)
        {
            instance = this;
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
        }
        
    }

    void GenerateCards()
    {
        isGameStarted = true;
        int totalCards = cardDataArray.Length * repetitions;
        
        CardData[] tempCardData = new CardData[totalCards];
        
        int index = 0;
        
        for (int i = 0; i < cardDataArray.Length; i++)
        {
            for (int j = 0; j < repetitions; j++)
            {
                tempCardData[index] = cardDataArray[i];
                index++;
            }
        }
        
        for (int i = 0; i < tempCardData.Length; i++)
        {
            int randomIndex = Random.Range(i, tempCardData.Length);
            (tempCardData[i], tempCardData[randomIndex]) = (tempCardData[randomIndex], tempCardData[i]);
        }
        
        
        tableCards = new Card[totalCards];
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
            yield return new WaitForSeconds(GameManager.instance.durationFlip);
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