using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool Blocked; //Whether the node is occupied by a wall
    public Vector2 WorldPos; //Where the node is in the world
    public Vector2Int GridPos; //Where the node is on the grid

    public int GCost; //How far from the node is from the start
    public int HCost; //How close the node is to the end

    public Node Parent; //The node before it in the path

    int heapIndex;

    public int FCost //The combined total of the G and H Cost
    {
        get
        {
            return GCost + HCost; //Feed the result of the 2 when referenced instead of assigning the variable
        }
    }

    public int HeapIndex //Create a variable for the heap index
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    //Create a function to compare the priority of nodes
    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(nodeToCompare.HCost);
        }
        return -compare;
    }

    //Create a constructor for our nodes
    //A constructor is the Start() function equivalent of a non-Monobehaviour script
    //It is called when a new instance of the script is created
    //Here we use it to give each node its own statistics and important information it will need for pathfinding
    public Node(bool IsBlocked, Vector2 WorldPosition, Vector2Int GridPosition)
    {
        Blocked = IsBlocked;
        WorldPos = WorldPosition;
        GridPos = GridPosition;
    }
}
