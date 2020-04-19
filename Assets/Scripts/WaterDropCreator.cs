using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropCreator : MonoBehaviour
{

    public GameObject waterDropPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnWaterDrop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawnWaterDrop()
    {
        yield return new WaitForSeconds(1.0f);
        WaterDrop waterDrop = Instantiate(waterDropPrefab, transform.position, Quaternion.identity).GetComponent<WaterDrop>();
        StartCoroutine(spawnWaterDrop());
    }
}
