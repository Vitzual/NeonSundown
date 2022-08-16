using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuntimeCard : MonoBehaviour
{
    private CardData card;
    public TextMeshProUGUI amount;
    public Image background, icon;
    private RectTransform rect;
    private RuntimeCard nextCard;
    private int count = 1;

    public void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Setup(CardData card, Vector2 position)
    {
        this.card = card;
        icon.sprite = card.sprite;
        icon.color = card.color;
        background.color = card.color;
        amount.text = count.ToString();
    }

    public void SetNextStat(RuntimeCard stat)
    {
        nextCard = stat;
    }

    public void UpdateStat()
    {
        count += 1;
        amount.text = count.ToString();
    }

    public void DeleteStat(CardData card)
    {
        if (this.card == card)
        {
            if (nextCard != null)
                nextCard.UpdatePosition(rect.localPosition);
            Destroy(gameObject);
        }
    }

    public void SetPosition(Vector2 position)
    {
        rect.localPosition = position;
    }

    public void UpdatePosition(Vector2 position)
    {
        if (nextCard != null)
            nextCard.UpdatePosition(rect.localPosition);
        SetPosition(position);
    }
}
