using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Animal
{
    public string Name{get;set;}

    public void Eat()
    {
        Debug.Log(Name + " is eating");
    }
}

class Dog : Animal
{
    public void Bark()
    {
        Debug.Log(Name + " is barking");
    }
}

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RecursiveFunction(5);
        Dog dog = new Dog();
        dog.Bark();
        dog.Eat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RecursiveFunction(int count)
    {
        if (count <= 0)
        {
            Debug.Log("End");
            return;
        }
        Debug.Log("count : "+count);
        RecursiveFunction(count - 1);
    }
}
