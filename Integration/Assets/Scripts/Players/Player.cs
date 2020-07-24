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
            // slow time effect
            Time.timeScale = 0.10f;
            yield return new WaitForSeconds(0.25f);

            SceneId gameOverScreen = SceneId.GameOverMenu;
            yield return sceneLoadingManagerProxy.PreloadMainSceneAsync(gameOverScreen);

            Time.timeScale = 1;
            sceneLoadingManagerProxy.Activate(gameOverScreen);
        }
    }
}