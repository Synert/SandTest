using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandMesh : MonoBehaviour
{

    // mesh stuff
    private Vector3[] m_vVerts;
    private Vector3[] m_vUpdateVerts;
    private Vector2[] m_vUVs;
    private int[] m_iTris;
    private Mesh m_pMesh;

    // scaling
    [SerializeField] private float m_fScale = 1.0f;

    // position
    [SerializeField] private Vector3 m_vOrigin = Vector3.zero;

    // x * x size... higher res will look more detailed, might not run so well
    [SerializeField] private int m_iMeshW = 200;
    [SerializeField] private int m_iMeshH = 200;

    // max transfer rate, used to control flow
    [SerializeField] private float m_fMaxTransfer = 2.0f;

    // min difference to start flow
    [SerializeField] private float m_fMinDif = 0.5f;

    // max difference, used to work out multiplier for transfer rate
    [SerializeField] private float m_fMaxDif = 5.0f;

    // chunking for flow update, more framerate but looks worse
    [SerializeField] private int m_iChunkRate = 100;
    private int m_iCurChunk = 0;

    // Start is called before the first frame update
    void Start()
    {
        // set everything up
        m_pMesh = gameObject.AddComponent<MeshFilter>().mesh;
        m_pMesh = new Mesh();

        m_vVerts = new Vector3[m_iMeshW * m_iMeshH];
        m_vUpdateVerts = new Vector3[m_iMeshW * m_iMeshH];
        m_vUVs = new Vector2[m_iMeshW * m_iMeshH];
        m_iTris = new int[m_iMeshW * m_iMeshH * 6];

        int iNumTris = 0;

        // set up the mesh, simple starting pattern for demo
        // TODO set up a way to load in different starting heights
        for (int x = 0; x < m_iMeshW; x++)
        {
            for(int y = 0; y < m_iMeshH; y++)
            {
                float fHeight = 0.0f;

                if (x == 0 || y == 0 || x == m_iMeshW - 1 || y == m_iMeshH - 1)
                    fHeight = Random.Range(500.0f, 2000.0f);

                if (x == (m_iMeshW / 2) || y == (m_iMeshH / 2))
                    fHeight += 200.0f;

                fHeight += Random.Range(0.0f, 4.0f);

                m_vVerts[(x + y * m_iMeshW)] = new Vector3(x * m_fScale, fHeight, y * m_fScale) + m_vOrigin;

                m_vUVs[(x + y * m_iMeshW)].x = x % 2; //((float)x / (float)m_iMeshW) * m_fScale;
                m_vUVs[(x + y * m_iMeshW)].y = y % 2; //((float)y / (float)m_iMeshH) * m_fScale;

                // tris (y != h, n % w != 0)
                // n, n + w, n + 1
                // n + 1, n + w, n + w + 1
                int iTri = (x + y * m_iMeshW);

                if (y < m_iMeshH - 1 && ((iTri + 1) % m_iMeshW) != 0)
                {
                    m_iTris[iNumTris++] = iTri;
                    m_iTris[iNumTris++] = iTri + m_iMeshW;
                    m_iTris[iNumTris++] = iTri + 1;

                    m_iTris[iNumTris++] = iTri + 1;
                    m_iTris[iNumTris++] = iTri + m_iMeshW;
                    m_iTris[iNumTris++] = iTri + m_iMeshW + 1;
                }
            }
        }

        m_vUpdateVerts = m_vVerts;

        m_pMesh.Clear();
        m_pMesh.vertices = m_vVerts;
        m_pMesh.triangles = m_iTris;
        m_pMesh.uv = m_vUVs;

        m_pMesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = m_pMesh;

        //GetComponent<MeshCollider>().sharedMesh = m_pMesh;

        GetComponent<SkinnedMeshRenderer>().sharedMesh = m_pMesh;
    }

    // Update is called once per frame
    void Update()
    {
        // go over the vertices, check differences, process for new resulting verts
        // checks the mesh and updates the changes to local array, allowing it all to be done at once

        // check it in chunks to run smoother
        m_pMesh.vertices = m_vVerts;

        if (m_iCurChunk == 0)
        {
            // update mesh
            //m_pMesh.vertices = m_vVerts;
            m_vUpdateVerts = m_vVerts;

            m_pMesh.RecalculateNormals();
            //m_pMesh.RecalculateTangents();
        }

        for (int x = m_iCurChunk; x < m_iMeshW && x < m_iCurChunk + m_iChunkRate; x++)
        {
            for (int y = 0; y < m_iMeshH; y++)
            {
                // make sure this square isn't an object
                // TODO expand on this
                bool bObject = (x == (m_iMeshW / 2) && y == (m_iMeshH / 2));

                if (bObject)
                    continue;

                Vector3 vVert = m_vUpdateVerts[x + y * m_iMeshW];

                // first, find out how much we're transferring
                float fTotalTransfer = 0.0f;
                float[] fTransfer = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
                int iTransferTotal = 0;

                // up to 9 neighbours, depending on where this point is
                for (int i = 0; i < 9; i++)
                {
                    int iAddX = (i % 3) - 1;
                    int iAddY = (int)Mathf.Floor(i / 3.0f) - 1;
                    int iVert = (x + iAddX) + (y + iAddY) * m_iMeshW;

                    bool bInbounds = (iVert >= 0 && iVert < m_iMeshW * m_iMeshH)
                        && (x + iAddX >= 0 && x + iAddX < m_iMeshW)
                        && (y + iAddY >= 0 && y + iAddY < m_iMeshH);

                    // make sure the neighbour isn't an object
                    // TODO expand on this
                    bInbounds &= !((x + iAddX) == (m_iMeshW / 2) && (y + iAddY) == (m_iMeshH / 2));

                    if (bInbounds)
                    {
                        Vector3 vCompare = m_vUpdateVerts[iVert];
                        float fDif = vVert.y - vCompare.y;

                        if (fDif > m_fMinDif)
                        {
                            // add to the transfer as a scalar value, 0.0 - 1.0
                            fDif -= m_fMinDif;
                            fDif /= m_fMaxDif;

                            fTransfer[i] = Mathf.Min(fDif, m_fMaxDif);
                            fTotalTransfer += fTransfer[i];
                            iTransferTotal++;
                        }
                    }
                }

                // now we transfer
                if (iTransferTotal > 0)
                {
                    m_vVerts[x + y * m_iMeshW].y -= ((fTotalTransfer * m_fMaxTransfer) / iTransferTotal) * 1.0f;// Time.deltaTime;

                    for (int i = 0; i < 9; i++)
                    {
                        if (fTransfer[i] > 0.0f)
                        {
                            // at this point we can safely assume it was already in bounds
                            int iAddX = (i % 3) - 1;
                            int iAddY = (int)Mathf.Floor(i / 3.0f) - 1;
                            int iVert = (x + iAddX) + (y + iAddY) * m_iMeshW;

                            m_vVerts[iVert].y += ((fTransfer[i] * m_fMaxTransfer) / iTransferTotal) * 1.0f;// Time.deltaTime;
                        }
                    }
                }
            }
        }

        m_iCurChunk += m_iChunkRate;

        if (m_iCurChunk >= m_iMeshW)
            m_iCurChunk = 0;
    }
}
