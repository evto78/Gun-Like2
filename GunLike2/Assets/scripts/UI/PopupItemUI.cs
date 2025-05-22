using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupItemUI : MonoBehaviour
{
    public GameObject popupNotif;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNotif(int id, int change)
    {
        ItemObject fetchedObject;
        GameObject spawnedNotif = Instantiate(popupNotif);
        spawnedNotif.transform.SetParent(transform);
        spawnedNotif.transform.position = Vector3.zero;
        fetchedObject = Resources.Load<ItemObject>("Items/" + id.ToString());

        spawnedNotif.transform.GetChild(1).GetComponent<Image>().sprite = fetchedObject.itemSprite;
        if(change > 0) {spawnedNotif.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "+"+change.ToString();}
        else{spawnedNotif.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = change.ToString();}
        spawnedNotif.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = fetchedObject.itemName;

        Destroy(spawnedNotif, 2f);
    }
}
