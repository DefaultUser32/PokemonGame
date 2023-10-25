using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    [SerializeField] VideoClip startVideo;
    [SerializeField] VideoClip endVideo;

    VideoPlayer player;
    bool hasSwitched = false;
    private void Awake()
    {
        Screen.SetResolution(640, 480, true, 60);
    }
    private void Start()
    {
        player = GetComponent<VideoPlayer>();
        player.clip = startVideo;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || (long)player.frameCount <= player.frame)
        {
            if (!hasSwitched)
            {
                hasSwitched = true;
                player.clip = endVideo;
                return;
            }
            SceneManager.LoadScene("StartFight");
        }
    }
}
