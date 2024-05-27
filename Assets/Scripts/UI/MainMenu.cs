using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;
using System;

public class MainMenu : MonoBehaviour
{
    public Button loadGameButton;
    
    [SerializeField]
    AudioSource music;

    public void NewGame()
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, null));
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, LoadGame));
    }

    void LoadGame()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.Log("Cannot Find Game State Manager");
            return;
        }

        GameStateManager.Instance.LoadSave();
    }

    public void ToggleMusic()
    {
        if (music.isPlaying)
        {
            OffMusic();
        }
        else
        {
            OnMusic();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadGameAsync(SceneTransitionManager.Location scene, Action onFirstFrameLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());

        DontDestroyOnLoad(gameObject);

        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading");
        }

        Debug.Log("Loaded");

        yield return new WaitForEndOfFrame();
        Debug.Log("First frame is loaded");

        onFirstFrameLoad?.Invoke();

        Destroy(gameObject);
    }

    public void OnMusic()
    {
        music.UnPause();
    }

    public void OffMusic()
    {
        music.Pause();
    }

    // Start is called before the first frame update
    void Start()
    {
        loadGameButton.interactable = SaveManager.HasSave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
