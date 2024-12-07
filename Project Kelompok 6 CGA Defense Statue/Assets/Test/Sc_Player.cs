using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Player : MonoBehaviour
{
    public float movementSpeed = 7.0f;
    public float jumpForce = 4.0f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public Sc_FreeCam fc;
    
    private bool canJump = true;
    private Rigidbody rb;
    private bool isGrounded;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        MovePlayer();
        Jump();
        Attacking();
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized;
        if (moveHorizontal != 0 || moveVertical !=0){
            if(anim.GetBool("attacking")==true) return;
            anim.SetBool("running", true);
            anim.SetInteger("condition", 1);
        }else if(movement == Vector3.zero){
            anim.SetBool("running", false);
            if(anim.GetBool("attacking")==false) anim.SetInteger("condition", 0);
        }

        if (movement != Vector3.zero && !fc.isShiftlock)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        transform.position += movement * movementSpeed * Time.deltaTime;
    }

    void Jump(){
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (isGrounded && Input.GetButton("Jump") && canJump){
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            canJump = false;
        }else if (isGrounded){
            canJump = true;
        }
    }

    void Attacking(){
        if(Input.GetMouseButtonDown(0)){
            if(anim.GetBool("running") == true){
                return;
            }else if(anim.GetBool("running")==false){
                StartCoroutine (AttackRoutine());
            }
        }
    }

    IEnumerator AttackRoutine(){
        anim.SetBool("attacking", true);
        anim.SetInteger("condition", 2);
        yield return new WaitForSeconds (1);
        anim.SetInteger("condition", 0);
        anim.SetBool("attacking", false);
    }
}
