using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    public static Weapon instance;
    public GameObject ammoPrefab;
    static List<GameObject> ammoPool;
    public int poolSize;
    public float weaponVelocity;
    [HideInInspector]
    public bool isFiring;
    bool canAttack = true;
    [HideInInspector]
    public Animator animator;
    Camera localCamera;
    float positiveSlope;
    float negativeSlope;
    enum Quadrant
    {
        East,
        South,
        West,
        North
    }

    void Awake()
    {
        if (ammoPool == null)
        {
            ammoPool = new List<GameObject>();
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }

    void Start()
    {
        instance = this;
        animator = GetComponent<Animator>();
        isFiring = false;
        localCamera = Camera.main;

        Vector2 lowerLeft = localCamera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 upperRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 upperLeft = localCamera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 lowerRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0));

        positiveSlope = GetSlope(lowerLeft, upperRight);
        negativeSlope = GetSlope(upperLeft, lowerRight);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canAttack)
            {
                isFiring = true;
            }
            //FireAmmo();
        }

        UpdateState();
    }

    void UpdateState()
    {
        if (isFiring)
        {
            Vector2 quadrantVector;
            Quadrant quadEnum = GetQuadrant();
            switch (quadEnum)
            {
                case Quadrant.East:
                    quadrantVector = new Vector2(1.0f, 0.0f);
                    break;
                case Quadrant.South:
                    quadrantVector = new Vector2(0.0f, -1.0f);
                    break;
                case Quadrant.West:
                    quadrantVector = new Vector2(-1.0f, 0.0f);
                    break;
                case Quadrant.North:
                    quadrantVector = new Vector2(0.0f, 1.0f);
                    break;
                default:
                    quadrantVector = new Vector2(0.0f, 0.0f);
                    break;
            }

            animator.SetBool("isFiring", true);
            animator.SetFloat("moveX", quadrantVector.x);
            animator.SetFloat("moveY", quadrantVector.y);
            StartCoroutine(AttackTimer());
        }

        else
        {
            animator.SetBool("isFiring", false);
        }
    }

    void OnAttackEnd()
    {
        isFiring = false;
    }

    public IEnumerator AttackTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(1.1f);
        canAttack = true;
    }

    GameObject SpawnAmmo(Vector3 location)
    {
        foreach (GameObject ammo in ammoPool)
        {
            if (ammo.activeSelf == false)
            {
                ammo.SetActive(true);
                ammo.transform.position = location;
                return ammo;
            }
        }
        return null;
    }

    void FireAmmo()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject ammo = SpawnAmmo(transform.position);

        if (ammo != null)
        {
            Arc arcScript = ammo.GetComponent<Arc>();
            float travelDuration = 1.0f / weaponVelocity;
            StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
        }
    }

    void OnDestroy()
    {
        ammoPool = null;
    }

    float GetSlope(Vector2 pointOne, Vector2 pointTwo)
    {
        return (pointTwo.y - pointOne.y) / (pointTwo.x - pointOne.x);
    }

    bool HigherThanPositiveSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);
        float yIntercept = playerPosition.y - (positiveSlope * playerPosition.x);
        float inputIntercept = mousePosition.y - (positiveSlope * mousePosition.x);
        return inputIntercept > yIntercept;
    }

    bool HigherThanNegativeSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);
        float yIntercept = playerPosition.y - (negativeSlope * playerPosition.x);
        float inputIntercept = mousePosition.y - (negativeSlope * mousePosition.x);
        return inputIntercept > yIntercept;
    }

    Quadrant GetQuadrant()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerPosition = transform.position;

        bool higherThanPositiveSlopeLine = HigherThanPositiveSlopeLine(Input.mousePosition);
        bool higherThanNegativeSlopeLine = HigherThanNegativeSlopeLine(Input.mousePosition);

        if (!higherThanPositiveSlopeLine && higherThanNegativeSlopeLine)
        {
            return Quadrant.East;
        }
        else if (!higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.South;
        }
        else if (higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.West;
        }
        else
        {
            return Quadrant.North;
        }
    }
}