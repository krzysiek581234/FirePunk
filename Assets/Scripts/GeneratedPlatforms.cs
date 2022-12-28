using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;


public class GeneratedPlatforms : MonoBehaviour
{
    public GameObject platformPrefab;
    const int PLATFORMS_NUM = 6;
    public GameObject[] platforms;
    public Vector3[] positions;
    public Vector3[] DstPositions;
    public Vector3[] FirPositions;
    private float speed = 1f;

    void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[6];
        DstPositions = new Vector3[6];
        FirPositions = new Vector3[6];


        for (int i = 0; i < 6; i++)
        {
            float angle = (float)(i*2*3.14 / 6);
            float x = transform.position.x + 5 * cos(angle);
            float y = transform.position.y + 5 * sin(angle);
            float z = 0.0f;

            float x1 = transform.position.x + 2 * cos(angle);
            float y1 = transform.position.y + 2 * sin(angle);
            float z1 = 0.0f;


            DstPositions[i] = new Vector3(x1, y1, z1);
            positions[i] = new Vector3(x, y, z);
            FirPositions[i] = new Vector3(x, y, z);
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);

    }

}
// Start is called before the first frame update
void Start()
{

}

// Update is called once per frame
void Update()
{
    
    for (int i = 0; i < PLATFORMS_NUM; i++)
    {
        var step = speed * Time.deltaTime;
        platforms[i].transform.position = Vector3.MoveTowards(platforms[i].transform.position, DstPositions[i], step);
        if (Vector3.Distance(platforms[i].transform.position, DstPositions[i]) < 0.1  )
        {
                Debug.Log("swap");
                var temp = DstPositions[i];
                DstPositions[i] = FirPositions[i];
                FirPositions[i] = temp;
        }
    }



    }
}