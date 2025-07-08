using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRarity
{
    //These MUST add up to 100
    static public int commonChance = 70;
    static public int uncommonChance = 20;
    static public int rareChance = 2;
    static public int mutatedChance = 2;
    static public int hauntedChance = 2;
    static public int irradiatedChance = 2;
    static public int nuclearChance = 1;
    static public int legendaryChance = 1;
    static public int GetWeightedRandRarity()
    {
        int chosenRarity = 0;

        int rand = Random.Range(1, 101);
        int collectiveChance = 0;
        collectiveChance += commonChance; if(rand <= collectiveChance) { return 0; } 
        collectiveChance += uncommonChance; if(rand <= collectiveChance) { return 1; } 
        collectiveChance += rareChance; if(rand <= collectiveChance) { return 2; } 
        collectiveChance += mutatedChance; if(rand <= collectiveChance) { return 4; } 
        collectiveChance += hauntedChance; if(rand <= collectiveChance) { return 5; } 
        collectiveChance += irradiatedChance; if(rand <= collectiveChance) { return 6; } 
        collectiveChance += nuclearChance; if(rand <= collectiveChance) { return 7; } 
        collectiveChance += legendaryChance; if(rand <= collectiveChance) { return 3; } 

        return chosenRarity;
    }
    static public int GetUnWeightedRandRarity()
    {
        int chosenRarity = Random.Range(0, 8);
        return chosenRarity;
    }
}
