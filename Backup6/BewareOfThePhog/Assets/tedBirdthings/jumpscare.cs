using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class jumpscare : MonoBehaviour
{
    public float scare; // 1 = true, 0 = false
    public Animator animator;
    public Camera scareCam;
    public AudioSource scareAudio;
    public AudioClip scareNoise;

    // Start is called before the first frame update
    void Start()
    {
        scareAudio = GetComponent<AudioSource>();
        scareAudio.clip = scareNoise;
    }
     IEnumerator jumpscareCoroutine()
    {
        animator.SetFloat("jumpscare", 1f);

        scareAudio.PlayOneShot(scareNoise);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // Update is called once per frame
    void Update()
    {
        if (scareCam.enabled == true)
        {
            StartCoroutine(jumpscareCoroutine());
            
        }
    }
}
