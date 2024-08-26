using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevItemSpawner : MonoBehaviour
{
	public GameObject item;
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
				Debug.Log("ATTEMPING TO SPAWN: " + numberTyped);
				Debug.Log("ATTEMPING TO SPAWN: " + System.Int32.Parse(numberTyped.Trim()));
				
				SpawnItem(System.Int32.Parse(numberTyped.Trim()));

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
		spawnedItem = Instantiate(item, new Vector3(transform.localPosition.x, transform.localPosition.y + 2f, transform.localPosition.z + 5f), transform.rotation);
		spawnedItem.GetComponent<Item>().SetItemID(iD);
	}
}