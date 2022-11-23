using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private CharacterController cc;
    public float Velocidad = 12;

    public float Gravedad = -9.81f;
    private Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask floorMask;
    private bool isGrounded;

    private void Start()
    {
        cc = transform.gameObject.GetComponent<CharacterController>();
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, floorMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButton("Jump") /*&& isGrounded*/)
        {
            velocity.y = Mathf.Sqrt(0.5f * -2 * Gravedad);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        cc.Move(move * Velocidad * Time.deltaTime);


        velocity.y += Gravedad * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

    }
}
