using UnityEngine;
using System.Collections;

public class BallSpawner : MonoBehaviour
{
    public GameObject myBall;
    
    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(waiter());
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-9, 9), 1, Random.Range(-9, 9));
        Instantiate(myBall, randomSpawnPosition, Quaternion.identity);
    }
    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(1);
    }
}
