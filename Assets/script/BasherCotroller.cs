using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherCotroller : MonoBehaviour
{

    GameObject player;

    public float walkSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform.position);
        //idz do przodu
        transform.position += transform.forward * Time.deltaTime * walkSpeed;
    }
}
