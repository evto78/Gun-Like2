using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBranch : MonoBehaviour
{
    public AnimationCurve curve;
    public GameObject baseBranch;
    public EnemyHealthManager attachedEHM;
    public List<GameObject> branches;
    List<Vector3> targetBranchAngles = new List<Vector3>();
    List<Vector3> initialBranchAngles = new List<Vector3>();
    float timer;
    float deathTimer;
    public float timeToGrow;
    float dmgTimer;
    float selfDmgTimer;
    public float damage;
    void Start()
    {
        deathTimer = timeToGrow;
        timer = 0f;
        baseBranch.transform.localScale = Vector3.one * 0.1f;
        foreach(GameObject branch in branches)
        {
            initialBranchAngles.Add(branch.transform.localEulerAngles);
            targetBranchAngles.Add(branch.transform.localEulerAngles += new Vector3(Random.Range(-60f, 60f), Random.Range(-60f, 60f), Random.Range(-60f, 60f)));
        }
        Destroy(gameObject, timeToGrow*10f);
    }
    void Update()
    {
        if (dmgTimer < 0f) { dmgTimer = 0.25f; } if (selfDmgTimer < 0f) { selfDmgTimer = 0.75f; }
        timer += Time.deltaTime;
        if (timer < timeToGrow) 
        { 
            baseBranch.transform.localScale = Vector3.Lerp(Vector3.one * 0.1f, Vector3.one * 1.5f, curve.Evaluate(timer / timeToGrow));
            for(int i = 0; i < branches.Count; i++)
            {
                branches[i].transform.localEulerAngles = Vector3.Lerp(initialBranchAngles[i],targetBranchAngles[i],timer/timeToGrow);
            }
        }
        else if(deathTimer > 0)
        {
            deathTimer -= Time.deltaTime / 4f;
            baseBranch.transform.localScale = Vector3.Lerp(Vector3.one * 0.1f, Vector3.one * 1.5f, curve.Evaluate(deathTimer / timeToGrow));
            for (int i = 0; i < branches.Count; i++)
            {
                branches[i].transform.localEulerAngles = Vector3.LerpUnclamped(initialBranchAngles[i], targetBranchAngles[i], timer / timeToGrow);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        dmgTimer -= Time.deltaTime;
        selfDmgTimer -= Time.deltaTime;
    }
    private void OnTriggerStay(Collider collision)
    {
        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyWeakPoint"))
        {

            if(collision.gameObject.GetComponentInParent<EnemyHealthManager>() != attachedEHM && dmgTimer <= 0f)
            {
                collision.gameObject.GetComponentInParent<EnemyHealthManager>().QueStandardDamage(damage);
            }
            else if(collision.gameObject.GetComponentInParent<EnemyHealthManager>() != attachedEHM && selfDmgTimer <= 0f)
            {
                collision.gameObject.GetComponentInParent<EnemyHealthManager>().QueStandardDamage(1);
            }
        }
    }
}
