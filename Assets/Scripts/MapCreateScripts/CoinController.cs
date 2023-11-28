using UnityEngine;

public class CoinController : MonoBehaviour
{
    public AudioClip collectSound;
    public GameObject collectEffect;
    public PlayerControl coinCount;
    

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 10.0f * Time.deltaTime, Space.World);
    }

    // 플레이어가 코인을 먹을때
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("asdf");
        if (other.CompareTag("Player"))
        {
            Collect();
            Debug.Log("Player");
        }
    }

    void Collect()
    {
        Debug.Log("Coll");
        coinCount.coinCount--;
        Destroy(gameObject);
        // if(collectSound)
        // 	AudioSource.PlayClipAtPoint(collectSound, transform.position);
        // if(collectEffect)
        // 	Instantiate(collectEffect, transform.position, Quaternion.identity);
    }
}