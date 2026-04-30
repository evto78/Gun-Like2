using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevItemSpawner : MonoBehaviour
{
	GameDataManager gdm; UnlockManager unlockMan;
	public GameObject item;
	public GameObject itemPotential;
	List<ItemObject> itemData;

	private Camera cam;

	public TextMeshProUGUI consoleText;

	bool typing;
	string numberTyped;
	int modifier;
    private void Awake()
    {
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
		unlockMan = gdm.gameObject.GetComponent<UnlockManager>();
    }
    private void Start()
	{
		modifier = 0;
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

			if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKey(KeyCode.Backspace)) && numberTyped.Length > 0)
			{
				numberTyped = numberTyped.Substring(0, numberTyped.Length - 1);
			}
			else if (Input.GetKeyDown(KeyCode.Return) && typing)
			{	
				if (numberTyped == "") { Debug.Log("EmptyCommand"); }
				else if(numberTyped == "$") { GameObject.Find("Player").GetComponent<HealthManager>().money += 99999999; }
				else if(numberTyped == "c") { SpawnPotential(0); }
				else if (numberTyped == "u") { SpawnPotential(1); }
				else if (numberTyped == "r") { SpawnPotential(2); }
				else if (numberTyped == "l") { SpawnPotential(3); }
				else if (numberTyped == "m") { SpawnPotential(4); }
				else if (numberTyped == "h") { SpawnPotential(5); }
				else if (numberTyped == "i") { SpawnPotential(6); }
				else if (numberTyped == "n") { SpawnPotential(7); }
				else if (numberTyped == "o") { SpawnPotential(8); }
				else if (numberTyped == "rand") { SpawnPotential(Random.Range(0,8)); }
				else if (numberTyped == "gunlike") { Application.OpenURL("https://scratch.mit.edu/projects/547360850/"); }
				else if (numberTyped == "all") { SpawnALL(); }
				else if (numberTyped == "unlockall") { unlockMan.UnlockAll(); }
				else if (numberTyped == "lockall") { unlockMan.LockAll(); }
				else if (numberTyped == "openroof") { gdm.roofScript.OpenRoof(); }
				else if (numberTyped == "kill") { foreach (EnemyHealthManager ehm in gdm.activeEhms) { ehm.TakeDamage(float.PositiveInfinity, true, HitType.ht.special, ehm.transform.position, "god"); } }
				else if (numberTyped[0].ToString() == "x") { numberTyped = numberTyped.Remove(0, 1); modifier = int.Parse(numberTyped.Trim()); }
				else
				{
					try
					{
                        if (modifier > 0)
                        {
                            for (int i = 0; i < modifier; i++)
                            {
                                SpawnItem(int.Parse(numberTyped.Trim()));
                            }
                        }
                        SpawnItem(int.Parse(numberTyped.Trim()));
                    }
					catch (System.Exception)
					{
						Debug.LogWarning("Invalid Command: " + numberTyped);
						typing = false;
						consoleText.text = "";

						throw;
					}
					
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
	void SpawnALL()
    {
		foreach(ItemObject itemObj in itemData)
        {
			SpawnItem(itemObj.id);
        }
    }
	private void SpawnItem(int iD)
	{
		GameObject spawnedItem;
		spawnedItem = Instantiate(item, transform.position + transform.forward * 3f + Vector3.up * 3f, transform.rotation);
		spawnedItem.GetComponent<Item>().SetItemID(iD);
	}
	void SpawnPotential(int iD)
    {
		GameObject spawnedItem;
		spawnedItem = Instantiate(itemPotential, transform.position + transform.forward * 3f + Vector3.up * 3f, transform.rotation);
		spawnedItem.GetComponent<ItemPossibility>().SetRarity(iD, false);
	}
}