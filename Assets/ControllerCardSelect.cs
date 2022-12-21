using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCardSelect : MonoBehaviour
{
    public int position = 1;

    public List<Card> cards;
    protected bool cardSelect = false;
    protected bool upgradesOpen = false;

    public void Start()
    {
        InputEvents.Instance.onClickThruPressed.AddListener(OnCardSelect);
        InputEvents.Instance.onRightButton.AddListener(OnCardSkip);
        InputEvents.Instance.onLeftButton.AddListener(OnCardBurn);
        InputEvents.Instance.onRightDPad.AddListener(OnRightDpad);
        InputEvents.Instance.onLeftDPad.AddListener(OnLeftDpad);
    }

    public void OnCardSelect()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen)
        {
            if (!cardSelect)
            {
                position = 1;
                cardSelect = true;
                cards[position].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
            else
            {
                cards[position].OnClick(false);
                cardSelect = false;
            }
        }
    }

    public void OnCardSkip()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen)
        {
            if (!cardSelect)
            {
                position = 1;
                cardSelect = true;
                cards[position].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
            else
            {
                Dealer.active.Redraw(cards[position]);
                cardSelect = false;
            }
        }
    }

    public void OnCardBurn()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen)
        {
            if (!cardSelect)
            {
                position = 1;
                cardSelect = true;
                cards[position].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
            else
            {
                Dealer.active.Burn(cards[position]);
                cardSelect = false;
            }
        }
    }

    public void OnRightDpad()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen)
        {
            if (position == 0)
            {
                position = 1;
                cards[0].GetComponent<OnHoverAdjustScale>().OnPointerExit(null);
                cards[1].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
            else if (position == 1)
            {
                position = 2;
                cards[1].GetComponent<OnHoverAdjustScale>().OnPointerExit(null);
                cards[2].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
        }
    }

    public void OnLeftDpad()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen)
        {
            if (position == 2)
            {
                position = 1;
                cards[2].GetComponent<OnHoverAdjustScale>().OnPointerExit(null);
                cards[1].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
            else if (position == 1)
            {
                position = 0;
                cards[1].GetComponent<OnHoverAdjustScale>().OnPointerExit(null);
                cards[0].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
        }
    }
}
