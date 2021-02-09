using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrderController : MonoBehaviour
{
    [SerializeField] private Canvas _customerOrderUI;
    [SerializeField] private Image _propImage;
    [SerializeField] private Sprite _successImage;
    [SerializeField] private Sprite _failureImage;
    [SerializeField] private Image _rageBar;
    [SerializeField] private CustomerMover _customerMover;
    [SerializeField] private int pointsPer5SecondsRemaining = 1;
    [SerializeField] private float distanceToPlayerToShowBubble = 4f;

    private GameObject wantedProps;
    private CustomerOrder _customerOrder;
    private float _waitTime = 0;
    private CustomerController customerComponent;
    private Transform player;
    private bool done;

    public bool isWaiting = false;
    public PropsColor propsColor;
    public PropsType propsType;

    public CustomerMover CustomerMover => _customerMover;

    public static Action<GameObject> OnCustomerOrderComplete;
    public static Action<GameObject> OnCustomerOrderNotComplete;
    public static Action<GameObject> OnCustomerLeaving;
    public static Action<GameObject> OnCustomerGivenWrongProp;

    private void Awake()
    {
        _customerMover.OnCustomerWaiting += OnCustomerWaiting;
        customerComponent = GetComponentInParent<CustomerController>();
    }

    private void Start()
    {
        player = LevelManager.instance.Player.transform;
    }

    private void OnDisable()
    {
        _customerMover.OnCustomerWaiting -= OnCustomerWaiting;
    }

    public void Setup(CustomerOrder customerOrder, GameObject wantedProps)
    {
        this.wantedProps = wantedProps;
        _customerOrder = customerOrder;
        propsType = _customerOrder.PropsType;
        propsColor = _customerOrder.PropsColor;
        _propImage.sprite = _customerOrder.PropImage;
        _propImage.color = SetPropImageColor(propsColor);
    }

    private Color SetPropImageColor(PropsColor propsColor)
    {
        switch (propsColor)
        {
            case PropsColor.Orange:
                return new Color32(255, 165, 0, 255);
            case PropsColor.Purple:
                return Color.magenta;
            case PropsColor.Blue:
                return Color.blue;
            case PropsColor.Green:
                return Color.green;
            default:
                return Color.white;
        }
    }

    private void Update()
    {
        UpdateWaitTime();
    }

    private void UpdateWaitTime()
    {
        if(!done) _customerOrderUI.enabled = Vector3.Distance(transform.parent.position, player.position) < distanceToPlayerToShowBubble;

        if (!isWaiting) { return; }

        _waitTime += Time.deltaTime;

        var rect = _rageBar.rectTransform;

        rect.sizeDelta = new Vector2((float) _waitTime / _customerOrder.OrderTime, 0.25f);

        if (_waitTime > _customerOrder.OrderTime)
        {
            isWaiting = false;
            done = true;
            //_customerOrderUI.enabled = false;
            _propImage.sprite = _failureImage;
            _propImage.color = Color.white;

            OnCustomerOrderNotComplete?.Invoke(gameObject.transform.parent.gameObject);
            StrikeManager.instance.AddStrike();
            OnCustomerLeaving?.Invoke(wantedProps);
        }
    }

    private void OnCustomerWaiting()
    {
        isWaiting = true;
    }

    public bool TryToCompleteOrder(Props props)
    {
        if (propsType == props.GetPropsType() && propsColor == props.GetPropsColor())
        {
            _propImage.sprite = _successImage;
            _propImage.color = Color.white;
            AkSoundEngine.PostEvent("Prop_Success", gameObject);
            OnCustomerOrderComplete?.Invoke(transform.parent.gameObject);
            customerComponent.SnapObjectToHands(props.gameObject);
            int bonus = (Mathf.FloorToInt((_customerOrder.OrderTime - _waitTime) * 0.2f) * pointsPer5SecondsRemaining);
            ScoreManager.instance.ModifyScore(_customerOrder.Score + bonus);
            return true;
        }
        else
        {
            _propImage.sprite = _failureImage;
            _propImage.color = Color.white;

            OnCustomerGivenWrongProp?.Invoke(wantedProps);
            OnCustomerOrderNotComplete?.Invoke(transform.parent.gameObject);
            StrikeManager.instance.AddStrike();
            return false;
        }
    }
}
