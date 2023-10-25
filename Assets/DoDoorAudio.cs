using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDoorAudio : MonoBehaviour
{
    public void PlayDoorSound()
    {
        GetComponent<AudioSource>().Play();
    }
    public void StopSound()
    {
        GetComponent<AudioSource>().Stop();
    }
}
