using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //pozycja gracza
    Transform player;

    //prefab przeciwnika
    public GameObject basherPrefab;

    //czas między respawnem kolejnego bashera
    public float spawnInterval = 1;

    //czas od ostatniego respawnu
    float timeSinceSpawn;

    //bezpieczna odległość spawnu
    float spawnDistance = 30;

    //ilosc pkt
    int points = 0;

    public GameObject pointsCounter;
    // Start is called before the first frame update
    void Start()
    {
        //zlinkuj aktualna pozycje gracza do zmiennej transform
        player = GameObject.FindWithTag("Player").transform;

        //zerujemy licznik
        timeSinceSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //dodaj do czasu od ostatniego spawnu czas od ostatniej klatki (ostatni update())
        timeSinceSpawn += Time.deltaTime;

        //jeżeli dłużej niż jedna sekunda
        if (timeSinceSpawn > spawnInterval)
        {
            //wygeneruj losową pozycje
            //Vector3 randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));

            //wygeneruj randomową pozycję na kole o promieniu 1
            Vector2 random = Random.insideUnitCircle.normalized;

            //skonwertuj x,y na x,z i zerową wysokość
            Vector3 randomPosition = new Vector3(random.x, 0, random.y);

            //zwielokrotnij odległosć od gracza tak, żeby spawn następował poza kamerą
            randomPosition *= spawnDistance;

            //dodaj do niej pozycje gracza tak, aby nowe współrzędne były pozycją względem gracza
            randomPosition += player.position;

            //sprawdz czy danej miejsce jest wolne
            if (!Physics.CheckSphere(new Vector3(randomPosition.x, 1, randomPosition.z), 0.5f))
            {
                //stworz nowego przeciwnika z istniejącego prefaba, na pozycji randomPosition z rotacją domyślną
                Instantiate(basherPrefab, randomPosition, Quaternion.identity);

                //wyzeruj licznik
                timeSinceSpawn = 0;
            }
            //jeśli miejsce będzie zajęte to program podejmie kolejną próbę w następnej klatce

        }

        //TODO: opracować sposób na przyspieszanie spawnu w nieskończoność wraz z długościa trwania etapu

        UpdateUI();
    }
    public void AddPoints(int amount)
    {
        points += amount;
    }
    //funkcja która odpowiada za aktualizacje interfejsu
    private void UpdateUI()
    {
        pointsCounter.GetComponent<TextMeshProUGUI>().text = "Punkty: " + points.ToString();
    }
}