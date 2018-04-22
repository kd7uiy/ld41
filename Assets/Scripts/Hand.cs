/* Copyright 2018 Old Ham Media/ Ben Pearson
 * For more information, see http://www.whereisroadster.com
 * All rights reserved
 **/

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Hand : MonoBehaviour {

    private Card selectedCard;

    public AudioClip UseSound;
    public AudioClip DumpSound;
    public AudioClip SellSound;

    private new AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }


    public void SelectCard(Card card)
    {
        if (selectedCard!=null)
        {
            selectedCard.SetSelectionState(false);
        }
        if (card != selectedCard)
        {

            card.SetSelectionState(true);
            selectedCard = card;
        }
    }

    public void UseSelected()
    {
        if (selectedCard != null)
        {
            selectedCard.Use();
            selectedCard = null;

            audio.clip = UseSound;
            audio.Play();
        }
        
    }

    public void DumpSelected()
    {
        if (selectedCard != null)
        {
            selectedCard.Remove();
            selectedCard = null;

            audio.clip = DumpSound;
            audio.Play();
        }
        
    }

    public void SellSelected()
    {
        if (selectedCard!=null)
        {
            selectedCard.Cash();
            selectedCard = null;

            audio.clip = SellSound;
            audio.Play();
        }
    }
}
