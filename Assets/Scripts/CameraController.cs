using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Tilemap tilemap;
    private Vector3 boundary1;
    private Vector3 boundary2;
    private float halfHeight;
    private float halfWidth;

    public int musicToPlay;
    public bool musicStarted;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        if (tilemap != null)
        {
            tilemap.CompressBounds();

            boundary1 = tilemap.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);
            boundary2 = tilemap.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0f);

            PlayerController.instance.SetBounds(tilemap.localBounds.min, tilemap.localBounds.max);
        }
    }


    void LateUpdate()
    {
       // player = FindObjectOfType<PlayerController>().transform;
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }

        //keep the camera inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundary1.x, boundary2.x), Mathf.Clamp(transform.position.y, boundary1.y, boundary2.y)
    , transform.position.z);

        if (!GameManager.instance.cutSceneMusicActive && !GameManager.instance.battleActive)
        {
            if (!musicStarted)
            {
                musicStarted = true;
                AudioManager.instance.PlayBGM(musicToPlay);
            }


            if (!AudioManager.instance.bgm[musicToPlay].isPlaying)
            {
                musicStarted = false;
            }
        }

    }
}

