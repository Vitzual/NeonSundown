using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCardSelect : MonoBehaviour
{
    public int position = 1;

    public List<Card> cards;
    protected bool cardSelect = false;
    protected bool upgradesOpen = false;
    
    protected bool controllerEnabled = false;
    public GameObject controllerBurn, kmbBurn, controllerSkip, 
        kmbSkip, controllerRedraw, kmbRedraw;

    public void Start()
    {
        InputEvents.Instance.onClickThruPressed.AddListener(OnCardSelect);
        InputEvents.Instance.onRightButton.AddListener(OnCardRedraw);
        InputEvents.Instance.onTopButton.AddListener(OnCardSkip);
        InputEvents.Instance.onLeftButton.AddListener(OnCardBurn);
        InputEvents.Instance.onRightDPad.AddListener(OnRightDpad);
        InputEvents.Instance.onLeftDPad.AddListener(OnLeftDpad);
    }

    public void ToggleController(bool toggle)
    {
        if (controllerEnabled != toggle)
        {
            controllerEnabled = toggle;
            controllerBurn.SetActive(toggle);
            controllerSkip.SetActive(toggle);
            controllerRedraw.SetActive(toggle);
            kmbBurn.SetActive(!toggle);
            kmbSkip.SetActive(!toggle);
            kmbRedraw.SetActive(!toggle);
        }
    }

    public void OnCardSelect()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen)
        {
            ToggleController(true);

            if (!cardSelect)
            {
                position = 1;
                cardSelect = true;
                cards[position].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }

            cards[position].OnClick(false);
            cardSelect = false;
        }
    }

    public void OnCardRedraw()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen)
        {
            ToggleController(true);

            if (!cardSelect)
            {
                position = 1;
                cardSelect = true;
                cards[position].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }

            Dealer.active.Redraw(cards[position]);
            cardSelect = false;
        }
    }

    public void OnCardBurn()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen)
        {
            ToggleController(true);

            if (!cardSelect)
            {
                position = 1;
                cardSelect = true;
                cards[position].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }

            Dealer.active.Burn(cards[position]);
            cardSelect = false;
        }
    }

    public void OnCardSkip()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen)
        {
            ToggleController(true);

            if (!cardSelect)
            {
                position = 1;
                cardSelect = true;
                cards[position].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }

            Dealer.active.CloseDealer();
            cardSelect = false;
        }
    }

    public void OnRightDpad()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen)
        {
            ToggleController(true);

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
            ToggleController(true);

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
