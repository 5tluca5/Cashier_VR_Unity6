using UnityEngine;

public class Floater : MonoBehaviour
{
    public float amplitude = 0.5f; // The height of the floating motion
    public float speed = 2.0f;    // The speed of the floating motion

    private Vector3 startPos;

    void Start()
    {
        // Store the starting position of the GameObject
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate the new position
        float offset = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPos + new Vector3(0, offset, 0);
    }
}
