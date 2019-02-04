using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    //Array of all neighbor nodes represented by their index
    public int[] neighbors;

    private void Update()
    {
        Transform transform = GetComponent<Transform>();

        transform.Rotate(new Vector3(0, 1, 0));
    }
}
