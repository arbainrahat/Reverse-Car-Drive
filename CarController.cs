using UnityEngine;


public class CarController : MonoBehaviour
{
    public Vector3 Direction;
    public float runSpeed;
    public float rotateSpeed;
    public Rigidbody _rigidBody;
    public GameObject []parkPathches;
 //   public GameConstants _GameConstants;
    public CameraFollowScripts _CameraFollowScripts;

    public Transform targetPosRight;
    public Transform targetPosLeft;
    public Transform spwanPosition;

    private bool rotate;
    private bool canGo;
    private int curntPatch;
    private int curntP_Point;
    private ParkingPoints parkPnts;
    private Transform target;
    GameObject _gameObject;

    private void Start()
    {
        //Debug.Log("Start");
        parkPathches = GameObject.FindGameObjectsWithTag("Patch");
        _CameraFollowScripts = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollowScripts>();

        curntPatch = GameConstants._instnace.currentPatch;
        curntP_Point = GameConstants._instnace.currentPPoint;
        parkPnts = parkPathches[curntPatch].GetComponent<ParkingPoints>();
    }

    private void FixedUpdate()
    {
        if (!canGo)
        {
            Vector3 direction = transform.TransformDirection(Direction);
            _rigidBody.velocity = direction * runSpeed;
        }
       

        if (rotate)
        {
            //Debug.Log("Rotate_GameValue = " + rotate);
            // curntP_Point = GameConstants._instnace.currentPPoint;
            if (parkPnts.parkingPoints[curntP_Point].CompareTag("LeftPark"))
            {
                target = targetPosLeft;
                targetPosLeft.parent = null;
            }
            else
            {
                target = targetPosRight;
                targetPosRight.parent = null;
            }

            transform.localPosition = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 1.7f);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0f,180f,0f,0f), Time.deltaTime * rotateSpeed);
          
           // Debug.Log("Rotate");
        }
        
    }

    public void Car_Rotate()
    {
        GameConstants._instnace.currentPPoint++;
        spwanPosition.parent = null;
        rotate = true;
        _CameraFollowScripts.Target = null;
        canGo = true;
        // Debug.Log("Rotate"+rotate);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
           
           // canGo = true;
            //Debug.Log("Collide with Wall");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftPark") || other.CompareTag("RightPark"))
        {
            Debug.Log("On_Parking");
            //int rand = Random.Range(0,2);
            //GameObject gm = GameConstants._instnace.cars[rand];
            //_gameObject  =  Instantiate(gm, spwanPosition.position, Quaternion.identity);
            Invoke("Delay",2.0f);
            //Destroy(this.gameObject,3.0f);
            //_CameraFollowScripts.Target = _gameObject.transform;
            //GameConstants._instnace.screenTap.carController = _gameObject.GetComponent<CarController>();
        }
    }

    private void Delay()
    {
        int rand = Random.Range(0, 2);
        GameObject gm = GameConstants._instnace.cars[rand];
        _gameObject = Instantiate(gm, spwanPosition.position, Quaternion.identity);
        GameConstants._instnace.screenTap.carController = _gameObject.GetComponent<CarController>();
        Destroy(this.gameObject,3f);
        _CameraFollowScripts.Target = _gameObject.transform;
    }
}
