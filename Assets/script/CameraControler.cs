using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    //pozycja gracza
    Transform player;
    //offset kamery
    Vector3 offset;
    //cos tam co działa
    Vector3 cameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //zapisujemy aktualny offset kamery
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Policz nowa pozycje kamery
        Vector3 targetPosition = player.position + offset;
        //przsuń kamere w kierunku celu
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraSpeed, 0.3f);
    }
}
