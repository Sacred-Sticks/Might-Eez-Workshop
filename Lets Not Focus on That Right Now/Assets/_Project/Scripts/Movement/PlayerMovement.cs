using System.Collections;
using System.Collections.Generic;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;
using UnityEngine.Scripting;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour, IInputReceiver
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector2Input playerInputs;
    private Vector3 rawInputs;

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        rb.velocity = rawInputs * moveSpeed;
    }

    private void OnMoveInputChange(Vector2 inputs)
    {
        rawInputs = new Vector3(inputs.x, 0, inputs.y);
    }

    public void SubscribeToInputs(Player player)
    {
        playerInputs.SubscribeToInputAction(OnMoveInputChange, player.PlayerID);
    }

    public void UnsubscribeToInputs(Player player)
    {
        playerInputs.UnsubscribeToInputAction(OnMoveInputChange,player.PlayerID);
    }
}
