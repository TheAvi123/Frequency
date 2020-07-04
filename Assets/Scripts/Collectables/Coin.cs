﻿using UnityEngine;

public class Coin : MonoBehaviour
{
    //Reference Variables
    [SerializeField] ParticleSystem coinCollectVFX = null;

    //Internal Methods
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            CoinCollected();
        }
    }

    private void CoinCollected() {
        CoinManager.sharedInstance.CollectCoin();
        SpawnCollectVFX();
        ShakeCamera();
        SendBackToPool();
    }

    private void SpawnCollectVFX() {
        ParticleSystem coinVFX = Instantiate(coinCollectVFX, transform.position, transform.rotation) as ParticleSystem;
        Destroy(coinVFX, 1f);
    }

    private void ShakeCamera() {
        CameraShaker.sharedInstance.AddCameraShake(0.1f);
    }

    private void SendBackToPool() {
        //Coin Pool
        gameObject.SetActive(false);
    }
}