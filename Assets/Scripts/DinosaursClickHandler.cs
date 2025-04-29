using UnityEngine;
using TMPro; // Untuk menggunakan TMP_Text
using System.Collections;

public class DinosaursClickHandler : MonoBehaviour
{
    private GameObject inspectorWorld; // Ini adalah objek yang juga berfungsi sebagai button
    private Animator inspectorWorldAnimator; // Animator untuk mengontrol animasi fade

    private bool isButtonActive = false; // Status tombol, mulai dengan tidak aktif
    private float clickCooldown = 1f; // Cooldown 1 detik agar tidak double click
    private float lastClickTime = -1f; // Waktu terakhir klik

    // Start is called before the first frame update
    void Start()
    {
        // Cari objek Canvas yang bernama "Canvas"
        Canvas canvas = GameObject.Find("Canvas")?.GetComponent<Canvas>();

        if (canvas != null)
        {
            // Cari objek anak bernama "InspectorWorld" di dalam Canvas
            inspectorWorld = canvas.transform.Find("InspectorWorld")?.gameObject;

            // Ambil komponen Animator dari inspectorWorld
            inspectorWorldAnimator = inspectorWorld?.GetComponent<Animator>();

            // Pastikan inspectorWorld (yang juga merupakan button) tidak aktif di awal
            if (inspectorWorld != null)
            {
                inspectorWorld.SetActive(false); // Set awal objek tidak aktif
            }
            else
            {
                Debug.LogError("InspectorWorld tidak ditemukan di dalam Canvas!");
            }
        }
        else
        {
            Debug.LogError("Canvas tidak ditemukan di dalam scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Cek jika sudah cukup waktu (Cooldown) sejak klik terakhir
        if (Time.time - lastClickTime < clickCooldown)
        {
            return; // Jika belum 1 detik, tidak lakukan apa-apa
        }

        // Cek jika model dinosaurus diklik (menggunakan raycast)
        if (Input.GetMouseButtonDown(0)) // 0 berarti klik kiri mouse
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Cek apakah ray mengenai dinosaurus ini
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // Set waktu terakhir klik
                    lastClickTime = Time.time;

                    // Toggle status aktif untuk inspectorWorld
                    isButtonActive = !isButtonActive;

                    // Tampilkan atau sembunyikan InspectorWorld dengan animasi fade
                    if (inspectorWorld != null)
                    {
                        if (isButtonActive)
                        {
                            inspectorWorld.SetActive(true); // Aktifkan sebelum animasi fade-in
                            inspectorWorldAnimator?.Play("InspectorFadeIn"); // Mulai animasi fade-in
                        }
                        else
                        {
                            inspectorWorldAnimator?.Play("InspectorFadeOut"); // Mulai animasi fade-out
                            StartCoroutine(WaitForFadeOutAndDisableButton());
                        }
                    }
                }
            }
        }
    }

    // Coroutine untuk menunggu animasi fade-out selesai
    private IEnumerator WaitForFadeOutAndDisableButton()
    {
        // Tunggu hingga animasi fade-out selesai
        yield return new WaitForSeconds(inspectorWorldAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Setelah fade-out selesai, sembunyikan objek
        if (!isButtonActive)
        {
            inspectorWorld.SetActive(false);
        }
    }
}
