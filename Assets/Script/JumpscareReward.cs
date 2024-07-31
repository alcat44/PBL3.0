using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpscareReward : MonoBehaviour
{
    public AudioSource scareSound;
    public Collider collision; // Collider yang digunakan sebagai trigger jumpscare
    public GameObject Jumpscare;
    public TextMeshProUGUI conversationText; // TextMeshPro untuk percakapan
    public GameObject rewardObject; // Game object yang akan diaktifkan setelah jumpscare
    public MonoBehaviour playerMovementScript; // Script movement player

    void Start()
    {
        collision.enabled = false; // Pastikan collider dinonaktifkan pada awalnya
        conversationText.gameObject.SetActive(false); // Pastikan teks percakapan tidak aktif di awal
        rewardObject.SetActive(false); // Pastikan rewardObject tidak aktif di awal
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Jumpscare.SetActive(true);
            collision.enabled = false; // Nonaktifkan collider setelah trigger
            PlayScareSound();
            DisablePlayerMovement(); // Nonaktifkan movement player
            StartCoroutine(JumpscareCoroutine()); // Mulai jumpscare dengan penundaan
        }
    }

    void PlayScareSound()
    {
        if (scareSound != null)
        {
            scareSound.Play();
        }
    }

    void DisablePlayerMovement()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false; // Nonaktifkan script movement player
        }
    }

    void EnablePlayerMovement()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true; // Aktifkan kembali script movement player
        }
    }

    // Fungsi untuk mengaktifkan collider jumpscare
    public void ActivateJumpscareTrigger()
    {
        collision.enabled = true;
    }

    IEnumerator JumpscareCoroutine()
    {
        yield return new WaitForSeconds(2.0f); // Penundaan 2 detik sebelum jumpscare selesai

        Jumpscare.SetActive(false); // Nonaktifkan jumpscare setelah selesai
        EnablePlayerMovement(); // Aktifkan kembali movement player
        rewardObject.SetActive(true); // Aktifkan rewardObject setelah jumpscare selesai
    }
}
