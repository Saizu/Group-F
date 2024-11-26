using UnityEngine;
using Complete;

namespace Complete{
    public class HudManager : MonoBehaviour{
        [SerializeField] private PlayerStockArea player1StockArea;
        [SerializeField] private PlayerStockArea player2StockArea;
        //GameManagerオブジェクトへの参照を保持するための変数
        [SerializeField] private GameManager gameManager;

        private void OnEnable(){
            if (gameManager != null){
                gameManager.GameStateChanged += HandleGameStateChanged;

                foreach (var tank in gameManager.m_Tanks){
                    tank.AmmoStockChanged+= HandleWeaponStockChanged;
                }
            }
        }

        private void OnDisable(){
            if (gameManager != null){
                gameManager.GameStateChanged -= HandleGameStateChanged;

                foreach (var tank in gameManager.m_Tanks){
                    tank.AmmoStockChanged -= HandleWeaponStockChanged;
                }
            }
        }

        private void HandleGameStateChanged(GameManager.GameState newState){
            bool isGameActive = (newState == GameManager.GameState.RoundPlaying);
            //HUDの表示・非表示を更新
            player1StockArea.gameObject.SetActive(isGameActive);
            player2StockArea.gameObject.SetActive(isGameActive);
        }

        private void HandleWeaponStockChanged(int playerNumber, int currentStock){
            if (playerNumber == 1){
                player1StockArea.UpdatePlayerStockArea(currentStock);
            }
            else if (playerNumber == 2){
                player2StockArea.UpdatePlayerStockArea(currentStock);
            }
        }
    }
}
