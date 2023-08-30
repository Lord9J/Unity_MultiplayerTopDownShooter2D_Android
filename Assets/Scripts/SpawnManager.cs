using UnityEngine;
using Photon.Pun;
public class SpawnManager : MonoBehaviourPunCallbacks
{

    [Header("Площадь спавнера")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Спавнер игроков")]
    public GameObject playerPrefab;


    [Header("Спавнер монет")]
    public GameObject coinPrefab;
    public LayerMask obstacleLayer; // Слои объектов, которые нужно учитывать при проверке
    public int maxCoinCount = 50; // Максимальное количество монет
    public float coinSpawnInterval = 5f; // Интервал создания монеты
    public float coinSpawnMargin = 1f; // Отступ от краев
    private int currentCoinCount = 0;




    [PunRPC]   // Метод вызываемый у всех, когда игра начниается
    public void StartGameRPC()
    {
        //Начинаем создавать монеты через определенные интервалы
        InvokeRepeating("SpawnCoin", 0f, coinSpawnInterval);

        // Создание персонажа игры
        SpawnPlayer();
    }

    public void SpawnPlayer() // Создание персонажей игрока
    {
        Vector2 spawnPosition = GetRandomSpawnPosition(); // получить случайные координаты

        GameObject playerObject = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);

        PlayerData playerData = playerObject.GetComponent<PlayerData>(); // Инициалзиция данных персонажа
        if (playerData != null)
        {
            playerData.InitializePlayerData();
            GameManager.instance.playerDataDictionary.Add(playerObject.GetPhotonView().ViewID, playerData); // Здесь сохраняем playerData в словарь по ViewID
        }
    }

    private void SpawnCoin()  // Создание префаба монет
    {
        if (PhotonNetwork.IsMasterClient) // если это мастер
        {
            // Проверяем, не превышено ли максимальное количество монет
            if (currentCoinCount < maxCoinCount)
            {
                Vector2 spawnPosition = GetRandomSpawnPosition();

                // Создаем монету в найденной позиции
                PhotonNetwork.Instantiate(coinPrefab.name, spawnPosition, Quaternion.identity);

                // Увеличиваем счетчик монет
                currentCoinCount++;
            }
        }
    }


    private Vector2 GetRandomSpawnPosition() // получение случайно координаты
    {
        Vector3 randomPosition;
        int attempts = 0;
        int maxAttempts = 20;

        do
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            randomPosition = new Vector3(randomX, randomY, 0f);

            attempts++;
        } while (attempts < maxAttempts && Physics2D.OverlapCircle(randomPosition, 0.1f, obstacleLayer));

        if (attempts >= maxAttempts)
        {
            return Vector2.zero;
        }

        return randomPosition;
    }

}
