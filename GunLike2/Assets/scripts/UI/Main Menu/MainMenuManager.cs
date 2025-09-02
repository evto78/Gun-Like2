using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public List<GunObjectData> gunObjectData;
    public RemoteDoor bgDoor;
    Camera cam;
    bool starting;
    public List<GameObject> uiTOHIDE; bool loading;
    public SaveFileReadWrite instance;
    WeaponSelection weaponSelect;
    public Transform continueSlots;
    public SettingsScript settings;
    public UiSoundPlayer usp;
    public int selectedDifficulty;
    public EventSystem eventS;
    [System.Serializable]
    public class DiffDetail
    {
        public string difName;
        public Sprite bg;
        public GameObject detailsHolder;
    }
    public List<DiffDetail> difficultyInfo;
    int curDifDisplay = 1;
    [System.Serializable]
    public class MutatedRule
    {
        public string rule;
        public int id;
        public Color color;
        public int rarity; // (0 - 50%) (1 - 30%) (2 - 15%) (3 - 5%) <-- Odds
    }
    public List<MutatedRule> mutatedRulesPossibilities;
    List<MutatedRule> rarity0Rules = new List<MutatedRule>();
    List<MutatedRule> rarity1Rules = new List<MutatedRule>();
    List<MutatedRule> rarity2Rules = new List<MutatedRule>();
    List<MutatedRule> rarity3Rules = new List<MutatedRule>();
    public List<int> currentMutatedRules = new List<int>();
    public Transform mutatedRulesHolder; public TextMeshProUGUI mutationIDText; public string mutationID;
    Animator camAnim;
    public GameObject attachedUI;
    private void Awake()
    {
        gunObjectData = new List<GunObjectData>(); gunObjectData.AddRange(Resources.LoadAll<GunObjectData>("Guns"));
        SortGunObjData();
        attachedUI.SetActive(true);
        weaponSelect = GameObject.Find("BEHINDDOORUI").GetComponent<WeaponSelection>();
        usp = GetComponent<UiSoundPlayer>();
        Time.timeScale = 1f;
        if (PlayerPrefs.HasKey("SELECTEDDIFFICULTY")) { selectedDifficulty = PlayerPrefs.GetInt("SELECTEDDIFFICULTY"); } else { selectedDifficulty = 1; PlayerPrefs.SetInt("SELECTEDDIFFICULTY", 1); }

        settings.tab.SetActive(true);
        settings.InitialApply();
    }
    void SortGunObjData()
    {
        List<int> comparisonList = new List<int>();
        List<GunObjectData> sortedGunData = new List<GunObjectData>();
        for (int i = 0; i < gunObjectData.Count; i++) { comparisonList.Add(i-1); sortedGunData.Add(null); }
        for (int i = 0; i < gunObjectData.Count; i++)
        {
            sortedGunData[comparisonList.IndexOf(gunObjectData[i].id)] = gunObjectData[i];
        }
        gunObjectData = sortedGunData;
    }
    void Start()
    {
        Time.timeScale = 1f;
        cam = Camera.main;
        camAnim = cam.GetComponent<Animator>();

        foreach(MutatedRule mr in mutatedRulesPossibilities)
        {
            switch (mr.rarity)
            {
                case 0: rarity0Rules.Add(mr); break;
                case 1: rarity1Rules.Add(mr); break;
                case 2: rarity2Rules.Add(mr); break;
                case 3: rarity3Rules.Add(mr); break;
            }
        }
        mutationIDText.text = "Current Mutation: NULL";

        SaveDataButtonSetUp();
    }
    public void SaveDataButtonSetUp()
    {
        continueSlots.parent.parent.gameObject.SetActive(true);
        for(int i = 0; i < continueSlots.childCount; i++)
        {
            if (i >= instance.savedRuns.Count) { continueSlots.GetChild(i).gameObject.GetComponent<SaveSlotButton>().SaveSlotSetUp(null); }
            else { continueSlots.GetChild(i).gameObject.GetComponent<SaveSlotButton>().SaveSlotSetUp(instance.savedRuns[i]); }
        }
        continueSlots.parent.parent.gameObject.SetActive(false);
    }
    private void Update()
    {
        if ((Input.GetKeyDown(instance.controlsBinds.pauseMenu) || Input.GetKeyDown(KeyCode.Escape)) && starting) { Back(); }
    }
    public void Play(int what)
    {
        if (starting) { return; }
        if (what == -1)
        {
            eventS.SetSelectedGameObject(null); //Deselect buttons

            instance.loadingARun = -1;
            bgDoor.Activate();
            camAnim.SetBool("Leaving", true);
            starting = true;
            foreach (GameObject go in uiTOHIDE) { if (go != null) { go.SetActive(false); } }
            usp.UIDifficultySound(selectedDifficulty);
            PlayerPrefs.SetInt("SELECTEDDIFFICULTY", selectedDifficulty);
            if (selectedDifficulty == 4)
            {//get mutated rules id
                PlayerPrefs.SetInt("MUTATEDRULE1", currentMutatedRules[0]);
                PlayerPrefs.SetInt("MUTATEDRULE2", currentMutatedRules[1]);
                PlayerPrefs.SetInt("MUTATEDRULE3", currentMutatedRules[2]);
                PlayerPrefs.SetInt("MUTATEDRULE4", currentMutatedRules[3]);
                PlayerPrefs.SetInt("MUTATEDRULE5", currentMutatedRules[4]);
                PlayerPrefs.SetInt("MUTATEDRULE6", currentMutatedRules[5]);
            }
        }
        else
        {
            instance.loadingARun = what;
            LoadLevel(true);
        }
    }
    public void Back()
    {
        camAnim.Play("donemoving");
        camAnim.SetBool("Leaving", false);
        camAnim.SetTrigger("Back");
        starting = false;
        StartCoroutine(BackDoorActivate());
        foreach (GameObject go in uiTOHIDE)
        {
            if (go != null)
            {
                go.SetActive(true);
            }
        }
    }
    IEnumerator BackDoorActivate()
    {
        while(cam.transform.position.z < -9f)
        {
            yield return null;
        }
        bgDoor.Activate();
    }
    void UpdateGunInfo()
    {
        int tempLeft = PlayerPrefs.GetInt("leftHandGunSelect"); int tempRight = PlayerPrefs.GetInt("rightHandGunSelect");
        if(tempLeft == -1) { tempLeft = instance.data.gunInfo.Count - 1; }
        if(tempRight == -1) { tempRight = instance.data.gunInfo.Count - 1; }
        instance.data.gunInfo[tempLeft].runs++;
        instance.data.gunInfo[tempRight].runs++;
        instance.UpdateSaveFile();
    }
    public void ExitGameButton()
    {
        Application.Quit();
    }
    public void LoadLevel(bool continueRun)
    {
        if (loading) { return; } loading = true;
        if (!continueRun) { UpdateGunInfo(); }
        StartCoroutine(LoadAsyncScene("Area1"));
    }
    IEnumerator LoadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void OnHoverOver(bool playStandard)
    {
        if (playStandard)
        {
            usp.UIHoverSound(selectedDifficulty);
        }
    }
    public void UpdateDifficultyDisplayedInfo(int input)
    {
        selectedDifficulty = input;
        if(input == curDifDisplay) { return; } curDifDisplay = input;
        foreach(DiffDetail dd in difficultyInfo) { dd.detailsHolder.SetActive(false); }
        difficultyInfo[input].detailsHolder.SetActive(true);
        difficultyInfo[input].detailsHolder.transform.parent.GetComponent<Image>().sprite = difficultyInfo[input].bg;
        if(input == 4) { GenerateMutatedRules(); }
    }
    void GenerateMutatedRules()
    {
        List<int> nonStackables = new List<int>(); nonStackables.InsertRange(0, new int[] { 2,3,8,10,11 });
        int metaAttemptsLeft = 100;
        currentMutatedRules = new List<int>();
        while(currentMutatedRules.Count < 6 && metaAttemptsLeft > 0)
        {
            metaAttemptsLeft--;
            mutationID = "";
            currentMutatedRules = new List<int>();
            for (int i = 0; i < mutatedRulesHolder.childCount; i++)
            {
                TextMeshProUGUI ruleTxt = mutatedRulesHolder.GetChild(i).GetComponent<TextMeshProUGUI>();
                int index;
                int ruleRarity = Random.Range(0, 100);
                if (ruleRarity < 50) { ruleRarity = 0; }
                else if (ruleRarity < 80) { ruleRarity = 1; }
                else if (ruleRarity < 95) { ruleRarity = 2; }
                else if (ruleRarity < 100) { ruleRarity = 3; }
                switch (ruleRarity)
                {
                    case 0: index = Random.Range(0, rarity0Rules.Count); ruleTxt.text = rarity0Rules[index].rule; ruleTxt.color = rarity0Rules[index].color; currentMutatedRules.Add(rarity0Rules[index].id); mutationID += rarity0Rules[index].id + "|"; break;
                    case 1: index = Random.Range(0, rarity1Rules.Count); ruleTxt.text = rarity1Rules[index].rule; ruleTxt.color = rarity1Rules[index].color; currentMutatedRules.Add(rarity1Rules[index].id); mutationID += rarity1Rules[index].id + "|"; break;
                    case 2: index = Random.Range(0, rarity2Rules.Count); ruleTxt.text = rarity2Rules[index].rule; ruleTxt.color = rarity2Rules[index].color; currentMutatedRules.Add(rarity2Rules[index].id); mutationID += rarity2Rules[index].id + "|"; break;
                    case 3: index = Random.Range(0, rarity3Rules.Count); ruleTxt.text = rarity3Rules[index].rule; ruleTxt.color = rarity3Rules[index].color; currentMutatedRules.Add(rarity3Rules[index].id); mutationID += rarity3Rules[index].id + "|"; break;
                }
                index = currentMutatedRules[currentMutatedRules.Count - 1];
                List<int> prevRules = new List<int>(); prevRules.AddRange(currentMutatedRules);
                prevRules.RemoveAt(prevRules.Count - 1);
                if (nonStackables.Contains(index) && prevRules.Contains(index))
                {
                    currentMutatedRules = new List<int>();
                }
                ruleTxt.text += GetSpesificRandMutatedValue(index, i.ToString()); 
            }
            mutationID.Remove(mutationID.Length - 1);
            mutationIDText.text = "Current Mutation: " + mutationID;
        }
    }
    string GetSpesificRandMutatedValue(int type, string slot)
    {
        string output = "";
        
        switch (type)
        {
            case 7: PlayerPrefs.SetInt("MUTATEDRULEDOUBLESTATSLOT"+slot, Random.Range(0,29));
                switch (PlayerPrefs.GetInt("MUTATEDRULEDOUBLESTATSLOT" + slot))
                {
                    case 0: output += "Speed"; break;
                    case 1: output += "Sprint Speed"; break;
                    case 2: output += "Jump Height"; break;
                    case 3: output += "Number Of Jumps"; break;
                    case 4: output += "Crit Chance"; break;
                    case 5: output += "Crit Damage"; break;
                    case 6: output += "Weak Spot Damage"; break;
                    case 7: output += "Damage"; break;
                    case 8: output += "Attack Speed"; break;
                    case 9: output += "Reload Speed"; break;
                    case 10: output += "Magazine Size"; break;
                    case 11: output += "Accuracy"; break;
                    case 12: output += "Bullet Speed"; break;
                    case 13: output += "Bullet Size"; break;
                    case 14: output += "Pierce"; break;
                    case 15: output += "Crit Chance"; break;
                    case 16: output += "Crit Damage"; break;
                    case 17: output += "Weak Spot Damage"; break;
                    case 18: output += "Damage"; break;
                    case 19: output += "Attack Speed"; break;
                    case 20: output += "Reload Speed"; break;
                    case 21: output += "Magazine Size"; break;
                    case 22: output += "Accuracy"; break;
                    case 23: output += "Bullet Speed"; break;
                    case 24: output += "Bullet Size"; break;
                    case 25: output += "Pierce"; break;
                    case 26: output += "Max HP"; break;
                    case 27: output += "Passive HP Regen"; break;
                    case 28: output += "Armor"; break;
                }
                break;
            case 9: PlayerPrefs.SetInt("MUTATEDRULEHALFSTATSLOT"+slot, Random.Range(0, 29));
                switch (PlayerPrefs.GetInt("MUTATEDRULEHALFSTATSLOT" + slot))
                {
                    case 0: output += "Speed"; break;
                    case 1: output += "Sprint Speed"; break;
                    case 2: output += "Jump Height"; break;
                    case 3: output += "Number Of Jumps"; break;
                    case 4: output += "Crit Chance"; break;
                    case 5: output += "Crit Damage"; break;
                    case 6: output += "Weak Spot Damage"; break;
                    case 7: output += "Damage"; break;
                    case 8: output += "Attack Speed"; break;
                    case 9: output += "Reload Speed"; break;
                    case 10: output += "Magazine Size"; break;
                    case 11: output += "Accuracy"; break;
                    case 12: output += "Bullet Speed"; break;
                    case 13: output += "Bullet Size"; break;
                    case 14: output += "Pierce"; break;
                    case 15: output += "Crit Chance"; break;
                    case 16: output += "Crit Damage"; break;
                    case 17: output += "Weak Spot Damage"; break;
                    case 18: output += "Damage"; break;
                    case 19: output += "Attack Speed"; break;
                    case 20: output += "Reload Speed"; break;
                    case 21: output += "Magazine Size"; break;
                    case 22: output += "Accuracy"; break;
                    case 23: output += "Bullet Speed"; break;
                    case 24: output += "Bullet Size"; break;
                    case 25: output += "Pierce"; break;
                    case 26: output += "Max HP"; break;
                    case 27: output += "Passive HP Regen"; break;
                    case 28: output += "Armor"; break;
                }
                break;
            case 10:
                List<Spawnable> options = new List<Spawnable>(); options.AddRange(Resources.LoadAll<Spawnable>("Enemies"));
                PlayerPrefs.SetString("MUTATEDRULELONEENEMYSLOT"+slot, options[Random.Range(0, options.Count)].enemyName); output += PlayerPrefs.GetString("MUTATEDRULELONEENEMYSLOT"+slot); break;
        }

        return output;
    }
}
