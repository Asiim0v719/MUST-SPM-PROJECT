using UnityEngine;

public class  CavasManager: MonoBehaviour
{
    public static CavasManager instance;
    public GameObject UICanvas;

    private void Awake()
    {

        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}
