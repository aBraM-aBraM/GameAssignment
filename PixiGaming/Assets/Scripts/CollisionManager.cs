using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionManager
{

    public static List<GameObject> Colliders = new List<GameObject>();

    // Recursive collision finding
    public static void IteratorMethod(Collider collider, Material mat)
    {
        CollisionManager.Colliders.Add(collider.gameObject);
        BubbleController collisions = collider.GetComponent<BubbleController>();

        foreach (GameObject nextObject in collisions.allCollisions)
        {
            if (!CollisionManager.Colliders.Contains(nextObject))
            {
              if (nextObject.GetComponent<Renderer>().material != mat) return;
              IteratorMethod(nextObject.GetComponent<Collider>(), mat);
            }
        }
    }
}
