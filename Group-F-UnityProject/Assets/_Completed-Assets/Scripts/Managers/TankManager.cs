using System;
using System.Collections;
using UnityEngine;

namespace Complete
{
    [Serializable]
    public class TankManager
    {
        // This class is to manage various settings on a tank.
        // It works with the GameManager class to control how the tanks behave
        // and whether or not players have control of their tank in the 
        // different phases of the game.

        public Color m_PlayerColor;                             // This is the color this tank will be tinted.
        public Transform m_SpawnPoint;                          // The position and direction the tank will have when it spawns.
        public GameObject m_PlayerInfo;                         // 各戦車に対応するUI。
        [HideInInspector] public int m_PlayerNumber;            // This specifies which player this the manager for.
        [HideInInspector] public string m_ColoredPlayerText;    // A string that represents the player with their number colored to match their tank.
        [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.
        private int m_Wins;                                     // 勝利数。
        public int GetWinCount() => m_Wins;

        private TankMovement m_Movement;                        // Reference to tank's movement script, used to disable and enable control.
        private TankShooting m_Shooting;                        // Reference to tank's shooting script, used to disable and enable control.
        private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.
        public event Action<int, string, WeaponStockData> OnWeaponStockChanged; // PlayerNumber, WeaponName, StockDataを通知するイベント
        private float mineDisableDuration = 2.0f;               // 地雷を設置中に動きが止まる時間


        public void Setup()
        {
            // Get references to the components.
            m_Movement = m_Instance.GetComponent<TankMovement>();
            m_Shooting = m_Instance.GetComponent<TankShooting>();
            m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

            // UIを渡す。
            m_Instance.GetComponent<TankHealth>().SetPlayerInfo(m_PlayerInfo);

            // Set the player numbers to be consistent across the scripts.
            m_Movement.m_PlayerNumber = m_PlayerNumber;
            m_Shooting.m_PlayerNumber = m_PlayerNumber;

            // Create a string using the correct color that says 'PLAYER 1' etc based on the tank's color and the player's number.
            m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

            // Change color.
            m_Instance.GetComponent<TankColor>().ChangeColor(m_PlayerColor);

            // 地雷の所持数が変化したイベントを購読
            m_Shooting.OnWeaponStockChanged += HandleWeaponStockChanged;
            // 地雷を設置したイベントを購読
            m_Shooting.OnMinePlaced += HandleMinePlaced;
        }

        // Used during the phases of the game where the player shouldn't be able to control their tank.
        public void DisableControl()
        {
            m_Movement.enabled = false;
            m_Shooting.enabled = false;

            m_CanvasGameObject.SetActive(false);
        }


        // Used during the phases of the game where the player should be able to control their tank.
        public void EnableControl()
        {
            m_Movement.enabled = true;
            m_Shooting.enabled = true;

            m_CanvasGameObject.SetActive(true);
        }


        // Used at the start of each round to put the tank into it's default state.
        public void Reset()
        {
            m_Instance.transform.position = m_SpawnPoint.position;
            m_Instance.transform.rotation = m_SpawnPoint.rotation;

            m_Instance.SetActive(false);
            m_Instance.SetActive(true);
        }


        /// ラウンドが終了したときに呼ばれるメソッド
        /// 
        /// 勝利数をインクリメントしてUIに反映する。
        public void OnRoundEnded(bool win)
        {
            if (win)
            {
                m_Wins += 1;
            }
            m_PlayerInfo.GetComponent<PlayerStockArea>().UpdateWinCount(m_Wins);
        }

        // 武器の所持数が変化したイベントを受け取り、さらにイベントを発生
        private void HandleWeaponStockChanged(string weaponName, int currentCount)
        {
            if (weaponName == "Shell" || weaponName == "Mine")
            {
                var stockData = m_Shooting.GetWeaponStockData(weaponName);
                OnWeaponStockChanged?.Invoke(m_PlayerNumber, weaponName, stockData);
            }
        }

        // 地雷を設置したイベントを受け取り、コルーチンを呼び出す
        private void HandleMinePlaced()
        {
            m_Shooting.StartCoroutine(MinePlacingDisableMovement());
        }

        // 地雷を設置中に動きを止めるコルーチン
        private IEnumerator MinePlacingDisableMovement()
        {
            DisableControl();
            yield return new WaitForSeconds(mineDisableDuration); // 数秒間動きを停止
            EnableControl();
        }
    }
}