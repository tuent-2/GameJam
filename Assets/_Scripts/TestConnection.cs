using UnityEngine;

public class TestConnection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SmartFoxConnection.Instance.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
