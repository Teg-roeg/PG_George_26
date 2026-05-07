using UnityEngine;

public class Food : Item
{
    void Start()
    {
        
    }

    internal void Eat() {
        print("Yummy!");
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 50);
    }
}
