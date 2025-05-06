using System;
using UnityEngine;
using UnityEngine.UI;

public class MouseWorldPosition3D : MonoBehaviour
{
    public static MouseWorldPosition3D instance;
    public Vector3? worldPosition;
    public Camera miniMapCamera;

    Ray ray;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        worldPosition = GetMouseWorldPosition();
        // else
        //     Debug.Log("未获取到世界位置");
        
    }
    public Vector3? GetMouseWorldPosition()
    {
        Vector3 worldPosition = miniMapCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2, miniMapCamera.nearClipPlane));

        Vector3 rayOrigin = new Vector3(worldPosition.x + PlayerManager.instance.playerTransform.position.x * 10, worldPosition.y + PlayerManager.instance.playerTransform.position.y * 10, 0);

        ray = new Ray(rayOrigin, Vector3.forward);

        Vector3 intersectionPoint = ray.origin;

        return new Vector3(intersectionPoint.x / 10, intersectionPoint.y / 10, 0f);
    }
}