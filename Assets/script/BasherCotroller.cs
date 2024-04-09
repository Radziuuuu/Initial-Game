using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherCotroller : MonoBehaviour
{
    //gracz
    GameObject player;
    //flaga. która mówi czy zosta³ ju¿ trafiony przeciwnik (basher) i zosta³ za niego policzony
    bool HasBeenHit = false;

    private GameObject LevelManager;
    //prêdkoœæ pod¹¿ania za graczem
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
        //jeœli zosta³ ju¿ trafiony to nic nie rób
        if (HasBeenHit) { return; }

        /*Debug.Log("Trafiony!");*/
        //obiekt z którym mamy kolizjê
        GameObject projectile = collision.gameObject;

        //tylko jeœli trafi³ nas gracz
        if (projectile.CompareTag("PlayerProjectile"))
        {
            //ustaw flage
            HasBeenHit = true;

            //dolicz punkty
            LevelManager.GetComponent<LevelManager>().addPoints(1);

            //zniknij pocisk
            Destroy(projectile);

            //zniknij przeciwnika
            Destroy(transform.gameObject);
        }
    }
}
