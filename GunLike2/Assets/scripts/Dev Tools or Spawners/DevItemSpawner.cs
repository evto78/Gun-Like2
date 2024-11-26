using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevItemSpawner : MonoBehaviour
{
	public GameObject item;
	public GameObject itemPotential;

	private Camera cam;

	public TextMeshProUGUI consoleText;

	bool typing;
	string numberTyped;

	private void Start()
	{
		typing = false;
		cam = Camera.main;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.End))
		{
			SpawnItem(Random.Range(0, item.GetComponent<Item>().itemList.Count - 1));
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
				if(numberTyped == "c") { SpawnPotential(0); }
				if(numberTyped == "u") { SpawnPotential(1); }
				if(numberTyped == "r") { SpawnPotential(2); }
				if(numberTyped == "l") { SpawnPotential(3); }
				if(numberTyped == "m") { SpawnPotential(4); }
				if(numberTyped == "h") { SpawnPotential(5); }
				if(numberTyped == "i") { SpawnPotential(6); }
				if(numberTyped == "n") { SpawnPotential(7); }
				if(numberTyped == "o") { SpawnPotential(8); }
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
		spawnedItem.transform.position = transform.position;
		spawnedItem.transform.rotation = transform.rotation;
		spawnedItem.transform.Translate(0, 2, 4, Space.Self);
		spawnedItem.GetComponent<Item>().SetItemID(iD);
	}
	void SpawnPotential(int iD)
    {
		GameObject spawnedItem;
		spawnedItem = Instantiate(itemPotential);
		spawnedItem.transform.position = transform.position;
		spawnedItem.transform.rotation = transform.rotation;
		spawnedItem.transform.Translate(0, 2, 4, Space.Self);
		spawnedItem.GetComponent<ItemPossibility>().SetRarity(iD);
	}
}