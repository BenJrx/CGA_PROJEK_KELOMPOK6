using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoSceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Referensi ke VideoPlayer
    public string nextScene = "GameScene";  // Nama scene yang ingin dimuat setelah video selesai

    void Start()
    {
        // Pastikan video diputar hanya saat scene dimulai
        if (videoPlayer != null)
        {
            // Set event untuk mendeteksi video selesai
            videoPlayer.loopPointReached += CheckVideoFinished;
            videoPlayer.Play(); // Mulai video
        }
        else
        {
            Debug.LogError("VideoPlayer not assigned!");
        }
    }

    // Fungsi callback untuk memeriksa apakah video selesai
    private void CheckVideoFinished(VideoPlayer vp)
    {
        // Setelah video selesai, pindah ke scene berikutnya
        Debug.Log("Video finished. Loading next scene.");
        SceneManager.LoadScene(nextScene);
    }
}
