using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;

namespace Complete
{
    public class HudManager : MonoBehaviour
    {
        [SerializeField] private PlayerStockArea player1StockArea; // Player1のストック表示
        [SerializeField] private PlayerStockArea player2StockArea; // Player2のストック表示
        [SerializeField] private GameObject hudCanvas;            // HUD全体を管理するCanvas
        [SerializeField] private GameManager gameManager;         // GameManagerへの参照

        private void OnEnable()
        {
            gameManager.GameStateChanged += HandleGameStateChanged;

            // GameManagerのGameStateChangedイベントを購読
            gameManager.GameStateChanged += HandleGameStateChanged;

            // 各TankManagerのOnWeaponStockChangedイベントを購読
            foreach (var tank in gameManager.m_Tanks)
            {
                tank.OnWeaponStockChanged += HandleWeaponStockChanged;
            }
        }

        private void OnDisable()
        {
            // イベントの購読解除
            gameManager.GameStateChanged -= HandleGameStateChanged;

            // 各TankManagerのOnWeaponStockChangedイベントの購読解除
            foreach (var tank in gameManager.m_Tanks)
            {
                tank.OnWeaponStockChanged -= HandleWeaponStockChanged;
            }
        }

        private void HandleGameStateChanged(GameManager.GameState newGameState)
        {
            // ゲームのプレイ中のみHUDを表示、それ以外は非表示
            hudCanvas.SetActive(newGameState == GameManager.GameState.RoundPlaying || newGameState == GameManager.GameState.RoundStarting);
        }

        private void HandleWeaponStockChanged(int playerNumber, string weaponName, WeaponStockData stockData)
        {
            // 対応するPlayerStockAreaを取得
            PlayerStockArea targetStockArea = playerNumber == 1 ? player1StockArea : player2StockArea;

            // 更新するデータを辞書形式で作成
            var weaponStockDictionary = new Dictionary<string, WeaponStockData>
            {
                { weaponName, stockData }
            };
        
            // PlayerStockAreaのUIを更新
            targetStockArea.UpdatePlayerStockArea(weaponStockDictionary);
        }
    }
}