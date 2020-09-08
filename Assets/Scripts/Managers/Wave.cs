using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public float timeBtwGroups;
    public List<Enemies> enemiesToSpawn;

    public Wave(float _timeBtwGroups, List<Enemies> _enemiesToSpawn)
    {
        timeBtwGroups = _timeBtwGroups;
        enemiesToSpawn = _enemiesToSpawn;
    }

    [System.Serializable]
    public class Enemies
    {
        public string groupName;
        [Range(1,7)]
        public int index = 1;
        public int number;
        [Range(1, 5)]
        public int level = 1;
        public float spawnRate;
        public Enemies(int _index,int _number, int _level,float _spawnRate)
        {
            index = _index;
            number = _number;
            level = _level;
            spawnRate = _spawnRate;
        }
    }

}
