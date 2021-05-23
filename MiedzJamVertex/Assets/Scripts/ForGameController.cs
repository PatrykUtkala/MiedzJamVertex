using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForGameController : MonoBehaviour
{

    public GameObject PauseMenu;
    public GameObject PauseSettings;
    public GameObject FinishImage;
    public GameObject GamePart;
    public GameObject InGameButtons;
    public GameObject[] Slides;
    public bool[] Levelsbeaten = {false};
    public int playableLevels = 1;
    public int currentLevel = 0;
    public Button[] Levelbuttons;
    public GameObject[] PCBs;
    public GameObject Boardposition;

    GameObject currentPCB;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        setButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !FinishImage.activeSelf)
        {
            if (PauseMenu.activeSelf || PauseSettings.activeSelf)
            {
                Time.timeScale = 1;
                PauseMenu.SetActive(false);
                PauseSettings.SetActive(false);
                GamePart.SetActive(true);
            }
            else
            {
                Time.timeScale = 0;
                PauseMenu.SetActive(true);
                GamePart.SetActive(false);
            }
        }
    }

    void LoadPCB(int pcb)
    {
        Destroy(currentPCB);
        currentPCB = Instantiate(PCBs[pcb], Boardposition.transform.position, Quaternion.identity);
    }
    
    void DisableButton(int index)
    {
        Levelbuttons[index].interactable = false;
        ColorBlock colors = Levelbuttons[index].colors;
        colors.normalColor = Color.white;
        Levelbuttons[index].colors = colors;
    }

    void FinishLevelButton(int index)
    {
        Levelsbeaten[index] = true;
        Levelbuttons[index].interactable = false;
        ColorBlock colors = Levelbuttons[index].colors;
        colors.disabledColor = Color.green;
        Levelbuttons[index].colors = colors;
    }

    void EnableButton(int index)
    {
        Levelbuttons[index].interactable = true;
        ColorBlock colors = Levelbuttons[index].colors;
        colors.normalColor = Color.red;
        Levelbuttons[index].colors = colors;
    }



    private void EnableNextSet()
    {

        if (checkForNextSet())
        {
            playableLevels += 1;
            setButtons();
        }
        
    }

    private bool checkForNextSet()
    {
       
        for (int i = 0; i < playableLevels; i++)
        {
            if (!Levelsbeaten[i]) return false;

        }
        return true;
    }

    bool checkForFinish()
    {
        for (int i = 0; i < Levelsbeaten.Length; i++)
        {
            if (!Levelsbeaten[i]) return false;

        }
        return true;
    }

    void disableSliders()
    {
        for(int i = 0; i < Slides.Length; i++)
        {
            Slides[i].SetActive(false);
        }
    }

    void enableSliders()
    {
        for (int i = 0; i < Slides.Length; i++)
        {
            Slides[i].SetActive(true);
        }
    }


    public void setButtons()
    {
        for(int i = 0; i < Levelsbeaten.Length; i++)
        {
            if(i < playableLevels)
            {
                if (Levelsbeaten[i])
                {
                    FinishLevelButton(i);
                }
                else
                {
                    EnableButton(i);
                }
            }
            else
            {
                DisableButton(i);
            }

        }

    }

    public void restartLevel()
    {
        LoadPCB(currentLevel);
    }

    public void playLevel(int index)
    {
        currentLevel = index;
        LoadPCB(index);
        for (int i = 0; i < Levelbuttons.Length; i++)
        {
            DisableButton(i);
        }
        InGameButtons.SetActive(true);
        enableSliders();
    }

    public void FinishLevel()
    {
        if (currentPCB)
        {
            FinishLevelButton(currentLevel);
            Levelsbeaten[currentLevel] = true;
            if (checkForFinish())
            {
                finishGame();
            }
            EnableNextSet();
            exitLevel();
        }
        disableSliders();

    }

    public void exitLevel()
    {
        setButtons();
        Destroy(currentPCB);
        InGameButtons.SetActive(false);
        disableSliders();
    }

    public void finishGame()
    {
        FinishImage.SetActive(true);
        GamePart.SetActive(false);
        PauseMenu.SetActive(false);
    }

}



