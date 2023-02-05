using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class manager : MonoBehaviour
{
    [SerializeField] private int num_of_collectables;
    private int collectables;
    private void Awake()
    {
        // finds if there is an older InfoManager
        manager[] gm = FindObjectsOfType<manager>();
        if (gm.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    
    [Range(0,1)]
    public float sound_master_strength;
    [Range(0,1)]
    public float sound_sfx_strength;
    [Range(0,1)]
    public float sound_music_strength;
    
    public void GameExit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void SliderValue(string slider_name)
    {
        Slider slider = GameObject.Find(slider_name).GetComponent<Slider>();
        
        switch (slider_name)
        {
            case "Master":
                sound_master_strength = slider.value;
                break;
            case "Sfx":
                sound_sfx_strength = slider.value;
                break;
            case "Music":
                sound_music_strength = slider.value;
                break;
            default:
                return;
        }
    }

    private void FixedUpdate()
    {
        if(collectables >= num_of_collectables) LoadSceneByName("Win");
    }

    public void OnCollect()
    {
        collectables++;
    }
}