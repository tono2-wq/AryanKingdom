using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rigidBody;
    [HideInInspector]
    public Animator animator;
    public float moveSpeed;
    Vector2 moveDir = new Vector2();
    Vector2 lastMoveDir;

    private Vector3 boundary1;
    private Vector3 boundary2;
    public bool canMove = true;
    public string areaTransitionName;
    private GameObject playerStart;

    //Make instance of this script to be able reference from other scripts!
    public static PlayerController instance;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        areaTransitionName = "";
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnSceneChange(Scene current, Scene next)
    {
        playerStart = GameObject.Find("PlayerStart");
        if (playerStart != null)
        {
            transform.position = playerStart.transform.position;
        }
    }


    void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");

        moveDir.Normalize();

        bool isIdle = moveDir.x == 0 && moveDir.y == 0;

        if (!canMove)
        {
            isIdle = true;
        }

        if (isIdle)
        {
            rigidBody.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }
        else
        {
            lastMoveDir = moveDir;
            rigidBody.velocity = moveDir * moveSpeed;
            animator.SetFloat("moveX", moveDir.x);
            animator.SetFloat("moveY", moveDir.y);
            animator.SetBool("isWalking", true);
        }
    }

    //Method to set up the bounds which the player can not cross
    public void SetBounds(Vector3 bound1, Vector3 bound2)
    {
        boundary1 = bound1 + new Vector3(.5f, 1f, 0f);
        boundary2 = bound2 + new Vector3(-.5f, -1f, 0f);
    }




}