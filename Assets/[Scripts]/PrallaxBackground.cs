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
        public Vector3 offset;
    }


    [SerializeField] private Background[] backgrounds;
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

                    Vector3 spawnLoc = new Vector3((float)startIndexX * backgrounds[k].sizeDimensions.x, (float)startIndexY * backgrounds[k].sizeDimensions.y, 0f) + transform.position + backgrounds[k].offset;
                    GameObject bg = Instantiate(backgrounds[k].prefab, spawnLoc, Quaternion.identity, this.transform);

                    backgroundLists[k].Add(bg);
                }
            }

        }
        
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
        
    }
}
