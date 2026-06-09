using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotMachineInteractable : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Sprite icon;
        public int weight;
        public Color color;
    }
    public List<Transform> wheels;
    public List<SpriteRenderer> slots;
    public List<Slot> posibilities;
    List<int> weightedList;
    public float spinSpeed;
    public GameObject explosion;

    Animator anim;
    public int cost;
    GameObject player;
    PlayerItem pi;
    GameDataManager gdm;
    bool interacted;
    public float priceModifier = 1;

    int spinsMade = 0; float moneyMade = 0; float netMoneyMade = 0;

    public TextMeshProUGUI costTxt;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        player = gdm.phm.gameObject;
        pi = gdm.phm.playerItem;

        cost = Mathf.CeilToInt(gdm.phm.baseCost * priceModifier * (int)(gdm.difficulty * (gdm.roomNumber + 1)));
        costTxt.text = cost.ToString() + "$";

        weightedList = GetWeightedList();
    }
    public void Interact()
    {
        if (interacted) { return; }
        if (pi.healthManager.money < cost) { return; }
        pi.healthManager.money -= cost;
        pi.spentMoneyThisRoom = true;
        netMoneyMade -= cost;
        interacted = true; spinsMade++;
        anim.SetTrigger("Spin");

        StartCoroutine(SpinWheel(0)); StartCoroutine(SpinWheel(1));
        StartCoroutine(SpinWheel(2)); StartCoroutine(SpinWheel(3));
    }
    List<int> GetWeightedList()
    {
        List<int> output = new List<int>();

        for(int i = 0; i < posibilities.Count; i++)
        {
            for(int y = 0; y < posibilities[i].weight; y++) { output.Add(i); }
        }

        return output;
    }
    int GetResult() { return weightedList[Random.Range(0, weightedList.Count)]; }
    IEnumerator SpinWheel(int id)
    {
        int result = GetResult();
        float cyclesLeft = id+3;
        float rotation = 0f;
        while(cyclesLeft > 0)
        {
            cyclesLeft -= Time.deltaTime * spinSpeed;
            rotation += 360f * spinSpeed * Time.deltaTime;
            wheels[id].localEulerAngles = Vector3.right * rotation;
            yield return new WaitForEndOfFrame();
        }
        float tmp = 0f;
        slots[id].sprite = posibilities[result].icon; slots[id].color = posibilities[result].color;
        rotation /= 2f;
        while(tmp < 1)
        {
            tmp += Time.deltaTime * spinSpeed;
            rotation = Mathf.Lerp(rotation, 0, tmp);
            wheels[id].localEulerAngles = Vector3.right * rotation;
            yield return new WaitForEndOfFrame();
        }
        GiveReward(result);
        if(id == 3) { interacted = false; }
        yield return null;
    }
    void GiveReward(int id)
    {
        switch (id)
        {
            case 0: StartCoroutine(VendReward(cost*2)); weightedList.Add(3); moneyMade += cost * 2; netMoneyMade += cost * 2; break;
            case 1: StartCoroutine(VendReward(cost)); weightedList.Add(3); moneyMade += cost; netMoneyMade += cost; break;
            case 2: StartCoroutine(VendReward(cost/2)); weightedList.Add(3); moneyMade += cost/2; netMoneyMade += cost/2; break;
            case 3: Explode(); break;
            case 4: cost += (Mathf.CeilToInt(gdm.phm.baseCost * priceModifier * (int)(gdm.difficulty * (gdm.roomNumber + 1)))); costTxt.text = cost.ToString() + "$"; break;
        }
    }
    void Explode()
    {
        Debug.Log("spins: " + spinsMade + " | money made: " + moneyMade + " | net money made: " + netMoneyMade);
        Instantiate(explosion, transform.position, transform.rotation);
        explosion.GetComponent<NuclearExplosion>().damage = cost / 8;
        Destroy(gameObject);
    }
    IEnumerator VendReward(int winnings)
    {
        while (winnings > 0)
        {
            int mult;
            if (winnings > 100) { mult = 20; }
            else if (winnings > 50) { mult = 10; }
            else if (winnings > 25) { mult = 5; }
            else { mult = 1; }
            pi.healthManager.money += mult;
            winnings -= mult;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
