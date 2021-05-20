using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForGameController : MonoBehaviour
{
    public GameObject PauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.activeSelf)
            {
                Time.timeScale = 0;
                PauseMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                PauseMenu.SetActive(true);
            }
        }
        
    }
}
