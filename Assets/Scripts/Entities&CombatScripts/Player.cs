using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player : Character
{
    public HitPoints hitPoints;
    public HealthBar healthBarPrefab;
    HealthBar healthBar;
    public Inventory inventory;
    [SerializeField] private SimpleFlash flashEffect;

    void Awake()
    {
        ResetCharacter();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;
            Debug.Log("Registered consumable");

            if (hitObject != null)
            {
                bool shouldDisappear = false;
                AudioManager.instance.PlaySFX(2);

                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        Debug.Log("Registered coin type");

                        shouldDisappear = inventory.AddItem(hitObject);
                        break;
                    case Item.ItemType.HEALTH:

                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;
                    default:
                        break;
                }

                if (shouldDisappear)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }

    public bool AdjustHitPoints(int amount)
    {
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            return true;
        }
        return false;
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            StartCoroutine(FlickerCharacter());
            flashEffect.Flash();
            AudioManager.instance.PlaySFX(0);
            hitPoints.value = hitPoints.value - damage;
            if (hitPoints.value <= float.Epsilon)
            {
                StartCoroutine(DeathDelay());
                //KillCharacter();
                break;
            }
            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }


    public override void KillCharacter()
    {
        base.KillCharacter();
        AudioManager.instance.PlaySFX(4);
        
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(2);
        Destroy(healthBar.gameObject);
        KillCharacter();
    }

    public override void ResetCharacter()
    {
        hitPoints.value = startingHitPoints;
        healthBar = Instantiate(healthBarPrefab);
        inventory = healthBar.GetComponent<Inventory>();
        healthBar.character = this;
    }
}