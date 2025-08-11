using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public RemoteDoor bgDoor;
    Camera cam;
    bool starting;
    public List<GameObject> uiTOHIDE; bool loading;
    public SaveFileReadWrite instance;
    WeaponSelection weaponSelect;
    UiSoundPlayer usp;
    public int selectedDifficulty;
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
    List<int> currentMutatedRules = new List<int>();
    public Transform mutatedRulesHolder; public TextMeshProUGUI mutationIDText; public string mutationID;
    Animator camAnim;
    private void Awake()
    {
        weaponSelect = GameObject.Find("BEHINDDOORUI").GetComponent<WeaponSelection>();
        usp = GetComponent<UiSoundPlayer>();
        Time.timeScale = 1f;
        if (PlayerPrefs.HasKey("SELECTEDDIFFICULTY")) { selectedDifficulty = PlayerPrefs.GetInt("SELECTEDDIFFICULTY"); } else { selectedDifficulty = 1; PlayerPrefs.SetInt("SELECTEDDIFFICULTY", 1); }
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
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && starting) { Back(); }
    }
    public void Play(string what)
    {
        if(what == "new")
        {
            if (!starting)
            {
                bgDoor.Activate();
                camAnim.SetBool("Leaving", true);
                starting = true;
                foreach (GameObject go in uiTOHIDE)
                {
                    go.SetActive(false);
                }
                usp.UIDifficultySound(selectedDifficulty);
                PlayerPrefs.SetInt("SELECTEDDIFFICULTY", selectedDifficulty);
            }
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
            go.SetActive(true);
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
        instance.data.gunInfo[PlayerPrefs.GetInt("leftHandGunSelect")].runs++;
        instance.data.gunInfo[PlayerPrefs.GetInt("rightHandGunSelect")].runs++;
        instance.UpdateSaveFile();
    }
    public void ExitGameButton()
    {
        Application.Quit();
    }
    public void LoadLevel()
    {
        if (loading) { return; } loading = true;
        UpdateGunInfo();
        StartCoroutine(LoadAsyncScene("Level Generation"));
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
        if(input == curDifDisplay) { return; } curDifDisplay = input;
        foreach(DiffDetail dd in difficultyInfo) { dd.detailsHolder.SetActive(false); }
        difficultyInfo[input].detailsHolder.SetActive(true);
        difficultyInfo[input].detailsHolder.transform.parent.GetComponent<Image>().sprite = difficultyInfo[input].bg;
        if(input == 4) { GenerateMutatedRules(); }
    }
    void GenerateMutatedRules()
    {
        mutationID = "";
        currentMutatedRules = new List<int>();
        for(int i = 0; i < mutatedRulesHolder.childCount; i++)
        {
            TextMeshProUGUI ruleTxt = mutatedRulesHolder.GetChild(i).GetComponent<TextMeshProUGUI>();
            int ruleRarity = Random.Range(0,100);
            if (ruleRarity < 50) { ruleRarity = 0; }
            else if (ruleRarity < 80) { ruleRarity = 1; }
            else if (ruleRarity < 95) { ruleRarity = 2; }
            else if (ruleRarity < 100) { ruleRarity = 3; }
            int index;
            switch (ruleRarity)
            {
                case 0: index = Random.Range(0,rarity0Rules.Count); ruleTxt.text = rarity0Rules[index].rule; ruleTxt.color = rarity0Rules[index].color; currentMutatedRules.Add(rarity0Rules[index].id); mutationID += rarity0Rules[index].id + "|"; break;
                case 1: index = Random.Range(0,rarity1Rules.Count); ruleTxt.text = rarity1Rules[index].rule; ruleTxt.color = rarity1Rules[index].color; currentMutatedRules.Add(rarity1Rules[index].id); mutationID += rarity1Rules[index].id + "|"; break;
                case 2: index = Random.Range(0,rarity2Rules.Count); ruleTxt.text = rarity2Rules[index].rule; ruleTxt.color = rarity2Rules[index].color; currentMutatedRules.Add(rarity2Rules[index].id); mutationID += rarity2Rules[index].id + "|"; break;
                case 3: index = Random.Range(0,rarity3Rules.Count); ruleTxt.text = rarity3Rules[index].rule; ruleTxt.color = rarity3Rules[index].color; currentMutatedRules.Add(rarity3Rules[index].id); mutationID += rarity3Rules[index].id + "|"; break;
            }
        }
        mutationID.Remove(mutationID.Length-1);
        mutationIDText.text = "Current Mutation: " + mutationID;
    }
}
