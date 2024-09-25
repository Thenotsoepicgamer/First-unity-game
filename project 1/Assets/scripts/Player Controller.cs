using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Vector2 camRotation;

    Rigidbody myRB;
    Camera playerCam;

    Transform cameraHolder;


    [Header("Player stats")]
    public int health = 5;
    public int maxHealth = 10;
    public int healthPickupAmt = 5;
    public bool takenDamage = false;

    [Header("weapon Stats")]
    public Transform weaponSlot;
    public GameObject shot;
    public bool CanFire = true;
    public float fireRate = 0;
    public int weaponID = -1;
    public float shotVel = 0;
    public int firemode = 0;
    public float currentMag = 0;
    public float magSize = 0;
    public float maxAmmo = 0;
    public float currentAmmo = 0;
    public float reloadAmt = 0;
    public float bulletLifespan = 0;
  

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
        playerCam = Camera.main;


        camRotation = Vector2.zero;
  
        Cursor.visible = false;

        cameraHolder = transform.GetChild(0);

    }



    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        {

         
           

            camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

            camRotation.y = Mathf.Clamp(camRotation.y, -90, 90);

            playerCam.transform.position = cameraHolder.position;

            playerCam.transform.rotation = Quaternion.Euler(-camRotation.y, camRotation.x, 0);
            transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);

            if (Input.GetMouseButton(0) && CanFire && currentMag > 0 && weaponID >=0)
            {
                GameObject s = Instantiate(shot, weaponSlot.position, weaponSlot.rotation);
                s.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * shotVel);
                Destroy(s, bulletLifespan);
                CanFire = false;
                currentMag--;
                StartCoroutine("cooldownFire");

            }
            if (Input.GetKeyDown(KeyCode.R))
                reloadMag();

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


    {
        
    }
}
            
    }
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "health pick up") && health < maxHealth)
        {
            if (health + healthPickupAmt > maxHealth)
                health = maxHealth;

            else health += healthPickupAmt;

            Destroy(collision.gameObject);
    
      
        }

        if ((collision.gameObject.tag == "ammoPickup") && currentAmmo < maxAmmo)
        {
            if (currentAmmo + reloadAmt > maxAmmo)
                currentAmmo = maxAmmo;

            else currentAmmo += reloadAmt;

            Destroy(collision.gameObject);

        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Gun")
        {

            other.transform.position = weaponSlot.position;
            other.transform.rotation = weaponSlot.rotation;

            other.transform.SetParent(weaponSlot);

            switch(other.gameObject.name)
            {
                case "weapon1":
                    weaponID = 0;
                    shotVel = 10000;
                    firemode = 0;
                    currentMag = 20;
                    magSize = 400;
                    currentAmmo = 200;
                    reloadAmt = 20;
                    bulletLifespan = .5f;
                    break;

                default:
                    break;




            }
        }

    }
    public void reloadMag()
    {
        if (currentMag >= magSize)
            return;
        else
        {
            float reloadCount = magSize - currentMag;
            if(currentAmmo < reloadCount)
            {
                currentMag += currentAmmo;
                currentAmmo = 0;
                return;


            }

            else
            {
                currentMag += reloadCount;
                currentAmmo -= reloadCount;
                return;




            }



        }

        







    }

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
        CanFire = true;

    }



    IEnumerator cooldownDamage()
    {
        yield return new WaitForSeconds(fireRate);
        CanFire = true;

    }




}


