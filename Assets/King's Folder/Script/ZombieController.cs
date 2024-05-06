using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public int healthPoints;
    public Animator animator;
    public CharacterController characterController;


    //public BoxCollider handCollider;

    public GameObject zombieHand;


    NavMeshAgent nm;
    public Transform target;

    public int prevHealthPoints;
    public bool isDead;

    private void Awake()
    {
        healthPoints = 100;
        prevHealthPoints = healthPoints;
        characterController.enabled = true;
        isDead = false;
        
        nm = GetComponent<NavMeshAgent>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        
        if (!isDead)
        {
            if (distance <= 100f)
            {
                nm.destination = target.position;

                if(distance <= 1.1f)
                {
                    animator.SetTrigger("Attack");
                }
            }
        }

        if(nm.velocity.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }


        if (healthPoints < prevHealthPoints)
        {
            animator.SetTrigger("tookDamage");
        }
        prevHealthPoints = healthPoints;



        if (healthPoints <= 0)
        {
            animator.SetBool("isDead", true);
            characterController.enabled = false;
            isDead = true;
            
        }
    }



    public void takeDamage(int damagePoints)
    {
        healthPoints -= damagePoints;
    }


    public void enableHandCollider()
    {
        zombieHand.GetComponentInChildren<ZombieHand>().ActivateCollider();
    }
    public void disableHandCollider()
    {
        zombieHand.GetComponentInChildren<ZombieHand>().DeactivateCollider();
    }
}
