using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObjectPool : MonoBehaviour
{
    // 1. Barrel / 2. Create / 3. Stone WAll
    // ถูก set ให้ตรงกับ SpawnManager.obstaclePrefab ใน this.Start
    [SerializeField] private GameObject[] obstaclePrefab;
    [SerializeField] private int initialPoolSize = 3;

    // List ของแต่ละ Pool ข้อเสีย => ไม่ยืดหยุ่น, ถ้ามี pool เพิ่มก็ต้องเขียนโค้ดเพิ่มทั้งเป็นการเขียนลักษณะเดิม ๆ
    private readonly List<GameObject> obstaclePoolBarrel = new List<GameObject>();
    private readonly List<GameObject> obstaclePoolCrate = new List<GameObject>();
    private readonly List<GameObject> obstaclePoolStoneWall = new List<GameObject>();
    private readonly List<GameObject> obstaclePoolRock = new List<GameObject>();
    private readonly List<GameObject> obstaclePoolBarrier = new List<GameObject>();

    private static ObstacleObjectPool instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static ObstacleObjectPool GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        // set ให้ตรงกับ SpawnManager.obstaclePrefab
        obstaclePrefab = SpawnManager.GetInstance().obstaclePrefab;

        // สร้าง obstacle แต่ละอย่างตามจำนวน initialPoolSize
        for (int i = 0; i < initialPoolSize; i++)
        {
            for (int j = 0; j < obstaclePrefab.Length; j++)
            {
                CreateNewObstacle(j);
            }
        }
    }

    private void CreateNewObstacle(int poolIndex)
    {
        // สร้าง obstacle ตาม index
        // ปิด active
        // ใส่ pool 

        GameObject obstacle = Instantiate(obstaclePrefab[poolIndex]);
        Debug.Log($"Instantiate {obstaclePrefab[poolIndex]}");

        obstacle.SetActive(false);

        if (poolIndex == 0)
        {
            obstaclePoolBarrel.Add(obstacle);
            Debug.Log("Pool Barrel >> Add}");

        }
        else if (poolIndex == 1)
        {
            obstaclePoolCrate.Add(obstacle);
            Debug.Log("Pool Crate >> Add}");
        }
        else if(poolIndex == 2)
        {
            obstaclePoolStoneWall.Add(obstacle);
            Debug.Log("Pool Stone Wall >> Add}");
        }
        else if (poolIndex == 3)
        {
            obstaclePoolRock.Add(obstacle);
            Debug.Log("Pool Rock >> Add}");
        }
        else if (poolIndex == 4)
        {
            obstaclePoolBarrier.Add(obstacle);
            Debug.Log("Pool Barrier >> Add}");
        }
        else
        {
            Debug.Log("No Pool for Add}");
        }

    }

    public GameObject Acquire(int poolIndex)
    {
        // check .Count ของ pool ถ้า = 0 สร้างเพิ่ม
        // set obstacle = pool index 0
        // ลบ pool index 0 ออก
        // เปิด active
        // return obstacle
        // ถ้า index ไม่ตรงกับ pool ให้ return null ไปก่อน

        GameObject obstacle;
        if (poolIndex == 0)
        {
            obstacle = AcquireFromList(poolIndex, obstaclePoolBarrel);
            Debug.Log("Acquire Barrel");
            return obstacle;
        }
        else if (poolIndex == 1)
        {
            obstacle = AcquireFromList(poolIndex, obstaclePoolCrate);
            Debug.Log("Acquire Create");
            return obstacle;
        }
        else if (poolIndex == 2)
        {
            obstacle = AcquireFromList(poolIndex, obstaclePoolStoneWall);
            Debug.Log("Acquire Stone Wall");
            return obstacle;
        }
        else if (poolIndex == 3)
        {
            obstacle = AcquireFromList(poolIndex, obstaclePoolRock);
            Debug.Log("Acquire Rock");
            return obstacle;
        }
        else if (poolIndex == 4)
        {
            obstacle = AcquireFromList(poolIndex, obstaclePoolBarrier);
            Debug.Log("Acquire Barrier");
            return obstacle;
        }
        Debug.Log("No Pool for Acquire");
        return null;
    }

    public GameObject AcquireFromList(int idx, List<GameObject> list)
    {
        GameObject obstacle;
        if (list.Count == 0)
        {
            CreateNewObstacle(idx);
        }
        obstacle = list[0];
        list.RemoveAt(0);
        obstacle.SetActive(true);

        return obstacle;
    }


    public void Return(GameObject obstacle, int poolIndex)
    {
        // add object ใส่ pool ตามเลข index
        // ปิด active

        if (poolIndex == 0)
        {
            obstaclePoolBarrel.Add(obstacle);
            Debug.Log("Return Barrel");
        }
        else if (poolIndex == 1)
        {
            obstaclePoolCrate.Add(obstacle);
            Debug.Log("Return Crate");
        }
        else if (poolIndex == 2)
        {
            obstaclePoolStoneWall.Add(obstacle);
            Debug.Log("Return Stone Wall");
        }
        else if (poolIndex == 3)
        {
            obstaclePoolRock.Add(obstacle);
            Debug.Log("Return Rock");
        }
        else if (poolIndex == 4)
        {
            obstaclePoolBarrier.Add(obstacle);
            Debug.Log("Return Barrier");
        }

        obstacle.SetActive(false);
    }
}
