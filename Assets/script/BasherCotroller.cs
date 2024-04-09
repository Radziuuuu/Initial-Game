using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherCotroller : MonoBehaviour
{

    GameObject player;

    bool HasBeenHit = false;

    private GameObject LevelManager;

    public float walkSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        LevelManager = GameObject.Find("LevelManager");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform.position);
        //idz do przodu
        transform.position += transform.forward * Time.deltaTime * walkSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (HasBeenHit) { return; }

        /*Debug.Log("Trafiony!");*/
        //obiekt z którym mamy kolizjê
        GameObject projectile = collision.gameObject;

        //tylko jeœli trafi³ nas gracz
        if (projectile.CompareTag("PlayerProjectile"))
        {
            HasBeenHit = true;

            LevelManager.GetComponent<LevelManager>().addPoints(1);

            //zniknij pocisk
            Destroy(projectile);

            //zniknij przeciwnika
            Destroy(transform.gameObject);
        }
    }
}
