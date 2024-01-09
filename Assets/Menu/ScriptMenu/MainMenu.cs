using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public GameObject[] _objectsToDisable;
    public GameObject _cinematicVideo;
    private VideoPlayer _videoPlayer;

    private void Awake()
    {
        _videoPlayer = _cinematicVideo.GetComponent<VideoPlayer>();
    }
    public void PlayGame()
    {
        foreach (var obj in _objectsToDisable)
        {
            obj.SetActive(false);
        }
        _cinematicVideo.SetActive(true);
        StartCoroutine(LoadGameScene());
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds((float)_videoPlayer.clip.length-2.45f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}