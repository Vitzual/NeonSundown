using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCardSelect : MonoBehaviour
{
    public int cardPosition = 1;
    public int upgradePosition = 0;

    public List<Card> cards;
    public List<UpgradeSlot> upgrades;

    protected bool cardSelect = false;
    protected bool upgradeSelect = false;
    
    protected bool controllerEnabled = false;
    public GameObject controllerBurn, kmbBurn, controllerSkip, kmbSkip, 
        controllerRedraw, kmbRedraw, controllerReroll, kmbReroll;

    public void Start()
    {
        InputEvents.Instance.onClickThruPressed.AddListener(OnCardSelect);
        InputEvents.Instance.onRightButton.AddListener(OnCardRedraw);
        InputEvents.Instance.onTopButton.AddListener(OnCardSkip);
        InputEvents.Instance.onLeftButton.AddListener(OnCardBurn);
        InputEvents.Instance.onRightDPad.AddListener(OnRightDpad);
        InputEvents.Instance.onLeftDPad.AddListener(OnLeftDpad);
        InputEvents.Instance.onTopDPad.AddListener(OnTopDpad);
        InputEvents.Instance.onBottomDPad.AddListener(OnBottomDpad);
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

            if (Dealer.active.isUpgrading)
            {
                if (!upgradeSelect)
                {
                    upgradePosition = 0;
                    upgradeSelect = true;
                    upgrades[upgradePosition].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
                }

                upgrades[cardPosition].ApplyUpgrade();
                upgradeSelect = false;
            }
            else
            {
                if (!cardSelect)
                {
                    cardPosition = 1;
                    cardSelect = true;
                    cards[cardPosition].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
                }

                cards[cardPosition].OnClick(false);
                cardSelect = false;
            }
        }
    }

    public void OnCardRedraw()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen)
        {
            ToggleController(true);

            if (Dealer.active.isUpgrading)
            {
                if (!upgradeSelect)
                {
                    upgradePosition = 0;
                    upgradeSelect = true;
                    upgrades[upgradePosition].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
                }

                Dealer.active.RerollUpgrades();
            }
            else
            {
                if (!cardSelect)
                {
                    cardPosition = 1;
                    cardSelect = true;
                    cards[cardPosition].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
                }

                Dealer.active.Redraw(cards[cardPosition]);
            }
        }
    }

    public void OnCardBurn()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen && !Dealer.active.isUpgrading)
        {
            ToggleController(true);

            if (!cardSelect)
            {
                cardPosition = 1;
                cardSelect = true;
                cards[cardPosition].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }

            Dealer.active.Burn(cards[cardPosition]);
        }
    }

    public void OnCardSkip()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen && !Dealer.active.isUpgrading)
        {
            ToggleController(true);

            if (!cardSelect)
            {
                cardPosition = 1;
                cardSelect = true;
                cards[cardPosition].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }

            Dealer.active.CloseDealer();
            cardSelect = false;
        }
    }

    public void OnTopDpad()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen && Dealer.active.isUpgrading)
        {
            ToggleController(true);

            if (!upgradeSelect)
            {
                upgradePosition = 0;
                upgradeSelect = true;
                upgrades[upgradePosition].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }

            if (upgradePosition == 1)
            {
                upgradePosition = 0;
                upgrades[1].GetComponent<OnHoverAdjustScale>().OnPointerExit(null);
                upgrades[0].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
            else if (upgradePosition == 2)
            {
                upgradePosition = 1;
                upgrades[2].GetComponent<OnHoverAdjustScale>().OnPointerExit(null);
                upgrades[1].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
        }
    }

    public void OnBottomDpad()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen && Dealer.active.isUpgrading)
        {
            ToggleController(true);

            if (!upgradeSelect)
            {
                upgradePosition = 0;
                upgradeSelect = true;
                upgrades[upgradePosition].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }

            if (upgradePosition == 0)
            {
                upgradePosition = 1;
                upgrades[0].GetComponent<OnHoverAdjustScale>().OnPointerExit(null);
                upgrades[1].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
            else if (upgradePosition == 1)
            {
                upgradePosition = 2;
                upgrades[1].GetComponent<OnHoverAdjustScale>().OnPointerExit(null);
                upgrades[2].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
        }
    }

    public void OnRightDpad()
    {
        if (Dealer.isOpen && Dealer.active.IsOpen && !Dealer.active.isUpgrading)
        {
            ToggleController(true);

            if (!cardSelect)
            {
                cardPosition = 1;
                cardSelect = true;
                cards[cardPosition].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }

            if (cardPosition == 0)
            {
                cardPosition = 1;
                cards[0].GetComponent<OnHoverAdjustScale>().OnPointerExit(null);
                cards[1].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
            else if (cardPosition == 1)
            {
                cardPosition = 2;
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

            if (!cardSelect)
            {
                cardPosition = 1;
                cardSelect = true;
                cards[cardPosition].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }

            if (cardPosition == 2)
            {
                cardPosition = 1;
                cards[2].GetComponent<OnHoverAdjustScale>().OnPointerExit(null);
                cards[1].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
            else if (cardPosition == 1)
            {
                cardPosition = 0;
                cards[1].GetComponent<OnHoverAdjustScale>().OnPointerExit(null);
                cards[0].GetComponent<OnHoverAdjustScale>().OnPointerEnter(null);
            }
        }
    }
}
