using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infohazard.Demos {
    public class DemoAvoidanceAgentSpawner : MonoBehaviour {
        [SerializeField] private int _spawnCount;
        [SerializeField] private float _spawnRadius;
        [SerializeField] private DemoAvoidanceAgentNonPathing _agentPrefab;

        private void Start() {
            for (int i = 0; i < _spawnCount; i++) {
                Vector3 pos = Random.onUnitSphere * _spawnRadius;
                Vector3 posNorm = pos / _spawnRadius;
                posNorm = (posNorm + Vector3.one) * 0.5f;

                Color color = new Color(posNorm.x, posNorm.y, posNorm.z);

                DemoAvoidanceAgentNonPathing agent = Instantiate(_agentPrefab, pos, Quaternion.identity);
                agent.SetColor(color);
            }
        }
    }
}