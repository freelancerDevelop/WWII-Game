using UnityEngine;
using System.Collections;

public class RotatingMenu : MonoBehaviour
{
    public float speed = 10f;


    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
        transform.Rotate(Vector2.up, speed * Time.deltaTime);
    }
}