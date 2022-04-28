using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    private float _powerupTurnChangeWait = 2f;

    private bool _processingPawnPromotion;

    #region Singleton Pattern

    private static PowerupManager _instance;

    public static PowerupManager Instance => _instance;

    #endregion

    public bool ProcessingPawnPromotion
    {
        set => _processingPawnPromotion = value;
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _powerupTurnChangeWait = SceneParamsHolder.Instance.PowerupTurnChangeWait;
    }

    public void ProcessPowerup(int index)
    {
        if (!_processingPawnPromotion)
        {
            switch (index)
            {
                case 0:
                    if (PowerupSelectionIsValid(PieceType.King, 0))
                    {
                        PieceBehaviour piece = PieceManager.Instance.CurrentSelection;
                        piece.PowerupBehaviour.ProcessPowerup();
                    }
                    break;

                case 1:
                    if (PowerupSelectionIsValid(PieceType.Rook, 1))
                    {
                        PieceBehaviour piece = PieceManager.Instance.CurrentSelection;
                        piece.PowerupBehaviour.ProcessPowerup();
                        StartCoroutine(ChangeTurnOnPowerupUse());
                    }
                    break;

                case 2:
                    if (PowerupSelectionIsValid(PieceType.Bishop, 2))
                    {
                        PieceBehaviour piece = PieceManager.Instance.CurrentSelection;
                        piece.PowerupBehaviour.ProcessPowerup();
                        StartCoroutine(ChangeTurnOnPowerupUse());
                    }
                    break;

                case 3:
                case 4:
                case 5:
                case 6:
                    if (PowerupSelectionIsValid(PieceType.Pawn, index))
                    {
                        PieceBehaviour piece = PieceManager.Instance.CurrentSelection;
                        GameplayManager.Instance.PromotePawn(piece, index - 2);
                        StartCoroutine(ChangeTurnOnPowerupUse());
                    }
                    break;
            }
        }
    }

    private bool PowerupSelectionIsValid(PieceType pieceType, int index)
    {
        PieceBehaviour piece = PieceManager.Instance.CurrentSelection;
        if (piece && piece.PieceData.PieceType == pieceType)
        {
            if (EconomyManager.Instance.ProcessCost(index))
            {
                UIManager.Instance.UpdateCost();
                return true;
            }
        }

        return false;
    }

    private IEnumerator ChangeTurnOnPowerupUse()
    {
        PieceManager.Instance.UnselectPiece();
        PieceManager.Instance.SetPossibleMoves();
        yield return TurnManager.Instance.EndTurnRoutine(_powerupTurnChangeWait);
    }
}