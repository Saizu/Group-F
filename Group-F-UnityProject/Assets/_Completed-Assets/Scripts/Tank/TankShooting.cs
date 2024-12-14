using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
        [SerializeField] private WeaponStockData m_ShellStockData;  // 銃弾の所持データ
        [SerializeField] private WeaponStockData m_MineStockData; // 地雷の所持データ
        [SerializeField] private GameObject m_MinePrefab;
        [SerializeField] private GameObject skullMarkerPrefab;
        private Dictionary<string, WeaponStockData> weaponStockDictionary = new Dictionary<string, WeaponStockData>();

        public int m_PlayerNumber = 1;              // Used to identify the different players.
        public Rigidbody m_Shell;                   // Prefab of the shell.
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
        public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
        public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
        public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
        public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.

        private string m_FireButton;                // The input axis that is used for launching shells.
        private string m_PlaceMineKey;
        private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        private bool m_Fired;                       // Whether or not the shell has been launched with this button press.

        public event Action<string, int> OnWeaponStockChanged;
        public event Action OnMinePlaced;

        private bool m_IsCharging;
        private bool m_IsIncreasing = true;

        private void OnEnable()
        {
            // When the tank is turned on, reset the launch force and the UI
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_AimSlider.value = m_MinLaunchForce;
        }


        private void Start()
        {
            // The fire axis is based on the player number.
            m_FireButton = "Fire" + m_PlayerNumber;
            m_PlaceMineKey = "PlaceMine" + m_PlayerNumber;

            // The rate that the launch force charges up is the range of possible forces by the max charge time.
            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

            m_ShellStockData.InitializeCount();
            m_MineStockData.InitializeCount();

            // 武器データを辞書に登録
            weaponStockDictionary.Add("Shell", m_ShellStockData);
            weaponStockDictionary.Add("Mine", m_MineStockData);

            NotifyWeaponStockChanged("Shell");
            NotifyWeaponStockChanged("Mine");
        }


        private void Update()
        {
            //砲弾の数がゼロ以下でないことをチェック
            if (weaponStockDictionary["Shell"].CurrentCount <= 0)
            {
                m_AimSlider.gameObject.SetActive(false); // スライダーを非表示
                return;  // ゲージの更新を止める
            }
            else
            {
                m_AimSlider.gameObject.SetActive(true); // スライダーを表示
            }
            // The slider should have a default value of the minimum launch force.
            m_AimSlider.value = m_MinLaunchForce;

            // If the max force has been exceeded and the shell hasn't yet been launched...
            //if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            //{
            // ... use the max force and launch the shell.
            //m_CurrentLaunchForce = m_MaxLaunchForce;
            //Fire ();
            //}
            // Otherwise, if the fire button has just started being pressed...
            if (Input.GetButtonDown(m_FireButton))
            {
                // ... reset the fired flag and reset the launch force.
                m_Fired = false;
                m_IsCharging = true;
                m_CurrentLaunchForce = m_MinLaunchForce;

                // Change the clip to the charging clip and start it playing.
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play();
            }

            if (m_IsCharging)
            {
                UpdateLaunchForce();  // ゲージ更新
            }

            // Otherwise, if the fire button is being held and the shell hasn't been launched yet...
            //else if (Input.GetButton (m_FireButton) && !m_Fired)
            //{
            // Increment the launch force and update the slider.
            //m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            //m_AimSlider.value = m_CurrentLaunchForce;
            //}
            // Otherwise, if the fire button is released and the shell hasn't been launched yet...
            if (Input.GetButtonUp(m_FireButton) && !m_Fired)
            {
                // ... launch the shell.
                Fire();
                m_IsCharging = false;
            }
            if (Input.GetButtonDown(m_PlaceMineKey))
            {
                if (weaponStockDictionary["Mine"].CurrentCount > 0)
                {
                    PlaceMine();
                }
            }
        }

        private void UpdateLaunchForce()
        {
            if (m_IsIncreasing)
            {
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                if (m_CurrentLaunchForce >= m_MaxLaunchForce)
                {
                    m_CurrentLaunchForce = m_MaxLaunchForce;
                    m_IsIncreasing = false;  // Max→Min
                }
            }
            else
            {
                m_CurrentLaunchForce -= m_ChargeSpeed * Time.deltaTime;
                if (m_CurrentLaunchForce <= m_MinLaunchForce)
                {
                    m_CurrentLaunchForce = m_MinLaunchForce;
                    m_IsIncreasing = true;  // Min→Max
                }
            }

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        private void Fire()
        {
            if (weaponStockDictionary["Shell"].CurrentCount > 0)
            {
                m_Fired = true;
                Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation);
                shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

                m_ShootingAudio.clip = m_FireClip;
                m_ShootingAudio.Play();

                weaponStockDictionary["Shell"].Decrement();
                NotifyWeaponStockChanged("Shell");

                m_CurrentLaunchForce = m_MinLaunchForce;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 衝突した相手のタグが "ShellCartridge" の場合
            if (collision.gameObject.CompareTag("ShellCartridge"))
            {
                RefillAmmo("Shell");
                // カートリッジオブジェクトを破棄
                Destroy(collision.gameObject);
            }
            // 衝突した相手のタグが "MineCartridge" の場合
            else if (collision.gameObject.CompareTag("MineCartridge"))
            {
                RefillAmmo("Mine");
                Destroy(collision.gameObject);
            }
        }

        //砲弾の所持数を増やすメソッド
        public void RefillAmmo(string weaponName)
        {
            if (weaponStockDictionary.TryGetValue(weaponName, out var stockData))
            {
                stockData.Add(stockData.ReplenishCount);
                NotifyWeaponStockChanged(weaponName);
            }
        }

        private void PlaceMine()
        {
            if (weaponStockDictionary["Mine"].CurrentCount > 0)
            {
                Vector3 placePosition = transform.position - transform.forward * 2;
                placePosition.y = 0; // 地面の高さに合わせる
                GameObject mine = Instantiate(m_MinePrefab, placePosition, Quaternion.identity);

                // ドクロマークを設置
                GameObject skullMarker = Instantiate(skullMarkerPrefab, placePosition + new Vector3(0, 1.5f, 0), Quaternion.identity);
                ShellExplosion shellExplosion = mine.GetComponent<ShellExplosion>();
                if (shellExplosion != null)
                {
                    shellExplosion.SetSkullMarker(skullMarker);
                }

                weaponStockDictionary["Mine"].Decrement();
                NotifyWeaponStockChanged("Mine");
                OnMinePlaced?.Invoke(); // 地雷設置イベントを発火
            }
        }

        // ドクロマークを生成するメソッド
        private void PlaceSkullMarker(Vector3 position)
        {
            Vector3 skullPosition = position + new Vector3(0, 1.5f, 0); // 地面の少し上に表示
            Instantiate(skullMarkerPrefab, skullPosition, Quaternion.identity);
        }

        private void NotifyWeaponStockChanged(string weaponName)
        {
            if (OnWeaponStockChanged != null)
            {
                OnWeaponStockChanged?.Invoke(weaponName, weaponStockDictionary[weaponName].CurrentCount);
            }
        }

        public WeaponStockData GetWeaponStockData(string weaponName)
        {
            return weaponStockDictionary[weaponName];
        }
    }
}