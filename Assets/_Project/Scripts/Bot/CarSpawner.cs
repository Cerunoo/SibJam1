using UnityEngine;
using System.Collections;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private PlayerController player;
    [SerializeField] private float[] posSpawnX;
    [SerializeField] private Vector2 spawnDelay;

    private void Start()
    {
        StartCoroutine(SpawnCarIterator());
    }

    private IEnumerator SpawnCarIterator()
    {
        while (true)
        {
            SpawnCar();
            yield return new WaitForSeconds(Random.Range(spawnDelay.x, spawnDelay.y));
        }
    }

    private void SpawnCar()
    {
        int row = Random.Range(0, 3);
        Vector2 pos = player.transform.position;
        pos.y += 15;
        pos.x = posSpawnX[row];

        Instantiate(car, pos, Quaternion.identity);
    }
}