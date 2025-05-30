﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obi.Samples
{
    public class ActorSpawner : MonoBehaviour
    {

        public ObiActor template;

        public int maxInstances = 32;
        public float spawnDelay = 0.3f;

        private int instances = 0;
        private float timeFromLastSpawn = 0;

        // Update is called once per frame
        void Update()
        {

            timeFromLastSpawn += Time.deltaTime;

            if (Input.GetMouseButtonDown(0) && instances < maxInstances && timeFromLastSpawn > spawnDelay)
            {
                GameObject go = Instantiate(template.gameObject, transform.position, Quaternion.identity);
                go.transform.SetParent(transform.parent);
                instances++;
                timeFromLastSpawn = 0;
            }
        }
    }
}