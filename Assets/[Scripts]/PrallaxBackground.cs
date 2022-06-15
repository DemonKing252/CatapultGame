using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public struct Background
    {
        public GameObject prefab;
        public Vector2Int spawnDimensions;
        public Vector2 sizeDimensions;
        public float scrollSpeed;
    }


    [SerializeField] private Background[] backgrounds;
    //[SerializeField] private Background background2;
    //[SerializeField] private Background background3;
    //[SerializeField] private Background background4;
    [Space(100)]
    public int x;
    private List<GameObject>[] backgroundLists;

    private Vector3 cameraPosLast;
    private Vector3 cameraPosCurrent;
    private Vector3 cameraPosDelta;

    // Start is called before the first frame update
    void Start()
    {
        backgroundLists = new List<GameObject>[4];
        backgroundLists[0] = new List<GameObject>();
        backgroundLists[1] = new List<GameObject>();
        backgroundLists[2] = new List<GameObject>();
        backgroundLists[3] = new List<GameObject>();

        // 3 -> - 1 = -2
        // 5 -> -2 = -3
        // 7 ->

        for (int k = 0; k < backgrounds.Length; k++)
        {
            for (int i = 0; i < backgrounds[k].spawnDimensions.x; i++)
            {
                for (int j = 0; j < backgrounds[k].spawnDimensions.y; j++)
                {
                    int startIndexX = -(Mathf.CeilToInt(backgrounds[k].spawnDimensions.x / 2) - 1) + i;
                    int startIndexY = -(Mathf.CeilToInt(backgrounds[k].spawnDimensions.y / 2) - 1) + j;

                    Vector3 spawnLoc = new Vector3((float)startIndexX * backgrounds[k].sizeDimensions.x, (float)startIndexY * backgrounds[k].sizeDimensions.y, 0f) + transform.position;
                    GameObject bg = Instantiate(backgrounds[k].prefab, spawnLoc, Quaternion.identity, this.transform);

                    backgroundLists[k].Add(bg);
                }
            }

        }
        
        //for (int i = 0; i < background2.spawnDimensions.x; i++)
        //{
        //    for (int j = 0; j < background2.spawnDimensions.y; j++)
        //    {
        //        int startIndexX = -(Mathf.CeilToInt(background2.spawnDimensions.x / 2) - 1) + i;
        //        int startIndexY = -(Mathf.CeilToInt(background2.spawnDimensions.y / 2) - 1) + j;
        //
        //        Vector3 spawnLoc = new Vector3((float)startIndexX * background2.sizeDimensions.x, (float)startIndexY * background2.sizeDimensions.y, 0f) + transform.position;
        //        GameObject bg = Instantiate(background2.prefab, spawnLoc, Quaternion.identity, this.transform);
        //
        //        background1List.Add(bg);
        //    }
        //}
        //for (int i = 0; i < background3.spawnDimensions.x; i++)
        //{
        //    int startIndexX = -(Mathf.CeilToInt(background3.spawnDimensions.x / 2) - 1) + i;
        //    int startIndexY = -(Mathf.CeilToInt(background3.spawnDimensions.y / 2) - 1) + i;
        //    //Debug.Log(startIndex);
        //
        //    Vector3 spawnLoc = new Vector3((float)startIndexX * background3.sizeDimensions.x, (float)startIndexY * background3.sizeDimensions.y, 0f) + transform.position;
        //    GameObject bg = Instantiate(background3.prefab, spawnLoc, Quaternion.identity, this.transform);
        //
        //    background3List.Add(bg);
        //}
        //for (int i = 0; i < background4.spawnDimensions.x; i++)
        //{
        //    int startIndexX = -(Mathf.CeilToInt(background4.spawnDimensions.x / 2) - 1) + i;
        //    int startIndexY = -(Mathf.CeilToInt(background4.spawnDimensions.y / 2) - 1) + i;
        //    //Debug.Log(startIndex);
        //
        //    Vector3 spawnLoc = new Vector3((float)startIndexX * background4.sizeDimensions.x, (float)startIndexY * background4.sizeDimensions.y, 0f) + transform.position;
        //    GameObject bg = Instantiate(background4.prefab, spawnLoc, Quaternion.identity, this.transform);
        //
        //    background4List.Add(bg);
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cameraPosLast = cameraPosCurrent;
        cameraPosCurrent = Camera.main.transform.position;
        cameraPosDelta = cameraPosCurrent - cameraPosLast;

        for(int j = 0; j < backgroundLists.Length; j++)
        {
            for (int i = 0; i < backgroundLists[j].Count; i++)
            {
                backgroundLists[j][i].transform.position += new Vector3(cameraPosDelta.x, 0f, 0f) * backgrounds[j].scrollSpeed;
            }
        }
        
        //for (int i = 0; i < background2List.Count; i++)
        //{
        //    background2List[i].transform.position += new Vector3(cameraPosDelta.x, 0f, 0f) * background2.scrollSpeed;
        //}
        //for (int i = 0; i < background3List.Count; i++)
        //{
        //    background3List[i].transform.position += new Vector3(cameraPosDelta.x, 0f, 0f) * background3.scrollSpeed;
        //}
        //for (int i = 0; i < background4List.Count; i++)
        //{
        //    background4List[i].transform.position += new Vector3(cameraPosDelta.x, 0f, 0f) * background4.scrollSpeed;
        //}
    }
}
