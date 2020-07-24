using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MazeGenerator_RB_bits : MonoBehaviour
{
    // Rows and columns being public so that clients may fill in the value they prefer
    public int row, column, wallWidth;
    public Transform walls, Tile_regular, Tile_current; // Wall and cells

    uint NORTH = 0b_0000_0001;
    uint EAST = 0b_0000_0010;
    uint SOUTH = 0b_0000_0100;
    uint WEST = 0b_0000_1000;
    uint VISITED = 0b_0001_0000;

    private uint[] maze;
    // Visited cells as well as the visit history to be recorded
    private int visitedCells;
    private Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
    private List<Transform> previousCurrent = new List<Transform>();
    System.Random rnd = new System.Random();

    void Start()
    {
        // Initialize the maze array with the preferred number of cells
        maze = new uint[row * column];
        // We start from a random position and set up whatever needed
        int x = rnd.Next(row); int y = rnd.Next(column);
        stack.Push(Tuple.Create(x, y));
        maze[y * row + x] = VISITED;
        visitedCells = 1;

        InitializeMazeStructure();
    }

    void Update()
    {
        if (RB_Algorithm())
            DrawEverything();
    }

    bool RB_Algorithm()
    {
        List<int> neighbours = new List<int>();
        Func<int, int, uint> lookAt = (px, py) => (uint)((stack.Peek().Item1 + px) + (stack.Peek().Item2 + py) * row);

        if (visitedCells < row * column)
        {
            // North neighbour
            if (stack.Peek().Item2 > 0 && (maze[lookAt(0, -1)] & VISITED) == 0)
                neighbours.Add(0); // meaning the north neighbour exists and unvisited
            // East neighbour
            if (stack.Peek().Item1 < row - 1 && (maze[lookAt(+1, 0)] & VISITED) == 0)
                neighbours.Add(1);
            // South neighbour
            if (stack.Peek().Item2 < column - 1 && (maze[lookAt(0, +1)] & VISITED) == 0)
                neighbours.Add(2);
            // West neighbour
            if (stack.Peek().Item1 > 0 && (maze[lookAt(-1, 0)] & VISITED) == 0)
                neighbours.Add(3);

            // Are there any neighbours available?
            if (neighbours.Count > 0)
            {
                // Choose one available neighbour at random
                int nextCellDir = neighbours[rnd.Next(neighbours.Count)];

                // Create a path between the neighbour and the current cell
                switch (nextCellDir)
                {
                    case 0: // North
                        maze[lookAt(0, -1)] |= VISITED | SOUTH;
                        maze[lookAt(0, 0)] |= NORTH;
                        stack.Push(Tuple.Create((stack.Peek().Item1 + 0), (stack.Peek().Item2 - 1)));
                        break;

                    case 1: // East
                        maze[lookAt(+1, 0)] |= VISITED | WEST;
                        maze[lookAt(0, 0)] |= EAST;
                        stack.Push(Tuple.Create((stack.Peek().Item1 + 1), (stack.Peek().Item2 + 0)));
                        break;

                    case 2: // South
                        maze[lookAt(0, +1)] |= VISITED | NORTH;
                        maze[lookAt(0, 0)] |= SOUTH;
                        stack.Push(Tuple.Create((stack.Peek().Item1 + 0), (stack.Peek().Item2 + 1)));
                        break;

                    case 3: // West
                        maze[lookAt(-1, 0)] |= VISITED | EAST;
                        maze[lookAt(0, 0)] |= WEST;
                        stack.Push(Tuple.Create((stack.Peek().Item1 - 1), (stack.Peek().Item2 + 0)));
                        break;

                }
                visitedCells++; // Update the visited cells' number
            }
            else
            {
                // No available neighbours so backtrack!
                stack.Pop();
            }
            //DrawEverything();
            return false;
        }
        return true;
    } 
    void DrawEverything()
    {
        // Draw Maze
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < column; y++)
            {
                // Knock down the wall pieces we put before
                for (int p = 0; p < wallWidth; p++)
                {
                    Vector3 v3 = new Vector3(x * (wallWidth + 1) + p, 0, y * (wallWidth + 1) + wallWidth);
                    Vector3 v3_2 = new Vector3(x * (wallWidth + 1) + wallWidth, 0, y * (wallWidth + 1) + p);
                    if (((maze[y * row + x] & SOUTH) == 4) && checkIfTilePosEmpty(v3))
                        Instantiate(Tile_regular, v3, Quaternion.identity);
                    if (((maze[y * row + x] & EAST) == 2) && checkIfTilePosEmpty(v3_2))
                        Instantiate(Tile_regular, v3_2, Quaternion.identity);
                }
            }
        }

        // Get rid of the previous stack peek first
        foreach (Transform t in previousCurrent)
        {
            if(t != null) // because it can be destroyed already
                Instantiate(Tile_regular, t.position, Quaternion.identity);
        }

        // Then we draw the new stack peek
        for (int py = 0; py < wallWidth; py++)
        {
            for (int px = 0; px < wallWidth; px++)
            {
                Vector3 v3 = new Vector3(stack.Peek().Item1 * (wallWidth + 1) + px, 0, stack.Peek().Item2 * (wallWidth + 1) + py);
                if(checkIfTilePosEmpty(v3))
                    previousCurrent.Add(Instantiate(Tile_current, v3, Quaternion.identity));
            }
        }
    }

    private void InitializeMazeStructure()
    {
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < column; y++)
            {
                // Each cell is inflated by wallWidth, so fill it in
                for (int py = 0; py < wallWidth; py++)
                {
                    for (int px = 0; px < wallWidth; px++)
                    {
                        Vector3 v3 = new Vector3(x * (wallWidth + 1) + px, 0, y * (wallWidth + 1) + py);
                        Instantiate(Tile_regular, v3, Quaternion.identity);
                        Vector3 v3_1 = new Vector3(x * (wallWidth + 1) + px, 0, y * (wallWidth + 1) + wallWidth);
                        Vector3 v3_2 = new Vector3(x * (wallWidth + 1) + wallWidth, 0, y * (wallWidth + 1) + px);
                        Instantiate(walls, v3_1, Quaternion.identity);
                        Instantiate(walls, v3_2, Quaternion.identity);
                    }
                }
                // Fill in the corner cell
                Instantiate(walls, new Vector3((x+1) * (wallWidth + 1) - 1, 0, (y+1) * (wallWidth + 1) - 1), Quaternion.identity);
            }
        }
        // Fill in rest of the cells
        for (int i = 0; i < row * (wallWidth+1); i++)
        {
            Instantiate(walls, new Vector3(i, 0, (column - column)* (wallWidth + 1) - 1), Quaternion.identity);
        }
        for (int j = -1; j < column * (wallWidth + 1); j++)
        {
            Instantiate(walls, new Vector3((row - row) * (wallWidth + 1) - 1, 0, j), Quaternion.identity);
        }
    }

    private bool checkIfTilePosEmpty(Vector3 targetPos)
    {
        GameObject[] allTilings = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject t in allTilings)
        {
            if (t.transform.position == targetPos)
            {
                Destroy(t);
            }
        }
        return true;
    }
}