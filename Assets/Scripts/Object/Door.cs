using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour, Interactable
{
    [SerializeField] private bool isLocked = false;
    [SerializeField] private KeyData key = null;

    [SerializeField] private string unlockFailMessage = "Default";
    [SerializeField] private string unlockSuccessMessage  = "Default";

    private static readonly string LockedSFXAddress = "Assets/SFX/Object/Door_Locked.wav";
    private AudioClip _lockedSFX;
    private static readonly string OpenCloseSFXAddress = "Assets/SFX/Object/Door_Open.wav";
    private AudioClip _openCloseSFX;
    
    private Animator _animator;
    private AudioSource _audioSource;
    
    private bool _isOpened = false;
    private bool _isOpening = false;
    
    private static readonly int IsOpened = Animator.StringToHash("isOpened");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;

        Addressables.LoadAssetAsync<AudioClip>(LockedSFXAddress).Completed += handle => { _lockedSFX = handle.Result; };
        Addressables.LoadAssetAsync<AudioClip>(OpenCloseSFXAddress).Completed += handle => { _openCloseSFX = handle.Result; };
    }

    public void Action(GameObject other)
    {
        if (!isLocked)
        {
            TryOpen();
            return;
        }
        
        if (key)
        {
            if (!other.TryGetComponent<InventoryComponent>(out var inventory)) return;
            var hasKey = inventory.HasKey(key);
            inventory.UseKey(key);
            StartCoroutine(hasKey ? UnLock() : Locked());
            return;
        }
        // 잠긴 상태이지만 키가 필요하지 않다면 반대편에서 열어야 함.
        var isFront = Vector3.Angle(other.transform.forward, transform.right) < 90;
        StartCoroutine(isFront ? Locked() : UnLock());
    }
    

    public void EnterTrigger(GameObject other)
    {
        UIManager.Instance.ShowTriggerMessage("Open/Close [E]");
    }

    public void ExitTrigger(GameObject other)
    {
        UIManager.Instance.HideTriggerMessage();
    }

    protected virtual void TryOpen()
    {
        if(_isOpening) return;
        StartCoroutine(_isOpened ? Close() : Open());
    }
    
    private IEnumerator Open()
    {
        _isOpening = true;

        _isOpened = true;
        _audioSource.PlayOneShot(_openCloseSFX);
        _animator.SetBool(IsOpened, _isOpened);
        
        
        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) yield return null;

        _isOpening = false;
    }
    
    private IEnumerator Close()
    {
        _isOpening = true;

        
        _isOpened = false;
        _audioSource.PlayOneShot(_openCloseSFX);
        _animator.SetBool(IsOpened, _isOpened);
        
        
        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) yield return null;
        // yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        _isOpening = false;
    }

    private IEnumerator UnLock()
    {
        isLocked = false;
        TryOpen();
        UIManager.Instance.ShowInteractionMessage(unlockSuccessMessage);
        yield return new WaitForSeconds(3.0f);
        UIManager.Instance.HideInteractionMessage();
    }

    private IEnumerator Locked()
    {
        _audioSource.PlayOneShot(_lockedSFX);
        UIManager.Instance.ShowInteractionMessage(unlockFailMessage);
        yield return new WaitForSeconds(3.0f);
        UIManager.Instance.HideInteractionMessage();
    }
}
