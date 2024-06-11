namespace Zen
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> m_basicOjectPrefabs;
        [SerializeField] private List<GameObject> m_jewelry0bjectPrefabs;
        [SerializeField] private int m_basicMaxObjects   = 20;
        [SerializeField] private int m_jewelryMaxObjects = 5;

        private HashSet<Vector3> m_spawnedPositions = new HashSet<Vector3>();

        public void Create_Object()
        {
            Spawn_Objects(m_basicMaxObjects, m_basicOjectPrefabs);
            Spawn_Objects(m_jewelryMaxObjects, m_jewelry0bjectPrefabs);
        }

        private void Spawn_Objects(int maxCount, List<GameObject> prefabs)
        {
            for (int i = 0; i < maxCount; i++)
            {
                Vector3 randomPosition = Get_RandomPosition();
                while (m_spawnedPositions.Contains(randomPosition))
                    randomPosition = Get_RandomPosition();

                Instantiate(prefabs[Random.Range(0, prefabs.Count)], randomPosition, Quaternion.identity);
                m_spawnedPositions.Add(randomPosition);
            }
        }

        private Vector3 Get_RandomPosition()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-0.4f, 0.2f), 0f);
        }
    }
}



