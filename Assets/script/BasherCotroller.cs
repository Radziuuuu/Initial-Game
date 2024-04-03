using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherController : MonoBehaviour
{
    //gracz
    GameObject player;
    //pr�dko�� pod��ania za graczem
    public float walkSpeed = 2f;
    //levelManager odwo�anie
    GameObject levelManager;

    bool hasBeenHit = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //patrz si� na gracza
        transform.LookAt(player.transform.position);
        //idz do przodu
        transform.position += transform.forward * Time.deltaTime * walkSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (hasBeenHit) { return; }
        //Debug.Log("Trafiony");
        //obiekt z kt�rym mamy kolizje
        GameObject projectile = collision.gameObject;

        //tylko je�li trafi� nas gracz
        if (projectile.CompareTag("PlayerProjectile"))
        {
            
            hasBeenHit = true;
            
            //dolicz punkty
            levelManager.GetComponent<LevelManager>().AddPoints(1);

            //zniknij pocisk
            Destroy(projectile);

            //zniknij przeciwnika
            Destroy(transform.gameObject);
        }
        /* if (collision.gameObject.CompareTag("Player"))
         {
             //weszlismy w gracza - poinformuj go o tym
             collision.gameObject.GetComponent<PlayerController>().Hit(gameObject);
         }*/
    }
}