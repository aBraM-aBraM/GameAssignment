using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ID : MonoBehaviour {

    [SerializeField]
    private int _id;

    public int GetID
    {
        get
        {
            return _id;
        }
    }
}
