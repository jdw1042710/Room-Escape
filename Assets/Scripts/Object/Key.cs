using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class Key : MonoBehaviour, Interactable
{
    [SerializeField] private KeyData data;

    private static readonly string PickUpSFXAddress = "Assets/SFX/Character/Footstep_Jump.wav";
    private AudioClip _pickUpSFX;

    private AudioSource _audioSource;
    private BoxCollider _collider;

    private GameObject _model;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;

        Addressables.LoadAssetAsync<AudioClip>(PickUpSFXAddress).Completed += handle => _pickUpSFX = handle.Result;

        _collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _model = transform.GetChild(0).gameObject;
    }

    public void Action(GameObject other)
    {
        if (!other.TryGetComponent<InventoryComponent>(out var inventory))
        {
            Debug.Log("컴포넌트 참조 실패");
            return;
        }
        inventory.AddKey(data);
        Destroy(_model);
        _collider.enabled = false;
        StartCoroutine(ShowInteractionMessage());
    }

    public void EnterTrigger(GameObject other)
    {
        UIManager.Instance.ShowTriggerMessage("Pick Up Key [E]");
    }

    public void ExitTrigger(GameObject other)
    {
        UIManager.Instance.HideTriggerMessage();
    }

    private IEnumerator ShowInteractionMessage()
    {   
        _audioSource.PlayOneShot(_pickUpSFX);
        UIManager.Instance.ShowInteractionMessage("Key is acquired");
        yield return new WaitForSeconds(3.0f);
        UIManager.Instance.HideInteractionMessage();
        Destroy(gameObject);
    }
}
