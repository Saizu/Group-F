﻿using UnityEngine;
using System.Collections;

namespace Complete
{
    /// 戦車の生死を管理するクラス
    /// 
    /// HPが0になると爆発エフェクトを発生して死ぬ。
    public class TankHealth : MonoBehaviour
    {
        /// HPを表示するUI
        /// 
        /// WARN: インスタンス化した時に適切にセットする必要がある。
        private GameObject m_PlayerInfo = null;
        public void SetPlayerInfo (GameObject playerInfo) => m_PlayerInfo = playerInfo;

        public float m_StartingHealth = 100f;               // The amount of health each tank starts with.
        public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.
        
        
        private AudioSource m_ExplosionAudio;               // The audio source to play when the tank explodes.
        private ParticleSystem m_ExplosionParticles;        // The particle system the will play when the tank is destroyed.
        private float m_CurrentHealth;                      // How much health the tank currently has.
        private bool m_Dead;                                // Has the tank been reduced beyond zero health yet?
        public float overHP = 0.0f;

        private void Awake ()
        {
            // Instantiate the explosion prefab and get a reference to the particle system on it.
            m_ExplosionParticles = Instantiate (m_ExplosionPrefab).GetComponent<ParticleSystem> ();

            // Get a reference to the audio source on the instantiated prefab.
            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource> ();

            // Disable the prefab so it can be activated when it's required.
            m_ExplosionParticles.gameObject.SetActive (false);
        }

        private void OnEnable()
        {
            m_Dead = false;
            // 初期化処理を遅延させるためにコルーチンを使用
            StartCoroutine(InitializeHealthWithDelay());
            Debug.Log("1:" + m_CurrentHealth);
        }

        private IEnumerator InitializeHealthWithDelay()
        {
            
            yield return null;  

            // 1フレーム後に初期HPを設定
            m_CurrentHealth = m_StartingHealth;
            
            // UIを更新
            SetHealthUI();

            // 初期化後にログを出力
            Debug.Log("2:" + m_CurrentHealth);
        }

        public void TakeDamage (float amount)
        {
            // Reduce current health by the amount of damage done.
            m_CurrentHealth -= amount;
            Debug.Log(m_CurrentHealth);
            // Change the UI elements appropriately.
            SetHealthUI ();

            // If the current health is at or below zero and it has not yet been registered, call OnDeath.
            if (m_CurrentHealth <= 0f && !m_Dead)
            {
                OnDeath ();
            }
        }

        private void SetHealthUI ()
        {
            if(m_CurrentHealth > 100f)
            {
                overHP = m_CurrentHealth - 100f;
                m_CurrentHealth -= overHP;
            }
            if (m_PlayerInfo == null)
            {
                return;
            }
            if (m_StartingHealth > 0.0f)
            {
                
                if(overHP > 0f)
                {
                    m_PlayerInfo.GetComponent<PlayerInfo>().UpdateOverHP(overHP / 100f);
                }
                else
                {
                    m_PlayerInfo.GetComponent<PlayerInfo>().UpdateOverHP(0f);
                    m_PlayerInfo.GetComponent<PlayerInfo>().UpdateHP(m_CurrentHealth / 100f);
                }
            }
            else
            {
                m_PlayerInfo.GetComponent<PlayerInfo>().UpdateHP(0.0f);
            }
            //初期化
            m_CurrentHealth += overHP;
            overHP = 0f;            
        }

        private void OnDeath ()
        {
            // Set the flag so that this function is only called once.
            m_Dead = true;

            // Move the instantiated explosion prefab to the tank's position and turn it on.
            m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.gameObject.SetActive (true);

            // Play the particle system of the tank exploding.
            m_ExplosionParticles.Play ();

            // Play the tank explosion sound effect.
            m_ExplosionAudio.Play ();

            // Turn the tank off.
            gameObject.SetActive (false);
        }
    }
}
