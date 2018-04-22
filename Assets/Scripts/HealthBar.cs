/* Copyright 2018 Old Ham Media/ Ben Pearson
 * For more information, see http://www.whereisroadster.com
 * All rights reserved
 **/
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image image;
    public SharedFloat Max;
    public SharedFloat Value;
	
	// Update is called once per frame
	void Update () {
        image.transform.localScale = new Vector3(Mathf.Clamp(Value.val / Max.val,0,1),1,1);
	}
}
