using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject startOpen;
    [SerializeField] Animator startgameanim;
    [SerializeField] Toggle toggleEasy;
    [SerializeField] Toggle toggleDifficulty;
    public bool easy;
    public bool difficulty;
    private void Awake()
    {
        startOpen.SetActive(false);
        easy = true;
        difficulty = false;
        toggleEasy.isOn = easy;
        toggleDifficulty.isOn = difficulty;
    }
    public void StartMenuOpen()
    {
        startgameanim.SetTrigger("startgame");
        startOpen.SetActive(true);
    }
    public void StartMenuClose()
    {
        startgameanim.SetTrigger("close");
        startOpen.SetActive(false);
    }
    public void OpenEasy()
    {
        //toggleEasy.isOn = true;
        toggleDifficulty.isOn = false;
        easy = true;
        difficulty = false;
    }
    public void OpenDifficulty()
    {
        toggleEasy.isOn = false;
        //toggleDifficulty.isOn = true;
        easy = false;
        difficulty = true;
    }
}
