using UnityEngine;

public class UI_NPCTextBox : MonoBehaviour
{
    private RectTransform panelTransform;
    private RectTransform textTransform;

    private void Awake() 
    {
        panelTransform = GetComponent<RectTransform>();
        textTransform = GetComponentInChildren<RectTransform>();
    }
    private void Start()
    {
        AdjustTextBoxSize();
    }

    private void Update() 
    {
        
    }

    private void AdjustTextBoxSize() 
    {
        textTransform.localScale = new Vector3(1 /panelTransform.lossyScale.x, 1 / panelTransform.lossyScale.y, 1 / panelTransform.lossyScale.z);
    }
}
