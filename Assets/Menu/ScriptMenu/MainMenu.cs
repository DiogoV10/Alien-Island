using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{


    private const string LOAD_SAVE_KEY = "LoadSave";


    public GameObject[] _objectsToDisable;
    public GameObject _cinematicVideo;
    [SerializeField] private VideoClip _secondVideoClip;
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

    public void LoadGame()
    {
        PlayerPrefs.SetInt(LOAD_SAVE_KEY, 1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    IEnumerator PlaySecondClip()
    {
        if (!_secondVideoClip)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            yield return null;
        }

        _videoPlayer.clip = _secondVideoClip;
        _videoPlayer.Play();
        yield return new WaitForSeconds((float)_videoPlayer.clip.length + .2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds((float)_videoPlayer.clip.length - 2.45f);
        StartCoroutine(nameof(PlaySecondClip));
    }
}
