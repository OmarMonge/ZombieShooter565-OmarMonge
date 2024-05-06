using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHand : MonoBehaviour
{

    [SerializeField]
    private BoxCollider handCollider;
    private int attackPoints;


    private void Awake()
    {
        handCollider.enabled = false;
        attackPoints = 10;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag "Zombie"
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            player.takeDamage(attackPoints);

            DeactivateCollider();
        }
    }


    public void ActivateCollider()
    {
        // Set the collider as active
        handCollider.enabled = true;
    }

    public void DeactivateCollider()
    {
        // Set the collider as inactive
        handCollider.enabled = false;
    }


}
