using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BIGCalcTester : MonoBehaviour
{
    BIGVALUE valueA;
    BIGVALUE valueB;

    BIGVALUE result;

    public enum OPERATION { Add, Subtract, Multiply};
    OPERATION selectedOperation;

    float delay; bool done;

    void Start()
    {
        done = false; delay = 1f;
    }
    private void Update()
    {
        if (done) { return; }        
        delay -= Time.deltaTime;
        if(delay < 0)
        {
            Debug.Log("Calculating...");
            //TestBIGVALUE();
            TestOthers();
            done = true;
        }
    }
    void TestOthers()
    {
        
    }
    void TestBIGVALUE()
    {
        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 0, 0, 0, 0, 0, 0 };
        valueB.value = new int[] { 0, 0, 0, 0, 0, 0 };
        selectedOperation = OPERATION.Add;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 1, 0, 0, 0, 0, 0 };
        valueB.value = new int[] { 1, 0, 0, 0, 0, 0 };
        selectedOperation = OPERATION.Add;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 0, 298, 478, 233, 555, 773 };
        valueB.value = new int[] { 0, 0, 200, 201, 999, 263 };
        selectedOperation = OPERATION.Add;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 999, 999, 999, 999, 999, 999 };
        valueB.value = new int[] { 999, 999, 999, 999, 999, 999 };
        selectedOperation = OPERATION.Add;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 999, 999, 999, 999, 999, 999 };
        valueB.value = new int[] { 1, 0, 0, 0, 0, 0 };
        selectedOperation = OPERATION.Add;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 999, 999, 999, 999, 999, 999 };
        valueB.value = new int[] { 1, 0, 0, 0, 0, 0 };
        selectedOperation = OPERATION.Subtract;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 0, 0, 0, 0, 0, 1 };
        valueB.value = new int[] { 999, 999, 999, 999, 999, 999 };
        selectedOperation = OPERATION.Subtract;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 298, 478, 233, 555, 773, 109 };
        valueB.value = new int[] { 0, 200, 201, 999, 263, 654 };
        selectedOperation = OPERATION.Subtract;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 298, 478, 233, 555, 773, 109 }; valueA.negative = true;
        valueB.value = new int[] { 0, 200, 201, 999, 263, 654 };
        selectedOperation = OPERATION.Add;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 298, 478, 233, 555, 773, 109 };
        valueB.value = new int[] { 0, 200, 201, 999, 263, 654 }; valueB.negative = true;
        selectedOperation = OPERATION.Add;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 298, 478, 233, 555, 773, 109 }; valueA.negative = true;
        valueB.value = new int[] { 0, 200, 201, 999, 263, 654 }; valueB.negative = true;
        selectedOperation = OPERATION.Add;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 298, 478, 233, 555, 773, 109 }; valueA.negative = true;
        valueB.value = new int[] { 0, 200, 201, 999, 263, 654 };
        selectedOperation = OPERATION.Subtract;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 298, 478, 233, 555, 773, 109 };
        valueB.value = new int[] { 0, 200, 201, 999, 263, 654 }; valueB.negative = true;
        selectedOperation = OPERATION.Subtract;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 298, 478, 233, 555, 773, 109 }; valueA.negative = true;
        valueB.value = new int[] { 0, 200, 201, 999, 263, 654 }; valueB.negative = true;
        selectedOperation = OPERATION.Subtract;
        Calculate();

        valueA = new BIGVALUE(); valueB = new BIGVALUE(); result = new BIGVALUE();
        valueA.value = new int[] { 0, 1, 0, 0, 0, 0 };
        valueB.value = new int[] { 0, 1, 0, 0, 0, 0 }; valueB.negative = true;
        selectedOperation = OPERATION.Multiply;
        Calculate();
    }
    void Calculate()
    {
        valueA.ValueUpdated();
        valueB.ValueUpdated();
        switch (selectedOperation)
        {
            case OPERATION.Add:
                result = BIGVALUE.BIGAdd(valueA, valueB, false, false);
                Debug.Log("("+BIGVALUE.ConvertToStringLONG(valueA)+") + ("+BIGVALUE.ConvertToStringLONG(valueB)+") =");
                break;
            case OPERATION.Subtract:
                result = BIGVALUE.BIGSubtract(valueA, valueB, false, false);
                Debug.Log("(" + BIGVALUE.ConvertToStringLONG(valueA) + ") - (" + BIGVALUE.ConvertToStringLONG(valueB) + ") =");
                break;
            case OPERATION.Multiply:
                result = BIGVALUE.BIGMultiply(valueA, valueB);
                Debug.Log("(" + BIGVALUE.ConvertToStringLONG(valueA) + ") x (" + BIGVALUE.ConvertToStringLONG(valueB) + ") =");
                break;
        }
        Debug.Log(BIGVALUE.ConvertToStringSHORT(result));
        Debug.Log(BIGVALUE.ConvertToStringLONG(result));
        Debug.Log("----------------------");
    }
}
