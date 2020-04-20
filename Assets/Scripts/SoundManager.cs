using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip slimeJump, braizerOn, torchSwing1, torchSwing2, slimeSquish, extinguish, music;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        slimeJump = Resources.Load<AudioClip>("slimeJump");
        braizerOn = Resources.Load<AudioClip>("bonfire_rev2");
        torchSwing1 = Resources.Load<AudioClip>("torch_dist");
        torchSwing2 = Resources.Load<AudioClip>("torch_base");
        slimeSquish = Resources.Load<AudioClip>("squelchy_squirt");
        extinguish = Resources.Load<AudioClip>("steam_iron");
        music = Resources.Load<AudioClip>("lightless-dawn-by-kevin-macleod-from-filmmusic-io");
        audioSrc = GetComponent<AudioSource>();
        //audioSrc.PlayOneShot(music);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip, float volume = 1.0f)
    {
        switch (clip) {
            case "slimeJump":
                audioSrc.PlayOneShot(slimeJump);
                break;
            case "braizerOn":
                audioSrc.PlayOneShot(braizerOn);
                break;
            case "torchSwing1":
                audioSrc.PlayOneShot(torchSwing1, 0.25f);
                break;
            case "torchSwing2":
                audioSrc.PlayOneShot(torchSwing2, 0.25f);
                break;
            case "slimeSquish":
                audioSrc.PlayOneShot(slimeSquish);
                break;
            case "extinguish":
                audioSrc.PlayOneShot(extinguish, 0.5f);
                break;
        }
    }
}
