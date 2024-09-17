using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 camRotation;

  

    Rigidbody myRB;
    Camera playerCam;

    [Header("movement stats")]
    public bool sprinting = false;
    public float speed = 10f;
    public float jumpHeight = 5f;

    public float grounddetection = 1f;

    [Header("user settings")]
    public float sprintMult = 1.5f;
    public bool sprintToggle = false;
    public float mouseSensitivity = 2.0f;
    public float XSensitivity = 2.0f;
    public float YSensitivity = 2.0f;
    public float camRotationLimit = 90f;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        playerCam = transform.GetChild(0).GetComponent<Camera>();

        camRotation = Vector2.zero;
  
        Cursor.visible = false;

    }



    // Update is called once per frame
    void Update()
    {
        


        {

         
           

            camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

            camRotation.y = Mathf.Clamp(camRotation.y, -90, 90);

            playerCam.transform.localRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);
            transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);


            Vector3 temp = myRB.velocity;

            temp.x = Input.GetAxisRaw("Horizontal") * speed;
            temp.z = Input.GetAxisRaw("Vertical") * speed;

            if (!sprinting && !sprintToggle && Input.GetKey(KeyCode.LeftShift))
                sprinting = true;

            if (!sprinting && sprintToggle && (Input.GetAxisRaw("vertical") > 0) && Input.GetKey(KeyCode.LeftShift))
                sprinting = true;

            if (sprinting)
                temp.z *= sprintMult;

            if (sprinting && sprintToggle && (Input.GetAxisRaw("vertical") <= 0))
                sprinting = false;

            if (sprinting && !sprintToggle && Input.GetKeyUp(KeyCode.LeftShift))
                sprinting = false;

            if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position,-transform.up, grounddetection))
                temp.y = jumpHeight;

            myRB.velocity = (transform.forward * temp.z) + (transform.right * temp.x) + (transform.up * temp.y);

        }
            
    }

}
