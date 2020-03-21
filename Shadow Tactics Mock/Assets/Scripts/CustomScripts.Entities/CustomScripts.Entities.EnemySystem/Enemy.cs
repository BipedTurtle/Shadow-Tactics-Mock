using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CustomScripts.Managers;

namespace CustomScripts.Entities.EnemySystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Enemy : MonoBehaviour
    {
        public static List<Enemy> Enemies { get; } = new List<Enemy>();

        public Vector3 Position { get => transform.position; }
        private bool _isWithinPlayerView;
        public bool IsWithinPlayerView { get => this._isWithinPlayerView; }

        protected NavMeshAgent agent;
        protected virtual void Start()
        {
            Enemies.Add(this);

            this.agent = GetComponent<NavMeshAgent>();
        }


        public void Mark(bool isWithinView) => this._isWithinPlayerView = isWithinView;


        private void OnDestroy()
        {
            Enemies.Remove(this);
        }

        protected abstract void Patrol();
    }
}
