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

    private void Update()
    {

        if (isOnSubmissionDelay)
        {
            currentTimerBeforeEnablingSubmission -= Time.deltaTime;
            if (currentTimerBeforeEnablingSubmission <= 0)
                isOnSubmissionDelay = false;
        }
        
        if (Input.GetKeyDown(KeyCode.E))
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
                            //give the item to the customers ?
                        }
                        else
                        {
                            //you gave THE WRONG ITEM to the customer TODO : ADD CONSEQUENCES
                            //the customer must leave
                            //the customer that left must talk to the propsbuilder and tell it what item will not be taken
                            //the player keep the item in his hands
                            //have a little delay so you cant spam the submit item action
                        }
                    }
                }
                else
                {
                    if(CanThrowOrDrop)
                        Throw();
                }
            }
        }
    }
    
    public void Pickup(GameObject itemObject)
    {
        Props props = itemObject.GetComponent<Props>();
        if (props != null)
        {
            props.GetPickedUp();
        }

        itemObject.transform.parent = pickUpContainer;
        itemObject.transform.localPosition = new Vector3(0, 1f, 0.4f);
        m_IsEquipped = true;
        pickedUpObject = itemObject;
    }

    public void Throw()
    {
        LetGoOfTheItem();
        pickedUpObject.GetComponent<Props>().GetThrown(transform.forward, m_throwForce);
        pickedUpObject = null;
    }

    private void LetGoOfTheItem()
    {
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