using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public ParticleSystem particles;

    void Start()
    {
        particles.Stop();
    }

	void OnCollisionEnter2D(Collision2D target) 
    {
        particles.Emit(100);
        Destroy(gameObject);

    }
}
