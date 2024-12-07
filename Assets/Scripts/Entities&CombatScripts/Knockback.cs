using System.Collections;
using UnityEngine;

public class GradualKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;  // Initial force of the knockback
    public float knockbackDuration = 0.5f;  // Total duration of the knockback
    public LayerMask enemyLayer;  // Layer to identify enemies
    public AnimationCurve knockbackCurve;  // Animation curve for force over time

    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collides with an enemy
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0 && !isKnockedBack)
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            StartCoroutine(ApplyGradualKnockback(knockbackDirection));
        }
    }

    IEnumerator ApplyGradualKnockback(Vector2 direction)
    {
        isKnockedBack = true;

        float elapsedTime = 0f;

        while (elapsedTime < knockbackDuration)
        {
            // Calculate knockback force based on time elapsed
            float forceMultiplier = knockbackCurve.Evaluate(elapsedTime / knockbackDuration);
            Vector2 force = direction * knockbackForce * forceMultiplier;

            // Apply the knockback gradually
            //rb.velocity = force;
            rb.AddForce(direction * knockbackForce);

            elapsedTime += Time.deltaTime;
            yield return null;  // Wait for the next frame
        }

        // Stop movement after knockback ends
        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }
}