using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour {

    [HideInInspector]
    public List<GameObject> allCollisions;

    public void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Bubble")
        {
            allCollisions.Add(col.collider.gameObject);
            print("name: " + name + "\n" + "colliders amount: " + allCollisions.Count);
        }
    }

    public void OnCollisionExit(Collision col)
    {
        if (col.transform.tag == "Bubble")
        {
            allCollisions.Remove(col.collider.gameObject);
        }
    }
}
