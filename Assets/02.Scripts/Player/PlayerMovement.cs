using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public struct PlayerInput : INetworkSerializable
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref up);
            serializer.SerializeValue(ref down);
            serializer.SerializeValue(ref left);
            serializer.SerializeValue(ref right);
        }
    }

    private PlayerInput playerInput;
    private float _speed = 3f;

    public NetworkVariable<FixedString32Bytes> nickName = new NetworkVariable<FixedString32Bytes>();

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        SendNickNameServerRpc($"Player {OwnerClientId}");
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;

        playerInput.up = Input.GetKey(KeyCode.W);
        playerInput.down = Input.GetKey(KeyCode.S);
        playerInput.left = Input.GetKey(KeyCode.A);
        playerInput.right = Input.GetKey(KeyCode.D);

        SendInputServerRpc(playerInput);
    }

    [ServerRpc]
    private void SendInputServerRpc(PlayerInput playerInput)
    {
        Vector2 dir = Vector2.zero;

        if (playerInput.up)
        {
            dir += Vector2.up;
        }
        if (playerInput.down)
        {
            dir += Vector2.down;
        }
        if (playerInput.left)
        {
            dir += Vector2.left;
        }
        if (playerInput.right)
        {
            dir += Vector2.right;
        }

        dir.Normalize();
        transform.Translate(dir * _speed * Time.fixedDeltaTime);
    }

    [ServerRpc]
    private void SendNickNameServerRpc(string nickName)
    {
        this.nickName.Value = nickName;
    }
}
