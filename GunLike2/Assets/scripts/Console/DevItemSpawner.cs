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

	float backspaceHeldFor = 0;
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

		if (Input.GetKey(KeyCode.Backspace)) { backspaceHeldFor += Time.deltaTime; }
		if (Input.GetKeyUp(KeyCode.Backspace)) { backspaceHeldFor = 0f; }

		if (typing)
		{
			consoleText.text = numberTyped;

			if ((Input.GetKeyDown(KeyCode.Backspace) || (Input.GetKey(KeyCode.Backspace) && backspaceHeldFor > 0.4f)) && numberTyped.Length > 0)
			{
				numberTyped = numberTyped.Substring(0, numberTyped.Length - 1);
			}
			else if (Input.GetKeyDown(KeyCode.Return) && typing)
			{
				TryRunCommand(numberTyped);

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
	void TryRunCommand(string command)
    {
        switch (command)
        {
			case "": Debug.Log("EmptyCommand"); break;
			case "$": GameObject.Find("Player").GetComponent<HealthManager>().money += 99999999; break;
			case "c": SpawnPotential(0); break;
			case "u": SpawnPotential(1); break;
			case "r": SpawnPotential(2); break;
			case "l": SpawnPotential(3); break;
			case "m": SpawnPotential(4); break;
			case "h": SpawnPotential(5); break;
			case "i": SpawnPotential(6); break;
			case "n": SpawnPotential(7); break;
			case "o": SpawnPotential(8); break;
			case "rand": SpawnPotential(Random.Range(0, 8)); break;
			case "gunlike": Application.OpenURL("https://scratch.mit.edu/projects/547360850/"); break;
			case "all": SpawnALL(); break;
			case "unlockall": unlockMan.UnlockAll(); break;
			case "lockall": unlockMan.LockAll(); break;
			case "openroof": gdm.roofScript.OpenRoof(); break;
			case "roofopen": gdm.roofScript.OpenRoof(); break;
			case "kill": foreach (EnemyHealthManager ehm in gdm.activeEhms) { ehm.TakeDamage(float.PositiveInfinity, true, HitType.ht.special, ehm.transform.position, "god"); } break;
			default: 
				if (command[0].ToString() == "x") { command = command.Remove(0, 1); modifier = int.Parse(command.Trim()); }
				else
				{
					try
					{
						if (modifier > 0)
						{
							for (int i = 0; i < modifier; i++)
							{
								SpawnItem(int.Parse(command.Trim()));
							}
						}
						SpawnItem(int.Parse(command.Trim()));
					}
					catch (System.Exception)
					{
						Debug.LogWarning("Invalid Command: " + command);
						typing = false;
						consoleText.text = "";

						throw;
					}

				}
				break;
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