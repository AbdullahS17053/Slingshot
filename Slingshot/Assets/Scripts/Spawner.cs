using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{ 
    public GameObject item = null;
    public Transform spawnPoint;
    public GameObject currItem = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (item)
        {
            currItem = Instantiate(item, spawnPoint);
            item = null;
        }
    }

    public void destroyItem()
    {
        if (currItem != null)
        {
            Destroy(currItem);
        }
    }
}
