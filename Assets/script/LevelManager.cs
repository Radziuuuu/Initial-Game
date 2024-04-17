using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    //ilość punktów
    int points;

    //licznik punktów na ekranie
    public GameObject pointsCounter;

    //licznik czasu na ekranie
    public GameObject timeCounter;

    //czas do końća poziomu
    public float levelTime = 60f;

    //ekran końca gry
    public GameObject gameOverScreen;


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
        
        if (levelTime < 0)
        {
            GameOver();
        }
        else
        {
            levelTime -= Time.deltaTime;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        pointsCounter.GetComponent<TextMeshProUGUI>().text = $"Points: " + points.ToString();
        timeCounter.GetComponent<TextMeshProUGUI>().text = Mathf.Floor(levelTime).ToString()=="-1"?"0": Mathf.Floor(levelTime).ToString();
        //time -1 to 0
    }

    public void addPoints(int amount)
    {
        points += amount;
    }

    //ta funkcja uruchamia sie jesli gracz zginie lub jesli czas sie skonczy

    public void GameOver()
    {
        //wyłącz sterowanie gracza
        player.GetComponent<PlayerControler>().enabled = false;
        player.transform.Find("MainTurret").GetComponent<WeaponController>().enabled = false;

        //wyłącz bashery
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject basher in enemyList)
        {
            basher.GetComponent<BasherCotroller>().enabled = false;
        }

        //wyświetl poprawny wynik na ekranie końcowym
        gameOverScreen.transform.Find("FinalScoreText").GetComponent<TextMeshProUGUI>().text = "Wynik końcowy: " + points.ToString();
        


        //pokaż ekran końca gry
        gameOverScreen.SetActive(true);
    }
}