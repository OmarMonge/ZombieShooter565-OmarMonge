using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Vector2 turn;
    public CharacterController characterController;


    public GameObject crossHair;
    public GameObject crosshair1, crosshair2, crosshair3;

    public Weapons weapons;
    public int currentMelee;
    public int currentGun;

    public GameObject myHead;
    public Camera myCamera;
    public GameObject mainCamera;
    public GameObject aimCamera;
    //public GameObject upperBody;

    public LayerMask playerLayer;

    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    float idleZ = 1.0f;
    float idleX = 0.0f;
    public float acceleration = 10.0f;
    public float deceleration = 10.0f;
    public float maxWalkVelocity = 1.0f;
    public float maxRunVelocity = 2.0f;
    public float jumpSpeed;
    public float ySpeed;
    public float speed;


    public Rig rigAim;
    public float targetRigWeight;
    public GameObject forearmAim, handAim, handcontainerAim;
 

    public Vector3 prevSpeed;

    bool isGrounded;
    bool isJumping;
    bool isMoving;
    bool isCombatMode = false;


    bool forwardPressed;
    bool leftPressed;
    bool rightPressed;
    bool backwardPressed;
    bool runPressed;

    bool rightMouse;
    bool leftMouse;


    public GameObject target;

    public int healthPoints;
    private int prevHealthPoints;
   

    void Awake()
    {
        animator = GetComponent<Animator>();

        characterController = GetComponent<CharacterController>();
        currentMelee = -1;
        currentGun = -1;
        targetRigWeight = 0f;
        crossHair.SetActive(false);

        healthPoints = 100;
        prevHealthPoints = 100;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }



    void Update()
    {

      

        //Key Inputs
        forwardPressed = Input.GetKey("w");
        leftPressed = Input.GetKey("a");
        rightPressed = Input.GetKey("d");
        backwardPressed = Input.GetKey("s");
        runPressed = Input.GetKey("left shift");

        rightMouse = Input.GetKey(KeyCode.Mouse1);
        leftMouse = Input.GetKey(KeyCode.F);


      

        //if runPressed is true then use maxRunVelocity, if not then use maxWalkVelocity
        float currentMaxVelocity = runPressed ? maxRunVelocity : maxWalkVelocity;

        //is the character moving (walking or running) ????
        //useful when landing to know whether the animation should go to a moving animation or idle
        if (forwardPressed || leftPressed || rightPressed || backwardPressed)
        {
            animator.SetBool("isMoving", true);
            isMoving = true;
        }
        else
        {
            animator.SetBool("isMoving", false);
            isMoving = false;
        }

        //If any of the "wasd" keys are pressed, it will increase the magnitude of velocityX and VelocityZ
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }
        if (backwardPressed && velocityZ > -currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }

        //If any of the "wasd" keys ARE not pressed, it will decrease the magnitude of velocityX and velocityZ
        if (!forwardPressed && velocityZ >= 0.0f)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }
        if (!leftPressed && velocityX <= 0.0f)
        {
            velocityX += Time.deltaTime * acceleration;
        }
        if (!rightPressed && velocityX >= 0.0f)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        if (!backwardPressed && velocityZ <= 0.0f)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        //IF a directional key and running key are pressed while the current velocity is greater
        //than the currentMaxVelocity, set the velocity to the currentMax Velocity
        //
        //ELSE a directional key is pressed and a running key is not pressed, descelerate velocity to the walking velocity
        if (forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }
        if (backwardPressed && runPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ = -currentMaxVelocity;
        }
        else if (backwardPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * deceleration;
        }
        if (rightPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
        if (leftPressed && runPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
        }
        if (velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }


        //-----------------------
        //IDLE DIRECTION HANDLING
        //-----------------------
        if(forwardPressed && idleZ <= 1f)
        {
            idleZ += Time.deltaTime * acceleration;

        }
        if(backwardPressed && idleZ >= -1f && !rightMouse)
        {
            if (!leftPressed) idleZ -= Time.deltaTime * acceleration;
        }
        if(rightPressed && idleX <= 1f && !rightMouse)
        {
            idleX += Time.deltaTime * acceleration;
        }
        if(leftPressed && idleX >= -1f && !rightMouse)
        {
            idleX -= Time.deltaTime * acceleration;
        }
        if(forwardPressed || backwardPressed)
        {
            if (!rightPressed && idleX > 0f) idleX -= Time.deltaTime * deceleration;
            if (!leftPressed && idleX < 0f) idleX += Time.deltaTime * deceleration;
        }
        if(leftPressed || rightPressed)
        {
            if (!forwardPressed && idleZ > 0f) idleZ -= Time.deltaTime * deceleration;
            if (!backwardPressed && idleZ < 0f) idleZ += Time.deltaTime * deceleration;
        }
        //handle
        if (backwardPressed && idleZ <= 1f && rightMouse)
        {
            idleZ += Time.deltaTime * acceleration;
        }
        if (leftPressed && backwardPressed && idleX <= 1f && rightMouse)
        {
            idleX += Time.deltaTime * acceleration;
        }
        if (rightPressed && backwardPressed && idleX >= -1f && rightMouse)
        {
            idleX -= Time.deltaTime * acceleration;
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isJumping", true);
            isJumping = true;
            ySpeed = jumpSpeed;
            isGrounded = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("isJumping", false);
        }



        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (isJumping && !isGrounded)
        {
            Vector3 velocity = new Vector3(horizontalInput * currentMaxVelocity, ySpeed, verticalInput * currentMaxVelocity);
            Vector3 worldVelocity = transform.TransformDirection(velocity);
            ySpeed += Physics.gravity.y * Time.deltaTime;
            characterController.Move(worldVelocity * Time.deltaTime);
            if (isGrounded)
            {
                isJumping = false;
            }
        }


        if (characterController.velocity.x != 0f || characterController.velocity.y != 0f || characterController.velocity.z != 0f)
        {
            isGrounded = characterController.isGrounded;
            animator.SetBool("isGrounded", isGrounded);
        }
        if(characterController.velocity == new Vector3(0f, 0f, 0f))
        {
            animator.SetBool("isGrounded", true);
            isGrounded = true;
        }



        animator.SetFloat("VelocityZ", velocityZ);
        animator.SetFloat("VelocityX", velocityX);
        animator.SetFloat("IdleZ", idleZ);
        animator.SetFloat("IdleX", idleX);



        turn.x = Input.GetAxis("Mouse X");
        turn.y = Input.GetAxis("Mouse Y");

        RotatePlayer();
        changeWeapon();


        if(currentMelee == -1)
        {
            animator.SetBool("isMelee", false);
        } else {
            animator.SetBool("isMelee", true);
        }


        if(currentGun ==-1)
        {
            animator.SetBool("isHoldingGun", false);
            animator.SetBool("isHoldingAutoGun", false);
        }
        else
        {
            animator.SetBool("isHoldingGun", true);
            if (weapons.guns[currentGun].GetComponentInChildren<WeaponStat>().isAuto_gun)
            {
                animator.SetBool("isHoldingAutoGun", true);
            }
        }

        if(leftMouse)
        {
            animator.SetBool("leftMouse", true);
        }
        else
        {
            animator.SetBool("leftMouse", false);
        }

        //Aiming
        if (rightMouse && currentGun!= -1)
        {
            targetRigWeight = 1f;
            animator.SetBool("isAiming", true);
            mainCamera.SetActive(false);
            changeCrosshair();
            crossHair.SetActive(true);
        }
        //Not aiming
        else if(!rightMouse)
        {
            targetRigWeight = 0f;
            animator.SetBool("isAiming", false);
            mainCamera.SetActive(true);
            crossHair.SetActive(false);
        }
        //Not aiming
        else if (rightMouse && currentGun == -1)
        {
            targetRigWeight = 0f;
            animator.SetBool("isAiming", false);
            mainCamera.SetActive(true);
            crossHair.SetActive(false);
        }

        rigAim.weight = Mathf.Lerp(rigAim.weight, targetRigWeight, Time.deltaTime * 20f);



        Debug.Log("Prev Healthpoints: " + prevHealthPoints + ", Curr Healthpoints: " + healthPoints);
        if (healthPoints < prevHealthPoints)
        {
            animator.SetTrigger("tookDamage");
        }



        prevHealthPoints = healthPoints;
    }




    void RotatePlayer()
    {
        // Rotate the player based on mouse input horizontally
        float horizontalRotation = turn.x * 5f;
        transform.Rotate(Vector3.up, horizontalRotation);

        // Rotate the camera and head based on mouse input vertically
        float verticalRotation = -turn.y * 5f; // Inverted vertical rotation
        float currentRotation = myCamera.transform.localEulerAngles.x; // Get current vertical rotation

        // Calculate the new vertical rotation
        float newRotation = currentRotation + verticalRotation;

        if (newRotation > 330f || newRotation < 30f)
        {
            // Apply the new rotation to the camera
            myCamera.transform.localRotation = Quaternion.Euler(newRotation, 0f, 0f);
            mainCamera.transform.localRotation = Quaternion.Euler(newRotation, 0f, 0f);
            aimCamera.transform.localRotation = Quaternion.Euler(newRotation, 0f, 0f);

            // Ensure head's Y and Z rotations remain fixed
            Quaternion fixedHeadRotation = Quaternion.Euler(-newRotation, -90f, 180f);
            if (myHead != null)
            {
                myHead.transform.localRotation = fixedHeadRotation;
            }
        }
    }


    void changeWeapon()
    {
        int totalNumMelee = weapons.melee.Length;
        int totalNumGuns = weapons.guns.Length;

        if (Input.GetKeyDown(KeyCode.E))
        {

            if(currentGun != -1)
            {
                weapons.guns[currentGun].SetActive(false);
                currentGun = -1;
            }

            if(currentMelee == -1)
            {
                currentMelee++;
                weapons.melee[currentMelee].SetActive(true);
            }
            else if(currentMelee == totalNumMelee-1)
            {
                weapons.melee[currentMelee].SetActive(false);
                currentMelee = -1;
            }
            else
            {
                weapons.melee[currentMelee].SetActive(false);
                currentMelee++;
                weapons.melee[currentMelee].SetActive(true);
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(currentMelee != -1)
            {
                weapons.melee[currentMelee].SetActive(false);
                currentMelee = -1;
            }

            if (currentGun == -1)
            {
                currentGun++;
                weapons.guns[currentGun].SetActive(true);
            }
            else if (currentGun == totalNumGuns - 1)
            {
                weapons.guns[currentGun].SetActive(false);
                currentGun = -1;
            }
            else
            {
                weapons.guns[currentGun].SetActive(false);
                currentGun++;
                weapons.guns[currentGun].SetActive(true);
            }
        }
    }

    public int GetAttackPoints()
    {
        int attackP = 0;
        if (currentMelee != -1)
        {
            attackP = weapons.melee[currentMelee].GetComponent<WeaponStat>().attackPoints;
        }
        if (currentGun != -1)
        {
            attackP = weapons.guns[currentGun].GetComponent<WeaponStat>().attackPoints;
        }
        return attackP;
    }


    private void changeCrosshair()
    {
        //handgun, revolver, mac10
        if(currentGun == 0 || currentGun == 1 || currentGun == 5)
        {
            crosshair1.SetActive(true);
            crosshair2.SetActive(false);
            crosshair3.SetActive(false);
        }
        //shotgun, doublebarrel
        if(currentGun == 2|| currentGun == 3)
        {
            crosshair1.SetActive(false);
            crosshair2.SetActive(true);
            crosshair3.SetActive(false);
        }
        //m4
        if(currentGun == 4)
        {
            crosshair1.SetActive(false);
            crosshair2.SetActive(false);
            crosshair3.SetActive(true);
        }
    }

    public void hitZombie()
    {
        int attackPoints = GetAttackPoints();
        // Cast a ray from the mouse position
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~playerLayer))
        {
            // Check if the object hit has the tag "Zombie"
            if (hit.transform.tag.Equals("Zombie"))
            {
                Debug.Log("Zombie hit detected.");
                // Access the Zombie component on the hit object
                ZombieController zombie = hit.transform.GetComponent<ZombieController>();

                if (zombie != null)
                {
                    Debug.Log("Zombie component found.");
                    zombie.takeDamage(attackPoints);
                    Debug.Log("Damage: " + attackPoints);
                }
                else
                {
                    Debug.Log("Zombie component not found on the hit object");
                }
            }
            else
            {
                Debug.Log("Hit object is not tagged as Zombie.");
            }
        }
    }

    public void enableMeleeCollider()
    {
        if(currentMelee != -1)
        {
           WeaponStat melee = weapons.melee[currentMelee].GetComponentInChildren<WeaponStat>();
            melee.ActivateCollider();
        }
    }

    public void disableMeleeCollider()
    {
        if(currentMelee != -1)
        {
            WeaponStat melee = weapons.melee[currentMelee].GetComponentInChildren<WeaponStat>();
            melee.DeactivateCollider();
        }
    }


    public void disableHandAim()
    {
        forearmAim.SetActive(false);
        handAim.SetActive(false);
        handcontainerAim.SetActive(false);

    }

    public void enableHandAim()
    {
        forearmAim.SetActive(true);
        handAim.SetActive(true);
        handcontainerAim.SetActive(true);
    }



    public void takeDamage(int damagePoints)
    {
        healthPoints -= damagePoints;
    }
}


