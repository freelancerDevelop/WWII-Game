using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoi : MonoBehaviour
{
    function Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // toggle visibility:
            renderer.enabled = !renderer.enabled;
        }

    }
}