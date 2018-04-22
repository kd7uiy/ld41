/* Copyright 2018 Old Ham Media/ Ben Pearson
 * For more information, see http://www.whereisroadster.com
 * All rights reserved
 **/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    public TextMeshProUGUI valueText;
    public TextMeshProUGUI useText;
    public TextMeshProUGUI massText;

    public SharedFloat MoneyOnShip;
    public SharedFloat MassOnShip;
    private SharedFloat ChangeOnUse;

    private float amountToChange;
    private int mass;
    private int value;

    public Color selected;
    public Color notSelected;

    private Hand hand;

    public void Initialize(int mass, int value, string use, SharedFloat ChangeOnUse, float amountToChange, Hand hand)
    {
        GetComponent<Image>().color = notSelected;
        this.hand=hand;
        //TODO Figure out how to use cards

        this.useText.text = use;
        this.valueText.text = "$" + value;
        this.massText.text = "" + mass + " mT";

        this.ChangeOnUse = ChangeOnUse;
        this.amountToChange = amountToChange;

        this.mass = mass;
        this.value = value;

        MassOnShip.val += mass;
    }

    public void Use()
    {
        ChangeOnUse.val += amountToChange;
        Remove();
    }

    public void Cash()
    {
        MassOnShip.val += mass;
        MoneyOnShip.val += value;
        Remove();
    }

    public void Remove()
    {
        MassOnShip.val -= mass;
        Destroy(gameObject);
    }

    public void OnClick()
    {
        hand.SelectCard(this);
    }

    public void SetSelectionState(bool state)
    {
        if (state)
        {
            GetComponent<Image>().color = selected;
        } else
        {
            GetComponent<Image>().color = notSelected;
        }
    }
}
