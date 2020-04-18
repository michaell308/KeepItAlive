using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Brazier : MonoBehaviour
{
    public int checkPointNum;
    public bool holdingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.sceneLoaded += respawnPlayer;
        //DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void respawnPlayer(Scene scene, LoadSceneMode mode)
    {
        /*if (holdingPlayer)
        {
            var player = GameObject.FindWithTag("Player");
            player.transform.position = transform.position;
        }*/
    }
}
