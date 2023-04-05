using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private CharacterController controller;
    public float playerSpeed;
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    
    
    // GroundCheck
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f; //Umbral de distància enterra
    public LayerMask ground;
    
    //jump
    public float jumpHeight = 2f;

    public PhotonView photonview;
    
    //gravity
    private Vector3 playerVelocity;
    public float gravity = -9.81f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom && !photonview.IsMine)
        {
            return;
        }
        
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,ground);
        
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * playerSpeed * Time.deltaTime);

        
        

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
        if (Input.GetButton("Fire3") && isGrounded)
        {
            playerSpeed = runSpeed;
        }
        else
        {
            playerSpeed = walkSpeed;
        }
    }
}
