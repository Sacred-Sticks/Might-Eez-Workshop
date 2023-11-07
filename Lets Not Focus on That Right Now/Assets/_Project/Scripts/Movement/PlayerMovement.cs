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
    [SerializeField] private float deadzone;

    [SerializeField]
    private float slerpRatio;
    private Vector3 rawInputs;

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        rb.velocity = rawInputs * moveSpeed;
        if (!(rawInputs.sqrMagnitude < deadzone * deadzone))
        {
            float angleA = Mathf.Atan2(rawInputs.x, rawInputs.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f,angleA, 0f), slerpRatio);
        }
    }

    private void OnMoveInputChange(Vector2 inputs)
    {
        rawInputs = new Vector3(inputs.x, 0, inputs.y);
    }

    public bool SubscribeToInputs(Player player)
    {
        return playerInputs.SubscribeToInputAction(OnMoveInputChange, player.PlayerID);
    }

    public bool UnsubscribeToInputs(Player player)
    {
        return playerInputs.UnsubscribeToInputAction(OnMoveInputChange,player.PlayerID);
    }
}
