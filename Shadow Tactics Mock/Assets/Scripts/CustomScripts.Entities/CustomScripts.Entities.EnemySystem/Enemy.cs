using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomScripts.Entities.EnemySystem
{
    public class Enemy : MonoBehaviour
    {
        public static List<Enemy> Enemies { get; } = new List<Enemy>();
        public Vector3 Position { get => transform.position; }
        private bool isWithinPlayerView;

        private void Start()
        {
            Enemies.Add(this);
        }


        public void Mark(bool isWithinView) => this.isWithinPlayerView = isWithinView;
    }
}
