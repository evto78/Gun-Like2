using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace peterkcodes.AdvancedMovement
{
    public class PlayerItem : MonoBehaviour
    {
        public List<int> playerItems;
        public PlayerMovement playerMvt;
        public HealthManager healthManager;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "item")
            {
                playerItems[collision.gameObject.GetComponentInParent<Item>().WhatItem()] += 1;
                Debug.Log("Item of Item ID " + collision.gameObject.GetComponentInParent<Item>().WhatItem() + " added to inventory");
                Destroy(collision.gameObject);
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "item")
            {
                playerItems[collision.gameObject.GetComponentInParent<Item>().WhatItem()] += 1;
                Debug.Log("Item of Item ID " + collision.gameObject.GetComponentInParent<Item>().WhatItem() + " added to inventory");
                Destroy(collision.gameObject);
            }
        }

        private void Update()
        {
            playerMvt.StatUpdate(playerItems);
            
        }
    }

}
