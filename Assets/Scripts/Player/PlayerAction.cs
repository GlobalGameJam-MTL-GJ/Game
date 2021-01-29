using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private float m_PickupDistance = 50f;

    [SerializeField]
    private float m_PickupSphereRadius = 3f;

    [SerializeField]
    private Vector3 m_Offset;

    private GameObject pickObject;
    private GameObject m_ObjectHeldInHand;

    private bool m_IsEquipped;

    public Transform pickUpContainer;

    private PickableItem m_PickableItem;

    [SerializeField]
    private LayerMask m_PickupLayer;

    // Start is called before the first frame update
    private void Start()
    {
        //pickUpContainer = GameObject.FindGameObjectWithTag("PickUp").transform;
        m_PickableItem = GameObject.FindGameObjectWithTag("PickUp").GetComponent<PickableItem>();
    }

    // Update is called once per frame
    private void Update()
    {
        RaycastHit hit;
        
        if (Physics.SphereCast(transform.position, m_PickupSphereRadius, transform.TransformPoint(Vector3.forward), out hit, 1f, m_PickupLayer))
        {
            pickObject = hit.collider.gameObject;
        }
        else
        {
            pickObject = null;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickObject != null)
            {
                Pickup(pickObject);
            }
            else
            {
                //Drop(pickObject);                
            }
        }
        
    }


/*
public void PickUpItem(PickableItem item)
{
    m_IsEquipped = true;
    Debug.Log("Pickable found");
    m_PickableItem = item;

    item.Rb.isKinematic = true;
    item.Rb.velocity = Vector3.zero;
    item.Rb.angularVelocity = Vector3.zero;

    item.transform.SetParent(pickUpContainer);
}*/

public void Pickup(GameObject itemObject)
{
    
    if (itemObject.GetComponent<Rigidbody>())
    {
        m_IsEquipped = true;
        Rigidbody objRb = itemObject.GetComponent<Rigidbody>();
        objRb.isKinematic = true;
        objRb.velocity = Vector3.zero;
        objRb.angularVelocity = Vector3.zero;

        objRb.transform.parent = pickUpContainer;
        //can be result of a funny bug need to correct it later
        objRb.transform.localPosition = new Vector3(0f, 1f, 0.4f);

            //m_ObjectHeldInHand = itemObject;
            
        //objRb.transform.localPosition = Vector3.up;
        //objRb.transform.localEulerAngles = Vector3.zero;
    }
}

public void Drop(GameObject drop)
{
        
        drop.transform.parent = null;
        drop.GetComponent<Rigidbody>().isKinematic = false;
        drop.GetComponent<Rigidbody>().AddForce(m_ObjectHeldInHand.transform.forward * 2, ForceMode.VelocityChange);
        
}

public void Throw()
{
}

private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.TransformPoint(Vector3.forward) + m_Offset, m_PickupSphereRadius);
}
}