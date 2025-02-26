using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardData cardData;
    private bool isFlipped = false;
    private bool isMatched = false;
    
    [SerializeField] private Image frontSprite;
    [SerializeField] private Image backSprite;
    
    private Button button;

    private void Start()
    {
        frontSprite.sprite = cardData.frontSprite;
        backSprite.sprite = cardData.backSprite;
        
        button = GetComponent<Button>();
        button.onClick.AddListener(OnCardClick);
    }

    private void OnCardClick()
    {
        if (!isFlipped && !isMatched && GameManager.Instance.CanFlip())
        {
            Flip();
            GameManager.Instance.CardFlipped(this);
        }
    }

    public void Flip()
    {
        isFlipped = !isFlipped;
        float targetAngle = isFlipped ? 180f : 0f;
        StartCoroutine(RotateCoroutine(targetAngle, GameManager.Instance.durationFlip));
    }

    public void SetMatched()
    {
        isMatched = true;
    }

    public bool IsMatched()
    {
        return isMatched;
    }

    public bool IsFlipped()
    {
        return isFlipped;
    }
    
    private IEnumerator RotateCoroutine(float targetAngle, float duration)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Quaternion startRotation = rectTransform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, targetAngle, 0);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            rectTransform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            yield return null;
        }

        rectTransform.rotation = endRotation;
    }
}