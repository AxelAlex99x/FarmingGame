using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class NewBehaviourScript : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    private float moveSpeed = 2f;


    [Header("Movement System")]
    public float walkSpeed = 2f;
    public float runSpeed = 3.4f;

    PlayerInteraction playerInteraction;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInteraction = GetComponentInChildren<PlayerInteraction>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Interact();

        if (Input.GetKey(KeyCode.RightAlt))
        {
            TimeManager.Instance.Tick();
        }
    }

    public void Interact()
    {
        if (Input.GetButtonDown("InventoryToggle"))
        {
            UIManager.Instance.ToggleInventoryPanel();
        }

        if (UIManager.Instance.inventoryPanel.activeSelf)
        {       
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            playerInteraction.Interact();
        }
        
        if(Input.GetButtonDown("Fire2"))
        {
            playerInteraction.ItemInteract();
        }
    }
    public void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 velocity = moveSpeed * Time.deltaTime * direction;


        if (!characterController.isGrounded)
        {
            velocity.y -= 9.81f * Time.deltaTime; 
        }
       

        if (Input.GetButton("Sprint")){ 
            moveSpeed = runSpeed;
            animator.SetBool("Running", true);
        }
        else
        {
            moveSpeed = walkSpeed;
            animator.SetBool("Running", false);
        }


        if (direction.magnitude > 0.1f ) 
        {
            transform.rotation = Quaternion.LookRotation(direction);
            characterController.Move(velocity);
            animator.SetFloat("Speed", velocity.magnitude);
        }
        
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }
  
}
