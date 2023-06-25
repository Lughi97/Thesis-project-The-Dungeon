using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    /// <summary>
    /// This node takes 2 vectors 2 postion and parent(where we cheking from the position) and set up a constructor
    /// used to create a list of node of position and parents  in the enemy script
    /// In the list we add for exemple
    /// 1,1 startPosition (1,1)the parent 
    /// </summary>

    public Vector2 position;
    public Vector2 parent;

    public Node(Vector2 _position, Vector2 _parent)
    {
        position = _position;
        parent = _parent;
    }

}