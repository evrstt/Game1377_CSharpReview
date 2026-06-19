using System;
using System.Collections.Generic;
using UnityEngine;

public class ErrorTypes : MonoBehaviour
{
    private GameObject testGameObject;
    private Rigidbody testRigidBody;
    private int[] testArray = { 10, 20, 30 };
    private List<string> _items = new List<string> { "A", "B" };
    private object _value = "hello";

    private void Start()
    {
        Method01();
        Method02();
        Method03();
        Method04();
        Method05();
        Method06();
        Method07();
        Method08();
        Method09();
        Method10();
        Method11();
        Method12();
        Method13();
        Method14();
        Method15();
    }

    // Error: _______________________________________________
    private void Method01()
    {
        Debug.Log(testGameObject.name);
    }

    // Error: _______________________________________________
    private void Method02()
    {
        GameObject a = new GameObject("A");
        DestroyImmediate(a);
        Debug.Log(a.name);
    }

    // Error: _______________________________________________
    private void Method03()
    {
        int x = testArray[5];
        Debug.Log(x);
    }

    // Error: _______________________________________________
    private void Method04()
    {
        string s = _items[10];
        Debug.Log(s);
    }

    // Error: _______________________________________________
    private void Method05()
    {
        int n = (int)_value;
        Debug.Log(n);
    }

    // Error: _______________________________________________
    private void Method06()
    {
        int d = 0;
        int result = 10 / d;
        Debug.Log(result);
    }

    // Error: _______________________________________________
    private void Method07()
    {
        Method07();
    }

    // Error: _______________________________________________
    private void Method08()
    {
        byte[] buffer = new byte[int.MaxValue];
        Debug.Log(buffer.Length);
    }

    // Error: _______________________________________________
    private void Method09()
    {
        DoThing();
    }

    private void DoThing()
    {
        throw new NotImplementedException();
    }

    // Error: _______________________________________________
    private void Method10()
    {
        testRigidBody = GetComponent<Rigidbody>();
        testRigidBody.AddForce(Vector3.up);
    }

    // Error: _______________________________________________
    private void Method11()
    {
        gameObject.SendMessage("Run");
    }

    // Error: _______________________________________________
    private void Method12()
    {
        GameObject found = GameObject.Find("Thing");
        Debug.Log(found.name);
    }

    // Error: _______________________________________________
    private void Method13()
    {
        GameObject a = new GameObject("A");
        DestroyImmediate(a);
        bool result = a.CompareTag("Player");
        Debug.Log(result);
    }

    // Error: _______________________________________________
    private void Method14()
    {
        checked
        {
            int n = int.MaxValue;
            int result = n + 1;
            Debug.Log(result);
        }
    }

    // Error: _______________________________________________
    private void Update()
    {
        string text = "Value: " + testArray[0].ToString();
        Debug.Log(text);
    }

    // Error: _______________________________________________
    private void Method15()
    {
        GameObject a = new GameObject("A");
        Transform t = a.transform;
        DestroyImmediate(a);
        Vector3 p = t.position;
        Debug.Log(p);
    }
}