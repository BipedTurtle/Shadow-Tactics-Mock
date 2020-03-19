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
        private bool _isWithinPlayerView;
        public bool IsWithinPlayerView { get => this._isWithinPlayerView; }

        private void Start()
        {
            Enemies.Add(this);
        }


        public void Mark(bool isWithinView) => this._isWithinPlayerView = isWithinView;


        private void OnDestroy()
        {
            Enemies.Remove(this);
        }
    }
}
