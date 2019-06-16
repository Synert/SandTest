using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandParticle : MonoBehaviour
{

    private float m_fStartSize = 0.1f;
    private float m_fCurSize;
    private float m_fEndSize = 0.8f;

    private float m_fScaleRate = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        m_fCurSize = m_fStartSize;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = m_fCurSize * Vector3.one;

        m_fCurSize += m_fScaleRate;

        m_fCurSize = Mathf.Min(m_fCurSize, m_fEndSize);
    }
}
