using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class PlayerItem : MonoBehaviour
    {
        public List<int> leftItems;
        public List<int> rightItems;

        public List<int> commonItems = new List<int>();
        public List<int> uncommonItems = new List<int>();
        public List<int> rareItems = new List<int>();
        public List<int> legendaryItems = new List<int>();
        public List<int> mutatedItems = new List<int>();
        public List<int> hauntedItems = new List<int>();
        public List<int> irradiatedItems = new List<int>();
        public List<int> nuclearItems = new List<int>();
        public List<int> uniqueItems = new List<int>();

        public List<List<int>> rarityList = new List<List<int>>();

        public PlayerMovement playerMvt;
        public HealthManager healthManager;
        public GunManager gunManager;

        public Transform playerCamera;

        public GameObject itemDisplay;

        private void Awake()
        {
            commonItems.InsertRange(0, new int[] { 0, 1, 2, 4, 6, 7, 8, 9, 60, 61, 62, 63, 64, 65, 78, 94, 173, 174, 175, 177 });
            uncommonItems.InsertRange(0, new int[] { 3, 10, 11, 12, 25, 28, 31, 32, 34, 35, 45, 47, 55, 114, 122, 148, 149, 150, 162, 165 });
            rareItems.InsertRange(0, new int[] { 5, 16, 26, 33, 36, 44, 57, 67, 76, 77, 82, 83, 86, 95, 101, 102, 119, 147, 156, 158 });
            legendaryItems.InsertRange(0, new int[] { 27, 38, 41, 42, 58, 75, 81, 88, 112, 116, 137, 154, 155, 164, 167, 178, 179, 182, 183, 184 });
            mutatedItems.InsertRange(0, new int[] { 17, 18, 23, 24, 69, 70, 71, 79, 117, 124, 129, 139, 140, 141, 144, 145, 146, 160, 161, 168 });
            hauntedItems.InsertRange(0, new int[] { 15, 19, 40, 43, 73, 96, 123, 125, 126, 127, 128, 130, 131, 132, 133, 134, 135, 136, 138, 151 });
            irradiatedItems.InsertRange(0, new int[] { 13, 14, 20, 22, 29, 39, 59, 80, 89, 90, 91, 92, 113, 118, 121, 153, 157, 159, 163, 166 });
            nuclearItems.InsertRange(0, new int[] { 21, 30, 37, 68, 74, 87, 93, 115, 120, 142, 152, 169, 170, 171, 172, 176, 180, 181, 185, 186 });
            uniqueItems.InsertRange(0, new int[] { 46, 48, 49, 50, 51, 52, 53, 54, 56, 66, 72, 84, 85, 97, 98, 99, 100, 103, 104, 105, 106, 107, 108, 109, 110, 111, 143, 187, 188, 189, 190 });

            rarityList.InsertRange(0, new List<int>[] { commonItems, uncommonItems, rareItems, legendaryItems, mutatedItems, hauntedItems, irradiatedItems, nuclearItems, uniqueItems });
        }

        private void Update()
        {
            playerMvt.StatUpdate(leftItems, rightItems, rarityList);
            healthManager.StatUpdate(leftItems, rightItems, rarityList);
            gunManager.StatUpdate(leftItems, rightItems, rarityList);


            LookForItem();
        }

        void LookForItem()
        {
            Vector3 camPos = playerCamera.position;
            Ray ray = new Ray(camPos, playerCamera.forward);
            RaycastHit hit;

            Debug.DrawLine(camPos, camPos + playerCamera.forward * 7f);
            if (Physics.Raycast(ray, out hit, 7f))
            {
                if(hit.collider.gameObject.tag == "item")
                {
                    Vector3 hitItem = hit.collider.gameObject.transform.position;

                    itemDisplay.SetActive(true);
                    itemDisplay.GetComponent<ItemDisplayScript>().InfoUpdate(hit.collider.gameObject.GetComponentInParent<Item>().WhatItem(), hitItem);

                    hit.collider.gameObject.GetComponentInParent<Item>().StayStill();

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        rightItems[hit.collider.gameObject.GetComponentInParent<Item>().WhatItem()] += 1;
                        Debug.Log("Item of Item ID " + hit.collider.gameObject.GetComponentInParent<Item>().WhatItem() + " added to inventory");
                        Destroy(hit.collider.gameObject);
                    }
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        leftItems[hit.collider.gameObject.GetComponentInParent<Item>().WhatItem()] += 1;
                        Debug.Log("Item of Item ID " + hit.collider.gameObject.GetComponentInParent<Item>().WhatItem() + " added to inventory");
                        Destroy(hit.collider.gameObject);
                    }
                }
                else
                {
                    itemDisplay.SetActive(false);
                }
            }
            else
            {
                itemDisplay.SetActive(false);
            }
        }
    }

