/* Copyright 2018 Old Ham Media/ Ben Pearson
 * For more information, see http://www.whereisroadster.com
 * All rights reserved
 **/
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour {

    public Asteroid rock;
    public Asteroid dust;

    public float RocksPerBlock = 5;
    public float blockSize=10;

    private new Camera camera;

    private Queue<Asteroid> unusedRocks = new Queue<Asteroid>();
    private Queue<Asteroid> unusedDust = new Queue<Asteroid>();
    private List<int> blocksDetermined=new List<int>();

    public AudioClip DustSound;
    public AudioClip RockSound;

    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckBlocks();
    }

    public void DeactivateRock(Asteroid rock)
    {
        rock.gameObject.SetActive(false);
        unusedRocks.Enqueue(rock);
    }

    public void DeactivateDust(Asteroid dust)
    {
        dust.gameObject.SetActive(false);
        unusedDust.Enqueue(dust);
    }

    public void CheckBlocks()
    {
        Vector3 lowerLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, 10 ));
        Vector3 upperRight = camera.ViewportToWorldPoint(new Vector3(1, 1, 10 ));
        for (int x = Mathf.FloorToInt(lowerLeft.x); x <= Mathf.CeilToInt(upperRight.x); x++)
        {
            for (int y = Mathf.FloorToInt(lowerLeft.y/blockSize); y <= Mathf.CeilToInt(upperRight.y/blockSize); y++)
            {
                int seed = SeedFromCoordinates(x, y);
                if (blocksDetermined.Contains(seed) == false)
                {
                    blocksDetermined.Add(seed);
                    
                    FindRocks(seed,x, y);
                }
            }
        }
    }

    public void FindRocks(int seed, float x, float y)
    {
        Random.InitState(seed);
        int rocks = Mathf.FloorToInt(RocksPerBlock * Random.value * Random.value);

        for (int i = 0; i < rocks; i++)
        {
            int size = Mathf.RoundToInt(3 * Random.value * Random.value + 0.5f);

            Asteroid rock = NewRock(transform,size);
            rock.transform.position = new Vector3((x + Random.value)*blockSize, (y + Random.value)*blockSize, 0);
            rock.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360f), Vector3.forward);
            rock.Initialize(this,size);
        }
    }

    public void CreatePair(Asteroid prev, int size)
    {
        Rigidbody2D rigid = prev.GetComponent<Rigidbody2D>();

        if (size>0)
        {
            AudioSource.PlayClipAtPoint(RockSound, prev.transform.position);
        } else
        {
            AudioSource.PlayClipAtPoint(DustSound, prev.transform.position);
        }

        float dir = Random.Range(0, 180f);
        float separationForce = Random.Range(10, 100f)*10;
        Asteroid r1 = NewRock(transform,size-1);
        r1.transform.position = prev.transform.position+ size * FromAngle(dir)*0.5f;
        r1.Initialize(this, size - 1);
        Rigidbody2D rr1 = r1.GetComponent<Rigidbody2D>();
        rr1.rotation = rigid.rotation;
        rr1.velocity = rigid.velocity;
        if (r1.tag == "rock")
        {
            rr1.AddForce(FromAngle(dir) * separationForce);
        }

        dir += 180f;
        Asteroid r2 = NewRock(transform,size-1);
        r2.transform.position = prev.transform.position + size * FromAngle(dir) * 0.5f;
        r2.Initialize(this, size - 1);
        Rigidbody2D rr2 = r2.GetComponent<Rigidbody2D>();
        rr2.rotation = rigid.rotation;
        rr2.velocity = rigid.velocity;
        if (r2.tag == "rock")
        {
            rr2.AddForce(FromAngle(dir) * separationForce);
        }
    }

    private static Vector3 FromAngle(float angle)
    {
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public Asteroid NewRock(Transform parent, int size)
    {
        if (size<=0)
        {
            if (unusedDust.Count > 0)
            {
                Asteroid obj = unusedDust.Dequeue();
                obj.transform.parent = parent;
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                return Instantiate(dust, parent);
            }
        }

        if (unusedRocks.Count > 0)
        {
            Asteroid obj = unusedRocks.Dequeue();
            obj.transform.parent = parent;
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            return Instantiate(rock, parent);
        }
    }

    private static int SeedFromCoordinates(float x, float y)
    {
        //Two prime numbers well outside of the expected range, which should make this unique.
        return Mathf.RoundToInt(x * 389 + y * 397);
    }
}