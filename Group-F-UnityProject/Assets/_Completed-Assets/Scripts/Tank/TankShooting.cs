using UnityEngine;
using UnityEngine.UI;
using System;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
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

        public int m_InitialAmmo = 10; //ゲーム開始時の砲弾の所持数
        private int m_CurrentAmmo; //現在の砲弾の所持数
        public int m_MaxAmmo = 50; //所持可能な砲弾の最大数
        public int m_CartridgeRefillCount = 10; //砲弾カートリッジを取得したときに補充する数

        private string m_FireButton;                // The input axis that is used for launching shells.
        private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        private bool m_Fired;                       // Whether or not the shell has been launched with this button press.

        private bool m_IsCharging;
        private bool m_IsIncreasing = true;

       //砲弾の所持数が変化したときに通知するイベント
        public event Action<int> OnShellStockChanged;
        private void OnEnable()
        {
            // When the tank is turned on, reset the launch force and the UI
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_AimSlider.value = m_MinLaunchForce;
        }


        private void Start ()
        {
            // The fire axis is based on the player number.
            m_FireButton = "Fire" + m_PlayerNumber;

            // The rate that the launch force charges up is the range of possible forces by the max charge time.
            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
            //砲弾の所持数を初期化
            m_CurrentAmmo = m_InitialAmmo;
            // 初期状態の砲弾所持数を通知
            OnShellStockChanged?.Invoke(m_CurrentAmmo);
        }


        private void Update ()
        {
            //砲弾の数がゼロ以下でないことをチェック
            if (m_CurrentAmmo <= 0){
                m_AimSlider.gameObject.SetActive(false); // スライダーを非表示
                return;  // ゲージの更新を止める
            }else {
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
            if (Input.GetButtonDown (m_FireButton))
            {
                // ... reset the fired flag and reset the launch force.
                m_Fired = false;
                m_IsCharging = true;
                m_CurrentLaunchForce = m_MinLaunchForce;

                // Change the clip to the charging clip and start it playing.
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play ();
            }

            if (m_IsCharging) {
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
            if (Input.GetButtonUp (m_FireButton) && !m_Fired)
            {
                // ... launch the shell.
                Fire ();
                m_IsCharging = false;
            }
        }

        private void UpdateLaunchForce(){
            if (m_IsIncreasing){
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                if(m_CurrentLaunchForce >= m_MaxLaunchForce) {
                    m_CurrentLaunchForce = m_MaxLaunchForce;
                    m_IsIncreasing = false;  // Max→Min
                }
            }
            else {
                m_CurrentLaunchForce -= m_ChargeSpeed * Time.deltaTime;
                if (m_CurrentLaunchForce <= m_MinLaunchForce) {
                    m_CurrentLaunchForce = m_MinLaunchForce;
                    m_IsIncreasing = true;  // Min→Max
                }
            }

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        private void Fire ()
        {
            // Set the fired flag so only Fire is only called once.
            m_Fired = true;

            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; 

            // Change the clip to the firing clip and play it.
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play ();

            // Reset the launch force.  This is a precaution in case of missing button events.
            m_CurrentLaunchForce = m_MinLaunchForce;

            //砲弾の消費
            m_CurrentAmmo--;
            OnShellStockChanged?.Invoke(m_CurrentAmmo); // 砲弾消費時にイベントを発生
        }

        private void OnCollisionEnter(Collision collision){
        // 衝突した相手のタグが "ShellCartridge" の場合
          if (collision.gameObject.CompareTag("ShellCartridge")){
            RefillAmmo();
        // カートリッジオブジェクトを破棄
            Destroy(collision.gameObject);
          }
        }

        //砲弾の所持数を増やすメソッド
        public void RefillAmmo(){
            // 最大所持数を超えないように砲弾を補充
            m_CurrentAmmo = Mathf.Min(m_CurrentAmmo + m_CartridgeRefillCount, m_MaxAmmo);
            OnShellStockChanged?.Invoke(m_CurrentAmmo); // 補充時にイベントを発生
        }
    }
}