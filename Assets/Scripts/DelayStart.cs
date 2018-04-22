using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DelayStart : MonoBehaviour {

    private TextMeshProUGUI text;

    public float TimeToShow = 1;

	// Use this for initialization
	void Start () {
        text = GetComponent<TextMeshProUGUI>();
        text.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time>TimeToShow)
        {
            text.enabled = true;
            enabled = false;
        }
	}
}
