using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip slimeJump;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        slimeJump = Resources.Load<AudioClip>("slimeJump");
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip) {
            case "slimeJump":
                audioSrc.PlayOneShot(slimeJump);
                break;
        }
    }
}
