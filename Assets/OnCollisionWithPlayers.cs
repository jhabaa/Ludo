using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionWithPlayers : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "yellow" | collision.collider.tag == "red")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), collision.collider);
        }
    }
} 
