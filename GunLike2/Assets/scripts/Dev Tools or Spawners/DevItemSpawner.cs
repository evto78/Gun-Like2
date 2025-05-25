using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevItemSpawner : MonoBehaviour
{
	public GameObject item;
	public GameObject itemPotential;
	List<ItemObject> itemData;

	private Camera cam;

	public TextMeshProUGUI consoleText;

	bool typing;
	string numberTyped;

	private void Start()
	{
		itemData = new List<ItemObject>();
		itemData.AddRange(Resources.LoadAll<ItemObject>("Items"));
		typing = false;
		cam = Camera.main;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.End))
		{
			SpawnItem(Random.Range(0, itemData.Count - 1));
		}

		if (Input.GetKeyDown(KeyCode.BackQuote) && !typing)
		{
			typing = true;
			numberTyped = "";
		}

		if (typing)
		{
			consoleText.text = numberTyped;

			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				numberTyped = numberTyped.Substring(0, numberTyped.Length - 1);
			}
			else if (Input.GetKeyDown(KeyCode.Return) && typing)
			{	
				if(numberTyped == "$") { GameObject.Find("Player").GetComponent<HealthManager>().money += 99999999; }
				if(numberTyped == "c") { SpawnPotential(0); }
				if(numberTyped == "u") { SpawnPotential(1); }
				if(numberTyped == "r") { SpawnPotential(2); }
				if(numberTyped == "l") { SpawnPotential(3); }
				if(numberTyped == "m") { SpawnPotential(4); }
				if(numberTyped == "h") { SpawnPotential(5); }
				if(numberTyped == "i") { SpawnPotential(6); }
				if(numberTyped == "n") { SpawnPotential(7); }
				if(numberTyped == "o") { SpawnPotential(8); }
				if(numberTyped == "rand") { SpawnPotential(Random.Range(0,8)); }
                else
                {
					SpawnItem(System.Int32.Parse(numberTyped.Trim()));
				}

				typing = false;
			}
			else
			{
				if(Input.inputString != "`") { numberTyped = numberTyped + Input.inputString; }
			}
		}
        else
        {
			consoleText.text = "";
        }
	}

	private void SpawnItem(int iD)
	{
		GameObject spawnedItem;
		spawnedItem = Instantiate(item);
		spawnedItem.transform.position = transform.position + transform.forward * 3f + Vector3.up * 3f;
		spawnedItem.transform.rotation = transform.rotation;
		spawnedItem.GetComponent<Item>().SetItemID(iD);
	}
	void SpawnPotential(int iD)
    {
		GameObject spawnedItem;
		spawnedItem = Instantiate(itemPotential);
		spawnedItem.transform.position = transform.position + transform.forward * 3f + Vector3.up * 3f;
		spawnedItem.transform.rotation = transform.rotation;
		spawnedItem.GetComponent<ItemPossibility>().SetRarity(iD);
	}
}