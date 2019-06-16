using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int m_iSandCount;
    [SerializeField] private Transform m_pSand;
    void Start()
    {
        // spawn the thingos
        for (int i = 0; i < m_iSandCount; i++)
        {
            Vector3 vPos;
            vPos.x = Random.Range(-3.0f, 3.0f);
            vPos.y = Random.Range(1.0f, 3.0f);
            vPos.z = Random.Range(-3.0f, 3.0f);
            Instantiate(m_pSand, vPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
