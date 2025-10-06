using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BIGVALUE
{
    //Static data
    public static string[] unitNames = new string[] { "", "K", "M", "B", "T", "Q" }; //Every 3 zeros, upgrade to the next unit.
    //Dynamic data
    public bool negative = false;
    public int[] value = new int[] {0,0,0,0,0,0};
    public int greatestUnit = 0; //unit index of the highest unit this value has
    
    //Functions
    public static BIGVALUE BIGAdd(BIGVALUE a, BIGVALUE b, bool aboslute, bool reverse)
    {
        BIGVALUE result = new BIGVALUE();
        int rVal; int aVal; int bVal;

        if((!a.negative && !b.negative) || aboslute)
        {
            for (int i = 0; i < unitNames.Length; i++)
            {
                rVal = result.value[i]; aVal = a.value[i]; bVal = b.value[i];
                rVal += aVal + bVal;

                if (rVal > 999 && i < unitNames.Length - 1) { result.value[i + 1] += 1; rVal -= 1000; } //Overflow
                result.value[i] = rVal;
            }
        }
        else if (a.negative && b.negative)
        {
            for (int i = 0; i < unitNames.Length; i++)
            {
                rVal = result.value[i]; aVal = a.value[i]; bVal = b.value[i];
                rVal += aVal + bVal;

                if (rVal > 999 && i < unitNames.Length - 1) { result.value[i + 1] += 1; rVal -= 1000; } //Overflow
                result.value[i] = rVal;
            }
            result.negative = true;
        }
        else
        {
            if (a.negative) { return BIGSubtract(a, b, true, true); }
            else { return BIGSubtract(a, b, true, false); }
        }

        if (reverse) { result.negative = !result.negative; }
        result.ValueUpdated(); return result;
    }

    public static BIGVALUE BIGSubtract(BIGVALUE a, BIGVALUE b, bool absolute, bool reverse)
    {
        BIGVALUE result = new BIGVALUE();
        BIGVALUE l = new BIGVALUE(); BIGVALUE s = new BIGVALUE();
        bool reversedOrder = false;
        int compareVal = BIGCompare(a, b);
        switch (compareVal)
        {
            case 1: l = a; s = b; reversedOrder = false; break;
            case 0: return result;
            case -1: l = b; s = a; reversedOrder = true; break;
        }
        int rVal; int lVal; int sVal;

        if ((!a.negative && !b.negative) || absolute)
        {
            for (int i = unitNames.Length-1; i > 0; i--)
            {
                lVal = l.value[i]; sVal = s.value[i];
                rVal = lVal - sVal;

                if (rVal < 0) { result.value[i - 1] -= 1; rVal += 1000; } //Overflow
                result.value[i] = rVal;
            }
        }
        else if (a.negative && b.negative)
        {
            for (int i = unitNames.Length - 1; i > 0; i--)
            {
                lVal = l.value[i]; sVal = s.value[i];
                rVal = lVal - sVal;

                if (rVal < 0) { result.value[i - 1] -= 1; rVal += 1000; } //Overflow
                result.value[i] = rVal;
            }
            result.negative = true;
        }
        else
        {
            if (a.negative) { return BIGAdd(a, b, true, true); }
            else { return BIGAdd(a, b, true, false); }
        }

        result.negative = reversedOrder;
        if (reverse) { result.negative = !result.negative; }
        result.ValueUpdated(); return result;
    }
    public static BIGVALUE BIGMultiply(BIGVALUE a, BIGVALUE b)
    {
        BIGVALUE result = new BIGVALUE(); 
        BIGVALUE const1 = new BIGVALUE(); const1.value[0] = 1;
        BIGVALUE const0 = new BIGVALUE();
        BIGVALUE multCount = new BIGVALUE(); multCount.value = (int[]) b.value.Clone();
        
        while(BIGCompare(multCount, const0) > 0)
        {
            BIGAdd(result, a, false, false);

            multCount = BIGSubtract(multCount, const1, false, false);
        }

        if (a.negative) { result.negative = !result.negative; }
        if (b.negative) { result.negative = !result.negative; }
        result.ValueUpdated(); return result;
    }
    public static BIGVALUE BIGDivide(BIGVALUE a, BIGVALUE b)
    {
        BIGVALUE result = new BIGVALUE();



        result.ValueUpdated(); return result;
    }
    public static int BIGCompare(BIGVALUE a, BIGVALUE b)
    {
        if (a.greatestUnit > b.greatestUnit) { return 1; }
        else if (b.greatestUnit > a.greatestUnit) { return -1; }
        else if (a.value[a.greatestUnit] > b.value[b.greatestUnit]) { return 1; }
        else if (b.value[b.greatestUnit] > a.value[a.greatestUnit]) { return -1; }
        else 
        { 
            for(int i = unitNames.Length-1; i > 0; i--)
            {
                if (a.value[i] > b.value[i]) { return 1; }
                else if (b.value[i] > a.value[i]) { return -1; }
            }
            return 0;
        }
    }
    public void ValueUpdated()
    {
        for (int i = value.Length -1; i > 0; i--)
        {
            if (value[i] > 0) { greatestUnit = i; return; }
        }
    }
    public static string ConvertToStringSHORT(BIGVALUE a)
    {
        string result;
        string vala; string valb;

        if (a.greatestUnit > 0) { vala = a.value[a.greatestUnit].ToString(); valb = a.value[a.greatestUnit - 1].ToString(); while (valb.Length < 3) { valb = "0" + valb; } }
        else { vala = a.value[a.greatestUnit].ToString(); valb = ""; }

        result = vala + "." + valb + unitNames[a.greatestUnit];
        if(valb == "000" || valb == "") { result = vala + unitNames[a.greatestUnit]; }

        if (a.negative) { result = "-" + result; }

        return result;
    }
    public static string ConvertToStringLONG(BIGVALUE a)
    {
        string result = "";
        string val;

        if(a.greatestUnit < 2) { return ConvertToStringSHORT(a); }

        for (int i = a.greatestUnit; i > 0; i--)
        {
            if (a.value[i] > 0)
            {
                if (i > 0) { result += a.value[i] + unitNames[i]; result += ", "; }
                else 
                {
                    val = a.value[i].ToString();
                    while (val.Length < 3) { val = "0" + val; }
                    result += val + unitNames[i];
                }
            }
        } if (result == "") { result = "0"; }

        if (a.negative) { result = "-" + result; }

        result.TrimEnd(' ');
        result.TrimEnd(',');
        result.TrimEnd('.');

        return result;
    }
}
