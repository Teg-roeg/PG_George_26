using UnityEngine;

public class Item : MonoBehaviour, IPickUp
{
    public void PickUp()
    {
        print("Picked Up");

        Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 50);
    }
}

internal interface IPickUp
{

    void PickUp();
}