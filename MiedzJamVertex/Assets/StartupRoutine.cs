using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupRoutine : MonoBehaviour
{
    public GameObject MainMenu;
    float startTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startTime + 3.5)
        {
            gameObject.SetActive(false);
            MainMenu.SetActive(true);
        }
        
    }
}
