using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] GameObject platformPrefab;
    const int PLATFORMS_NUM = 4;
    GameObject[] platforms;
    Vector3[] positions;
    Vector3[] DstPosition;
    int radius = 2;
    float speed = 2;

    private void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];
        DstPosition = new Vector3[PLATFORMS_NUM];
    }
    void Start()
    {
        
        for (int i= 0; i < PLATFORMS_NUM; i++)
        {
            float angle = 2 * Mathf.PI * i / PLATFORMS_NUM;
            positions[i].x = transform.position.x + radius * Mathf.Cos(angle);
            positions[i].y = transform.position.y + radius * Mathf.Sin(angle);
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            float angle = 2 * Mathf.PI * i / PLATFORMS_NUM;
            DstPosition[i].x = transform.position.x + radius * Mathf.Cos(angle + Time.time);
            DstPosition[i].y = transform.position.y + radius * Mathf.Sin(angle + Time.time);
        }
        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            platforms[i].transform.position = Vector3.MoveTowards(platforms[i].transform.position, DstPosition[i],speed * Time.deltaTime);
        }
    }
}
