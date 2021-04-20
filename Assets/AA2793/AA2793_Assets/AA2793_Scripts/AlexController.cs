using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class AlexController : MonoBehaviour
{
    //Basic moving and jumping using the unity's imput system.
    //I am using this script for the game engine animation assignment.
    //Movemnt is finished but jum is not yet ready.
    //I also attach an screenshot to show you how ive done it in unity.

    Animator anim;
    Rigidbody rigidbody;

    Vector2 moveDirection; //Direction of the user input (x and y --> z)
    Vector2 lookDirection;
    float jumpDirection;

    public float moveSpeed = 2f; 
    public float maxForwardSpeed = 8f; //Max total speed.
    public float turnSpeed = 100f;
    float desiredSpeed; 
    float forwardSpeed; 

    const float groundAcceleration = 5f; //From idle to max speed.
    const float groundDesacceleration = 25; //Frm max speed to idle.

    bool readyJump = false;
    float jumpSpeed = 30000f;
    float jumpEffort = 0f;

    public Transform spine;
    Vector2 lastLookDirection;
    [SerializeField] float mouseXSensitivity = 0.5f;
    [SerializeField] float mouseYSensitivity = 0.5f;

    bool escapePressed = false;
    bool cursorIsLocked = true;

    bool onGround = true;
    float groundRayDist = 2f;
        
    public Transform weapon;
    public Transform rightHand;
    public Transform rightHip;

    public LineRenderer laser;
    public GameObject crosshair;
    public GameObject crosshairLight;

    bool firing = false;
        
    public bool isDead = false;
    int health = 100;

    //Bullets hitting Alex
    public void OnCollisionEnter(Collision collisioner)
    {
        if (collisioner.gameObject.tag == "Bullet")
        {
            health -= 10;
            anim.SetTrigger("BeingHit");

            if (health <= 0)
            {
                isDead = true;
                anim.SetLayerWeight(1, 0);
                anim.SetBool("Dead", true);
            }
        }
    }

    //Picking up gun
    public void PickUpGun()
    {
        weapon.SetParent(rightHand); //Attaching it to the hand.
        weapon.localPosition = new Vector3(-0.0248f, 0.0713f, 0.0258f); //TRansform --> postion
        weapon.localRotation = Quaternion.Euler(-92.153f, -418.595f, 165.68f); //Transform --> rotation
        weapon.localScale = new Vector3(1, 1, 1); //Transform --> Sacvel. Just to make sure it does not deform as some animation do.
    }

    //Putting down gun
    public void PutDownGun()
    {
        weapon.SetParent(rightHip);
        weapon.localPosition = new Vector3(-0.134f, -0.117f, -0.061f); 
        weapon.localRotation = Quaternion.Euler(-93.486f, -162.28f, -25.85101f); 
        weapon.localScale = new Vector3(1, 1, 1); 
    }

    //Checking if the key is being pressed or not.
    bool IsMoveInput
    {
        get { return !Mathf.Approximately(moveDirection.sqrMagnitude, 0f); }
        
    }

    //**************************************************************************
    //****EVENT CALLERS FOR INPUT SYSTEM**** OBJECT --> PLAYER INPUT --> EVENTS --> PLAYER

    //Imput system Move (WASD or Arrowkeys).
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();        
    }

    //Imput system Jump (Spacebar)
    public void OnJump(InputAction.CallbackContext context)
    {
        jumpDirection = context.ReadValue<float>();        
    }

    //Input system Fire (Left Mouse)
    public void OnFire(InputAction.CallbackContext context)
    {
        firing = false;
        if ((int)context.ReadValue<float>() == 1) //Mouse click has up and down (0, 1)
        {
            anim.SetTrigger("Fire");
            firing = true;

        }
    }

    //Input system ArmWeapon (Q)
    public void OnArmed(InputAction.CallbackContext context)
    {
        anim.SetBool("Armed", !anim.GetBool("Armed")); //If its true it will put to false and viceversa.
    }


    //Input system Look (Mouse movement)
    public void OnLook(InputAction.CallbackContext context)
    {
        lookDirection = context.ReadValue<Vector2>();
    }

    //Input system Cancel (ESC)
    public void OnEscape (InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            escapePressed = true;
        }
        else
        {
            escapePressed = false;
        }
    }

    //*****************************************************************************
    
    //Locking the mouse until ESC is pressed.
    public void UpdateCursorLock()
    {
        if (escapePressed)
        {
            cursorIsLocked = false;
        }
        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    //Moving Alex
    void Move(Vector2 direction)
    {
        float forwardDirection = direction.y;
        float turnDirection = direction.x;
        if(direction.sqrMagnitude > 1f)
        {
            direction.Normalize(); //Scale value for the speed.
        }

        desiredSpeed = direction.magnitude * maxForwardSpeed * Mathf.Sign(forwardDirection);
        float acceleration = IsMoveInput ? groundAcceleration : groundDesacceleration;

        forwardSpeed = Mathf.MoveTowards(forwardSpeed, desiredSpeed, acceleration * Time.deltaTime);
        //Debug.Log("Forward speed is: " + forwardSpeed);
        //id "ForwardSpeed" == parameter in animator --> (Base Layer, Movement, Move)
        anim.SetFloat("ForwardSpeed", forwardSpeed);

        transform.Rotate(0, turnDirection * turnSpeed * Time.deltaTime, 0);
               
    }

    //Jumping
    void Jump(float direction)
    {
        if(direction > 0 && onGround)   
        {
            anim.SetBool("ReadyJump", true);
            readyJump = true;
            jumpEffort += Time.deltaTime;
        }
        else if (readyJump)
        {
            anim.SetBool("Launch", true); //Realease of spacebar!!
            readyJump = false;
            anim.SetBool("ReadyJump", false);
        }
        //Debug.Log("Jump Effort:" + jumpEffort);
    }


    //****ANIMATIONS**** ANIMATOR --> PARAMETERS

    public void Launch() // This code is going to be triggered in an specific time moment of the AlexJumpUpEditable animation!! 
    {
        rigidbody.AddForce(0, jumpSpeed * Mathf.Clamp(jumpEffort, 1, 3), 0);

        //Adding some forward force so can from idle --> jump forward
        rigidbody.AddForce(this.transform.forward * forwardSpeed * 500);

        anim.SetBool("Launch", false);
        anim.applyRootMotion = false;

        //Making sure that once hit spacebar, character is not on the ground animore
        onGround = false;
    }

    public void Land()
    {
        anim.SetBool("Land", false);
        anim.applyRootMotion = true;
        anim.SetBool("Launch", false);
        jumpEffort = 0f;
    }



    private void Awake()
    {
        //Assigning animator to anim
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    //This is going to happen as the final thing of the environments main loop (It happens after the animations have been applied)
    private void LateUpdate()
    {
        //Is dead, let it go.
        if (isDead)
        {
            return;
        }

        if (anim.GetBool("Armed"))
        {
            lastLookDirection += new Vector2(-lookDirection.y * mouseYSensitivity, lookDirection.x * mouseXSensitivity); //Flipping it for the rotation1!! Remember normal = x, y ,z and minus -y!!
            lastLookDirection.x = Mathf.Clamp(lastLookDirection.x, -30, 30);
            lastLookDirection.y = Mathf.Clamp(lastLookDirection.y, -30, 60);

            spine.localEulerAngles = lastLookDirection;
        }            
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateCursorLock(); //Listening for ESC

        //Is dead, let it go.
        if (isDead)
        {
            return;
        }

        Move(moveDirection); //Listening for WASD
        Jump(jumpDirection); //Listening for Escapebar

        //Activating the laser effect when gun is Armed (Q)
        if (anim.GetBool("Armed"))
        {
            laser.gameObject.SetActive(true);
            //crosshair.gameObject.SetActive(true);
            crosshairLight.gameObject.SetActive(true);

            //Laser from the gun
            RaycastHit laserHit;
            Ray laserRay = new Ray(laser.transform.position, laser.transform.forward);
            if (Physics.Raycast(laserRay, out laserHit))
            {
                laser.SetPosition(1, laser.transform.InverseTransformPoint(laserHit.point));
                
                crosshairLight.transform.localPosition = new Vector3(0, 0, laser.GetPosition(1).z * 0.9f);

                //"Shooting" the orbs to make them explode
                if (firing && laserHit.collider.gameObject.tag == "Orb")
                {
                    laserHit.collider.gameObject.GetComponent<AIController>().BlowUp();
                }
            }
            else
            {
                //crosshair.gameObject.SetActive(false);
                crosshairLight.gameObject.SetActive(false);
            }
        }
        else
        {
            laser.gameObject.SetActive(false);
            //crosshair.gameObject.SetActive(false);
            crosshairLight.gameObject.SetActive(false);

        }       


        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * groundRayDist * 0.5f, -Vector3.up);
        if(Physics.Raycast(ray, out hit, groundRayDist))
        {
            if (!onGround)
            {
                onGround = true;
                anim.SetFloat("LandingVelocity", rigidbody.velocity.magnitude);
                anim.SetBool("Land", true);
                anim.SetBool("Falling", false);
            }            
        }
        else
        {
            onGround = false;
            anim.SetBool("Falling", true);
            anim.applyRootMotion = false;

        }
     //   Debug.DrawRay(transform.position + Vector3.up * groundRayDist * 0.5f, -Vector3.up * groundRayDist, Color.red);
    }
}
