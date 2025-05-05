using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;
    private float width;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        var b = GetComponent<BoxCollider>();
        width = b.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < startPos.x - width/2)
        {
            transform.position = startPos;
        }
    }
}
