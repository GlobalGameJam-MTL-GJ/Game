using System;
using System.Linq;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private float m_PickupDistance = 50f;

    [SerializeField]
    private float m_PickupSphereRadius = 3f;

    [SerializeField] private float m_CustomerDetectionRadius;
    [SerializeField] private float delayBeforeSubmittingAgain = 1f;
    private float currentTimerBeforeEnablingSubmission;
    private bool isOnSubmissionDelay;
    [SerializeField]
    private Vector3 m_Offset;

    [SerializeField] private float m_throwForce = 8f;
    public bool CanThrowOrDrop = true;
    private bool m_IsEquipped;
    private GameObject pickedUpObject;
    public Transform pickUpContainer;
    
    [SerializeField]
    private LayerMask m_PickupLayer;

    [SerializeField]
    private LayerMask m_CustomerLayer;

    private RaycastHit[] m_SphereCastHits;

    private GameObject m_PickableItem;
    private PlayerInputs playerInputs;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerInputs = GetComponent<PlayerInputs>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {

        if (isOnSubmissionDelay)
        {
            currentTimerBeforeEnablingSubmission -= Time.deltaTime;
            if (currentTimerBeforeEnablingSubmission <= 0)
                isOnSubmissionDelay = false;
        }

        if (playerInputs.CheckIfThrowButtonPressed())
        {
            if(CanThrowOrDrop)
                Throw();
        }
        if (playerInputs.CheckIfMainActionButtonPressed())
        {
            if (!m_IsEquipped)
            {
                m_SphereCastHits = Physics.SphereCastAll(transform.TransformPoint(Vector3.forward) + m_Offset,
                    m_PickupSphereRadius, transform.forward, 0, m_PickupLayer);
                if (m_SphereCastHits.Length > 0)
                {
                    if (m_SphereCastHits.Length != 1) //si on a seulement 1 item proche, pas besoin de filter selon la distance
                    {
                        m_SphereCastHits = m_SphereCastHits.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToArray(); //if performance issues, we could optimize this
                    }
                    m_PickableItem = m_SphereCastHits[0].collider.gameObject;
                    Pickup(m_PickableItem);
                }
            }
            else
            {
                var customerColliders = Physics.OverlapSphere(transform.position, m_CustomerDetectionRadius, m_CustomerLayer, QueryTriggerInteraction.Collide);
                if (customerColliders.Length > 0)
                {
                    if (isOnSubmissionDelay) return;
                    var customerOrderController = customerColliders[0].GetComponent<CustomerController>().CustomerOrderController;
                    Props props = pickedUpObject.GetComponent<Props>();
                    if(props != null)
                    {
                        isOnSubmissionDelay = true;
                        currentTimerBeforeEnablingSubmission = delayBeforeSubmittingAgain;
                        if (customerOrderController.TryToCompleteOrder(props))
                        {
                            PropsBuilder.instance.RemoveAnActiveProps(pickedUpObject);
                            m_IsEquipped = false;
                            pickedUpObject = null;
                        }
                    }
                }
                else
                {
                    if(CanThrowOrDrop)
                        DropTheProps();
                }
            }
        }
    }

    public void Pickup(GameObject itemObject)
    {
        Props props = itemObject.GetComponent<Props>();
        AkSoundEngine.SetSwitch("PropType", props.GetPropsType().ToString(), gameObject);
        AkSoundEngine.PostEvent("Player_Pickup", gameObject);
        if (props != null)
        {
            props.GetPickedUp();
            playerMovement.SetSpeedModifier(props.SpeedModifier);
        }

        itemObject.transform.parent = pickUpContainer;
        itemObject.transform.localPosition = new Vector3(0, 1f, 0.4f);
        m_IsEquipped = true;
        pickedUpObject = itemObject;
    }

    private void Throw()
    {
        AkSoundEngine.PostEvent("Player_Throw", gameObject);
        LetGoOfTheItem();
        pickedUpObject.GetComponent<Props>().GetThrown(transform.forward, m_throwForce);
        pickedUpObject = null;
    }
    
    private void DropTheProps()
    {
        AkSoundEngine.PostEvent("Player_Drop", gameObject);
        LetGoOfTheItem();
        pickedUpObject.GetComponent<Props>().GetDropped();
        
        pickedUpObject = null;
    }

    private void LetGoOfTheItem()
    {
        if (pickedUpObject != null)
        {
            playerMovement.SetSpeedModifier(0);
            pickedUpObject.transform.parent = null;
        }
        pickedUpObject.transform.parent = null;
        m_IsEquipped = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.forward) + m_Offset, m_PickupSphereRadius);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.forward) + m_Offset, m_CustomerDetectionRadius);
    }
}