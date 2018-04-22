using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    AsteroidManager manager;
    int size;
    float collisionValidTime;

    public float timeToPreventCollisions = 0.5f;

    public float timeToLive = 0;

    private float timeToEnd = 0f;

    public bool canBeShot;

    public void Initialize(AsteroidManager asteroidManager, int size)
    {
        this.manager = asteroidManager;
        this.size = size;

        GetComponent<Rigidbody2D>().mass = 10 * Mathf.Pow(2, size );

        if (tag == "rock")
        {
            float scale = Mathf.Pow(2, size - 3);
            transform.localScale = new Vector2(scale, scale);
        }

        collisionValidTime = Time.time + timeToPreventCollisions;

        if (timeToLive>0)
        {
            timeToEnd = Time.time + timeToLive;
        }
    }

    private void Update()
    {
        if (timeToLive>0 && timeToEnd<Time.time)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (Time.time<collisionValidTime)
        {
            return;
        }
        if (target.gameObject.tag!="ship" && tag=="rock")
        {
            manager.CreatePair(this, size);
            manager.DeactivateRock(this);
        }
    }
}
