using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPowerup : PowerupBehaviour
{
    [SerializeField] private GameObject _powerParticles;

    private void OnEnable()
    {
        TurnManager.Instance.AssignOnTurnOver(ResetParticles);
    }

    private void OnDisable()
    {
        TurnManager.Instance.UnassignOnTurnOver(ResetParticles);
    }

    public override void ProcessPowerup()
    {
        PieceLogic pieceLogic = GameplayManager.Instance.GetPieceLogic(PieceType.Queen);

        _owningPiece.ValidMoves.Clear();
        pieceLogic.GenerateValidMoves(_owningPiece);

        _owningPiece.PossibleMoves.Clear();
        pieceLogic.GeneratePossibleMoves(_owningPiece);

        _powerParticles.SetActive(true);
    }

    private void ResetParticles()
    {
        if (_powerParticles.activeInHierarchy)
        {
            _powerParticles.SetActive(false);
        }
    }
}
