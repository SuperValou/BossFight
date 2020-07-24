﻿using System;
using System.Collections;
using Assets.Scripts.Damages;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class Player : Damageable
    {
        // -- Editor

        [Header("References")]
        [Tooltip("Handle scene loading when the player dies.")]
        public SceneLoadingManagerProxy sceneLoadingManagerProxy;

        // -- Class
        private FirstPersonController _firstPersonController;
        
        void Start()
        {
            _firstPersonController = this.GetOrThrow<FirstPersonController>();
        }

        protected override void OnDamageTaken()
        {
            // TODO: say ouch
        }

        protected override void Die()
        {
            StartCoroutine(DieAsync());
        }

        private IEnumerator DieAsync()
        {
            // TODO: game over
            Debug.LogWarning("Game over");
            
            Time.timeScale = 1f / 10;
            yield return new WaitForSeconds(0.5f);

            yield return sceneLoadingManagerProxy.LoadMainSceneAsync(SceneId.GameOverMenu);
        }
    }
}