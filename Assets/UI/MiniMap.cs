using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameObject player;
    public Camera MiniMapCamera;
    // private void Start()
    // {
    //     MiniMapCamera.transform.position = player.transform.position;
    // }
    //
    // private void Update()
    // {
    //     MiniMapCamera.transform.position = player.transform.position;
    // }

    private void Update()
    {
        Vector3 newPosition = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            MiniMapCamera.transform.position.z
        );
        MiniMapCamera.transform.position = newPosition;
        float zRotation = player.transform.eulerAngles.z;
        MiniMapCamera.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);

    }
}
