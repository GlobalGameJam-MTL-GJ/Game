using System.Linq;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private float m_PickupDistance = 50f;

    [SerializeField]
    private float m_PickupSphereRadius = 3f;

    [SerializeField] private float m_CustomerDetectionRadius = 2f;
    
    [SerializeField]
    private Vector3 m_Offset;

    [SerializeField] private float m_throwForce = 8f;

    private bool m_IsEquipped;
    private GameObject pickedUpObject;
    public Transform pickUpContainer;
    
    [SerializeField]
    private LayerMask m_PickupLayer;

    private RaycastHit[] m_SphereCastHits;

    private GameObject m_PickableItem;
    
    private void Update()
    {
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
                var customerColliders = Physics.OverlapSphere(transform.position, m_CustomerDetectionRadius, 20, QueryTriggerInteraction.Collide);
                if (customerColliders.Length > 0)
                {
                    var customerController = customerColliders[0].GetComponent<CustomerController>();
                    Props props = pickedUpObject.GetComponent<Props>();
                    // if (props != null && customerController.TryToCompleteOrder(props))
                    // {
                    //     //we gave the right item
                    // }
                    // else
                    // {
                    //     //we gave the wrong item
                    // }
                }
                else
                {
                    Drop();
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

    public void Drop()
    {
        pickedUpObject.transform.parent = null;
        m_IsEquipped = false;
        pickedUpObject.GetComponent<Props>().GetThrown(transform.forward, m_throwForce);
        pickedUpObject = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.forward) + m_Offset, m_PickupSphereRadius);
    }
}