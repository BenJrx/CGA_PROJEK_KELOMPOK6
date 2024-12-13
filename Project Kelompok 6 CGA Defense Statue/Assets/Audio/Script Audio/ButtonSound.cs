using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioClip clickSound; // Masukkan file suara klik di Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Ambil referensi Audio Source pada GameObject ini
        audioSource = GetComponent<AudioSource>();
    }

   public void PlayClickSound()
{
    Debug.Log("Button Clicked!"); // Tambahkan ini untuk debug
    if (clickSound != null)
    {
        audioSource.PlayOneShot(clickSound);
        Debug.Log("Sound Played!"); // Tambahkan ini untuk memeriksa
    }
    else
    {
        Debug.LogWarning("Click sound not assigned!");
    }
}

}
