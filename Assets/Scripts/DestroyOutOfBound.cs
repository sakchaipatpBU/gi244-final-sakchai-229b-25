using UnityEngine;

public class DestroyOutOfBound : MonoBehaviour
{
    public int obstacleIndex;
    public float leftBound = -10;

    void Update()
    {
        if (transform.position.x < leftBound)
        {
            ReturnObj();
        }
    }

    public void ReturnObj()
    {
        ObstacleObjectPool.GetInstance().Return(gameObject, obstacleIndex);
    }
}
