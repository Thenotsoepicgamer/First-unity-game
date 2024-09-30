using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class BasicEnemyController : MonoBehaviour
{
    public PlayerController player;
    public NavMeshAgent agent;
    public Transform target;


    [Header("Enemy Stats")]
    public int health = 3;
    public int maxHealth = 5;
    public int damageGiven = 1;
    public int damageReceived = 1;
    public float pushBackForce = 5;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
       

        if (health <= 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            health -= damageReceived;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Player" && !player.takenDamage)
        {
            player.takenDamage = true;
            player.health -= damageGiven;
            player.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * pushBackForce);
            player.StartCoroutine("cooldownDamage");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            agent.destination = player.transform.position;

        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            agent.destination = player.transform.position;

        }


    }
}

// AAAAAAAAAAAAAA
