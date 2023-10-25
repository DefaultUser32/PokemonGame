using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip road;
    [SerializeField] AudioClip battle;

    AudioSource source;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
        source.clip = road;
        source.Play();
    }
    public void SwitchTrack()
    {
        source.clip = battle;
        source.Play();
    }

}
