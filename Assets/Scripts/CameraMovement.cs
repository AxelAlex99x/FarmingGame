using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform playerPosition;
    
    public float offsetZ = 5f;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = FindAnyObjectByType<NewBehaviourScript>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(playerPosition.position.x, transform.position.y, playerPosition.position.z - offsetZ);
        transform.position = targetPosition;
       
    }
}
