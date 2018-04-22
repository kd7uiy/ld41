/* Copyright 2018 Old Ham Media/ Ben Pearson
 * For more information, see http://www.whereisroadster.com
 * All rights reserved
 **/
using UnityEngine;

public class FollowObject : MonoBehaviour {

    public bool x=true;
    public bool y=true;
    public bool z=false;
    public Transform objectToFollow;
	
	// Update is called once per frame
	void Update () {
        Vector3 newPosition = transform.position;
        if (x)
        {
            newPosition.x = objectToFollow.position.x;
        }
        if (y)
        {
            newPosition.y = objectToFollow.position.y;
        }
        if (z)
        {
            newPosition.z = objectToFollow.position.z;
        }
        transform.position = newPosition;
    }
}
