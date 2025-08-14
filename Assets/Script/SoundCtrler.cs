using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCtrler : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip dropSE;
    [SerializeField] AudioClip captureSE;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DropSound()
    {
        audioSource.PlayOneShot(dropSE);
    }
    public void CaptureSound()
    {
        audioSource.PlayOneShot(captureSE);
    }
}
