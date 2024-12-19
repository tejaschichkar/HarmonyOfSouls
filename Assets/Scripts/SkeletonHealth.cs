using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonHealth : MonoBehaviour
{
    public int skeletonHealth = 10;  // Initial health of the skeleton
    public SkeletonsScript manager; // Reference to the main skeleton manager

    private Rigidbody rb;
    private Text healthText; // Reference to the Text component for health display

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent unwanted rotations
        }

        // Locate the HealthUI Text component under Canvas
        Transform canvasTransform = transform.Find("Canvas");
        if (canvasTransform != null)
        {
            Transform healthUITransform = canvasTransform.Find("HealthUI");
            if (healthUITransform != null)
            {
                healthText = healthUITransform.GetComponent<Text>();
                UpdateHealthUI(); // Initialize health text
            }
            else
            {
                Debug.LogWarning("HealthUI not found under Canvas for " + gameObject.name);
            }
        }
        else
        {
            Debug.LogWarning("Canvas not found for " + gameObject.name);
        }
    }

    void Update()
    {
        if (skeletonHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died.");

        if (manager != null)
        {
            manager.RemoveSkeleton(gameObject);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug message to confirm collision detection
        Debug.Log(gameObject.name + " triggered by " + other.gameObject.name);

        if (other.CompareTag("Sword"))
        {
            Debug.Log("Hit by Sword!");
            TakeDamage(5); // Reduce health by 5
        }
    }


    public void TakeDamage(int damage)
    {
        skeletonHealth -= damage;
        Debug.Log(gameObject.name + " took damage! Remaining health: " + skeletonHealth);

        UpdateHealthUI(); // Update the health text
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = skeletonHealth.ToString();
        }
        else
        {
            Debug.LogWarning("Health Text not assigned for " + gameObject.name);
        }
    }
}
