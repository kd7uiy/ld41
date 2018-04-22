/* Copyright 2018 Old Ham Media/ Ben Pearson
 * For more information, see http://www.whereisroadster.com
 * All rights reserved
 **/
using System.Collections.Generic;
using UnityEngine;

public class StarfieldBackground : MonoBehaviour {

    public Star star;

    public float StarsPerBlock=5;

    public float Depth = 3f;

    //TODO Could do an animation curve here for better use
    public float maxSize = 1f;

    public Gradient colors;

    private new Camera camera;

    private Queue<Star> unusedStars = new Queue<Star>();
    private Dictionary<int, StarBlock> seedToBlock = new Dictionary<int, StarBlock>();

	// Use this for initialization
	void Start () {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        camera = Camera.main;
	}

    // Update is called once per frame
    void Update()
    {
        CheckBlocks();
    }

    public void DeactivateStar(Star star)
    {
        star.gameObject.SetActive(false);
        unusedStars.Enqueue(star);
    }

    public void CheckBlocks()
    {
        foreach (StarBlock block in seedToBlock.Values)
        {
            block.ResetTagged();
        }

        Vector3 lowerLeft=camera.ViewportToWorldPoint(new Vector3(0,0,10+Depth));
        Vector3 upperRight = camera.ViewportToWorldPoint(new Vector3(1, 1, 10 + Depth));
        for (int x=Mathf.FloorToInt(lowerLeft.x); x<=Mathf.CeilToInt(upperRight.x); x++)
        {
            for (int y = Mathf.FloorToInt(lowerLeft.y); y <= Mathf.CeilToInt(upperRight.y); y++)
            {
                int seed = SeedFromCoordinates(x, y);
                if (seedToBlock.ContainsKey(seed) == false)
                {
                    seedToBlock[seed]=new StarBlock(x, y, seed, transform,this);
                } else
                {
                    seedToBlock[seed].Tag();
                }
            }
        }

        foreach (KeyValuePair<int,StarBlock> block in seedToBlock)
        {
            if (block.Value.tagged==false)
            {
                block.Value.Deactivate(this);
                seedToBlock.Remove(block.Key);
            }
        }
    }

    public Star NewStar(Transform parent)
    {
        if(unusedStars.Count>0)
        {
            Star obj= unusedStars.Dequeue();
            obj.transform.parent = parent;
            obj.gameObject.SetActive(true);
            return obj;
        } else
        {
            return Instantiate(star, parent);
        }
    }

    private static int SeedFromCoordinates(float x, float y)
    {
        //Two prime numbers well outside of the expected range, which should make this unique.
        return Mathf.RoundToInt(x * 389 + y * 397);
    }
}

public struct StarBlock
{
    Star[] stars;
    public bool tagged;

    public StarBlock(float x, float y, int seed, Transform parent, StarfieldBackground Starfield)
    {
        Random.InitState(seed);
        tagged = true;
        int numStars = Mathf.FloorToInt(Starfield.StarsPerBlock * Random.value * Random.value);
        List<Star> stars = new List<Star>();
        for (int i = 0; i < numStars; i++)
        {
            Star star = Starfield.NewStar(parent);
            star.SetColor(Starfield.colors.Evaluate(Random.value));
            star.transform.position = new Vector3(x + Random.value, y + Random.value, Random.value * Starfield.Depth);
            float size = Starfield.maxSize * (1 + 2 * Random.value) / 3;
            star.transform.localScale = new Vector2(size, size);
            //TODO Randomize color!
            stars.Add(star);
        }
        this.stars = stars.ToArray();
    }

    public void ResetTagged()
    {
        tagged = false;
    }

    public void Tag()
    {
        tagged = true;
    }

    public void Deactivate(StarfieldBackground background)
    {
        foreach (Star star in stars) {
            background.DeactivateStar(star);
        }
    }
}
