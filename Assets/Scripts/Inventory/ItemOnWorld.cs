using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item item;
    public Inventory PlayerInventory;
    public GameObject self;

    private SpriteRenderer sr;

    private float time;
    private float amplitude = 0.05f;
    private float frequency = 1f;

  

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        time = Time.time;
    }

    private void Update()
    {
        ItemInVisualRange();
        ItemFloatingFX();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            Debug.Log("Pick up item: " + self.name);
            PlayerInventory.AddItem(item);
            Destroy(self);
        }
    }

    private void ItemFloatingFX()
    {
        time += Time.deltaTime;
        time %= 2 * Mathf.PI;
        sr.transform.localPosition = new Vector3(sr.transform.localPosition.x, amplitude * Mathf.Cos(time * frequency) + amplitude / 2, 0);
    }

    private void ItemInVisualRange() 
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, PlayerManager.instance.playerTransform.position)) >= MapGenerator.instance.radius - 2.5f)
            sr.enabled = false;
        else
            sr.enabled = true;
    }
}
