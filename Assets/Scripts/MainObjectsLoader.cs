using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObjectsLoader : MonoBehaviour
{
    public GameObject UIScreen;
    public GameObject player;
    private GameObject playerStart;
    public GameObject gameMan;
    public GameObject audioMan;
    public GameObject battleMan;
    public SpawnPoint playerSpawnPoint;

    // Start is called before the first frame update
    void Awake()
    {
        //    if (ScreenFade.instance == null)
        // {
        //     ScreenFade.instance = Instantiate(UIScreen).GetComponent<ScreenFade>();
        // }

        if (PlayerController.instance == null)
        {
        PlayerController clone = Instantiate(player).GetComponent<PlayerController>();
        PlayerController.instance = clone;
        playerStart = GameObject.Find("PlayerStart");
        if (playerStart != null)
             {
             clone.transform.position = playerStart.transform.position;
             }
        }

        SetupScene();

        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(gameMan).GetComponent<GameManager>();
        }

        if (AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(audioMan).GetComponent<AudioManager>();
        }

        // if(BattleManager.instance == null)
        // {
        //     BattleManager.instance = Instantiate(battleMan).GetComponent<BattleManager>();
        // }
    }

    public void SpawnPlayer()
    {
        if (playerSpawnPoint != null)
        {
            GameObject player = playerSpawnPoint.SpawnObject();
        }
    }

    public void SetupScene()
    {
        if (PlayerController.instance == null)
        {
            SpawnPlayer();
        }
    }
}



