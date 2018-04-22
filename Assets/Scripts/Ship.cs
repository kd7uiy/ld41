/* Copyright 2018 Old Ham Media/ Ben Pearson
 * For more information, see http://www.whereisroadster.com
 * All rights reserved
 **/
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class Ship : MonoBehaviour {

    Vector3 speed = Vector2.zero;
    float dir = 0;

    public float burnRate = 0.5f;
    public float steeringBurnRate = 0.25f;
    public float metalPerShot = 0.1f;
    public float lifeSupportRequired = 0.1f;

    public float bulletSpeed;

    public SharedFloat force;
    public SharedFloat maneuverability;

    public SharedFloat mass;
    public SharedFloat fireTime;

    public SharedFloat fuel;
    public SharedFloat fuelMax;

    public SharedFloat metal;
    public SharedFloat metalMax;

    public SharedFloat life;
    public SharedFloat lifeMax;

    public SharedFloat money;

    public ParticleSystem particle;

    public Transform[] guns;
    private float baseRate=100;
    private float nextFireTime;
    public GameObject bullet;
    private Rigidbody2D rigidBody;

    private AudioSource thrustSource;

    public AudioClip FireSound;
    public AudioClip GameOverSound;
    public AudioClip CollectCard;

    Deck deck;

    public GameOver gameOverDialog;

	// Use this for initialization
	void Start () {
        thrustSource = GetComponent<AudioSource>();

        particle.Stop();
        rigidBody = GetComponent<Rigidbody2D>();
        deck = FindObjectOfType<Deck>();

        mass.val = 10;
        money.val = 0;
        force.val = 10;
        maneuverability.val = 10;
        fireTime.val = 0.2f;

        fuel.val = 10;
        fuelMax.val = 10;

        metal.val = 10;
        metalMax.val = 10;

        life.val = 10;
        lifeMax.val = 10;
	}

    // Update is called once per frame
    void Update () {

        if (life.val>=lifeMax.val)
        {
            life.val = lifeMax.val;
        }

        if (fuel.val >= fuelMax.val)
        {
            fuel.val = fuelMax.val;
        }

        if (metal.val >= metalMax.val)
        {
            metal.val = metalMax.val;
        }


        thrustSource.volume = 0;
        if (fuel.val > 0)
        {
            float acceleration = Acceleration();
            float steering = Steering();

            float totalMass = (mass.val + fuel.val + metal.val)*0.5f;  //Life support doesn't weight much.

            //4 is to roughly convert to Moment of Inertia.
            dir += 4*Time.deltaTime * steering * maneuverability.val / totalMass;
            rigidBody.mass = totalMass;
            rigidBody.AddForce(100 * Time.deltaTime * transform.right * acceleration * force.val);

            transform.rotation = Quaternion.AngleAxis(dir * 180 / Mathf.PI, Vector3.forward);
            //transform.position += speed * Time.deltaTime;

            particle.Emit(Mathf.RoundToInt(baseRate * Time.deltaTime * acceleration));

            if (Input.GetKey(KeyCode.Space) && nextFireTime < Time.time)
            {
                Fire();
            }

            fuel.val -= Time.deltaTime * (acceleration * burnRate + Mathf.Abs(steering) * steeringBurnRate);
            fuel.val = Mathf.Max(0, fuel.val);

            thrustSource.volume = acceleration;
        }

        life.val -= lifeSupportRequired*Time.deltaTime;

        if (life.val<0)
        {
            GameOver("Ran out of life support.");
        }
        
    }

    private void GameOver(string message)
    {
        gameOverDialog.gameObject.SetActive(true);
        gameOverDialog.SetReason(message);
        AudioSource.PlayClipAtPoint(GameOverSound, transform.position);
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag=="rock")
        {
            GameOver("Ship was destroyed.");
        } else
        {
            deck.AddCard();
            Destroy(target.gameObject);
            AudioSource.PlayClipAtPoint(CollectCard, transform.position);
        }
    }

    void Fire()
    {
        if (metal.val > metalPerShot)
        {
            nextFireTime = Time.time + fireTime.val;
            foreach (Transform transform in guns)
            {
                GameObject bullet = Instantiate(this.bullet);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.GetComponent<Rigidbody2D>().velocity = speed + new Vector3(Mathf.Cos(dir), Mathf.Sin(dir)) * bulletSpeed;
            }
            metal.val -= metalPerShot;

            rigidBody.AddForce(-10 * transform.right * force.val * bulletSpeed * metalPerShot);
            AudioSource.PlayClipAtPoint(FireSound, transform.position);
        }
    }

    float Steering()
    {
        return -Input.GetAxis("Horizontal");
    }

    float Acceleration()
    {
        return Mathf.Clamp( Input.GetAxis("Vertical"),0,1);
    }
}
