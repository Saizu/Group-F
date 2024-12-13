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
            // GameManagerのGameStateChangedイベントを購読
            gameManager.GameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            // イベントの購読解除
            gameManager.GameStateChanged -= HandleGameStateChanged;
        }

        private void HandleGameStateChanged(GameManager.GameState newGameState)
        {
            // ゲームのプレイ中のみHUDを表示、それ以外は非表示
            hudCanvas.SetActive(newGameState == GameManager.GameState.RoundPlaying);
        }
    }
}