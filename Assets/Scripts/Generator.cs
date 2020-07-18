using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Generator : MonoBehaviour
{
    private GameObject[] grid;
    private List<GameObject> walls;
    private bool[] inMaze;
    private Transform[] children;

    // Use this for initialization
    void Start()
    {
        // get all wall gameobjects in grid from transforms.
        children = gameObject.GetComponentsInChildren<Transform>();
        GenerateTheMaze();
    }

    // Update is called once per frame
    void Update(){}

    void GenerateTheMaze()
    {
        // Cells are numbered from 0 to 63 from bottom left to top right.
        // every wall is marked as "the border between two cells"
        // Walls are named using adjacent cell numbers. so Wall05-10 the border between cell 5 and 10
        // Using Recursive Backtracking algorithm

        grid = new GameObject[112]; // since this is the number of grid we have
        int i = 0;
        foreach (Transform t in children)
        {
            if (t.name.Contains("Wall"))
            {
                grid[i] = t.gameObject;
                i++;
            }
        }

        // Initialize the inMaze booleans to false by default.
        inMaze = new bool[64];

        // select random starting wall and put in temporary list of processed walls.
        walls = new List<GameObject>();

        int r = UnityEngine.Random.Range(0, 64);
        inMaze[r] = true;
        foreach (GameObject g in grid)
        {
            if (r < 10)
            {
                if (g.name.Contains("0" + r.ToString()))
                {
                    walls.Add(g);
                }
            }
            else
            {
                if (g.name.Contains(r.ToString()))
                {
                    walls.Add(g);
                }
            }
        }

        // repeat until all walls processed.
        while (walls.Count != 0)
        {
            // pick random wall from list and find adjacent cells.
            GameObject w = walls[UnityEngine.Random.Range(0, walls.Count)];
            int first = int.Parse(w.name.Substring(4, 2));
            int second = int.Parse(w.name.Substring(7, 2)); // keep reading our label

            // if both cells are already in maze, keep wall and remove from processed walls.
            if (inMaze[first] && inMaze[second])
            {
                walls.Remove(w);
            }
            // if first one is in maze then make second one in maze and destroy wall between them.
            else if (inMaze[first])
            {
                inMaze[second] = true;
                w.SetActive(false);

                foreach (GameObject g in grid)
                {
                    if (second < 10)
                    {
                        if (g.name.Contains("0" + second.ToString()))
                        {
                            walls.Add(g);
                        }
                    }
                    else
                    {
                        if (g.name.Contains(second.ToString()))
                        {
                            walls.Add(g);
                        }
                    }
                }
            }
            // if second one is in maze then make first one in maze and destroy wall between them
            else if (inMaze[second])
            {
                inMaze[first] = true;
                w.SetActive(false);

                foreach (GameObject g in grid)
                {
                    if (first < 10)
                    {
                        if (g.name.Contains("0" + first.ToString()))
                        {
                            walls.Add(g);
                        }
                    }
                    else
                    {
                        if (g.name.Contains(first.ToString()))
                        {
                            walls.Add(g);
                        }
                    }
                }
            }
        }

        // get all remaining active walls
        List<GameObject> active = new List<GameObject>();
        foreach (GameObject g in grid)
        {
            if (g.activeSelf)
            {
                active.Add(g);
            }
        }

        // pick 20-30 random walls and make them breakable.
        r = UnityEngine.Random.Range(20, 30);
        Debug.Log(r);
        while (r > 0)
        {
            int rand = UnityEngine.Random.Range(0, active.Count);
            active[rand].tag = "Wall";
            active.RemoveAt(rand);
            r--;
        }
    }
}