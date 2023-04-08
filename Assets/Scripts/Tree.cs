using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Managers;
using UnityEngine;

public class Tree : Entity2D
{
    [field: SerializeField]
    public Dimension Dimension
    {
        get;
        private set;
    }
}
