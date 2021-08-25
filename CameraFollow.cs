using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform playerT;
    [Range(0,1)]
    public float cameraHeight = 0.6f;
    public float maxHeight = -0.85f;

    // Start is called before the first frame update
    void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(playerT.position.x, playerT.position.y + cameraHeight, -10);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempPos = transform.position;
        tempPos.x = playerT.position.x;

        if (playerT.position.y > transform.position.y) tempPos.y = playerT.position.y;
        if (playerT.position.y < transform.position.y - maxHeight) tempPos.y = playerT.position.y + cameraHeight;

        transform.position = tempPos;
    }
}
