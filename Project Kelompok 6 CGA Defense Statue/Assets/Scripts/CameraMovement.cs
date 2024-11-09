using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // Kecepatan pergerakan kamera
    public float rotationSpeed = 100f;  // Kecepatan rotasi kamera dengan keyboard
    public float mouseSensitivity = 2f; // Sensitivitas pergerakan kamera dengan mouse

    private float xRotation = 0f;       // Variabel untuk menyimpan rotasi X (vertikal)

    void Update()
    {
        // Menggerakkan kamera ke depan dan belakang dengan W dan S
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
        }

        // Menggerakkan kamera ke kiri dan kanan dengan A dan D
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }

        // Mengatur rotasi kamera dengan tombol panah kiri dan kanan
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        // Mengatur rotasi kamera dengan tombol panah atas dan bawah untuk tilt
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.right, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        }

        // Mengatur rotasi kamera dengan gerakan mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Update rotasi vertikal (X) dan batasi sudut pandang agar tidak terlalu jauh ke atas atau bawah
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Membatasi rotasi vertikal

        // Rotasi kamera
        transform.localRotation = Quaternion.Euler(xRotation, transform.localRotation.eulerAngles.y + mouseX, 0f);
    }
}
