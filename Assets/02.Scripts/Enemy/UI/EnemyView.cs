using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Raycast�� ���� �� ray�� �����ϴ� ��ġ�� ǥ���ϴ� struct ViewCastInfo ����
public struct ViewCastInfo
{
    public bool isHit;
    public Vector3 point;
    public float dst;
    public float angle;
    public RaycastHit hit;

    public ViewCastInfo(bool _isHit, Vector3 _point, float _dst, float _angle, RaycastHit _hit)
    {
        isHit = _isHit;
        point = _point;
        dst = _dst;
        angle = _angle;
        hit = _hit;
    }
}

public class EnemyView : MonoBehaviour
{

    // ����ĳ��Ʈ���� ��ֹ��� ������ ���̾ �����ϴ� ����
    public LayerMask obstacleMask;
    // ����ĳ��Ʈ�� ������ ��ȣ�ۿ��� ������Ʈ
    //public GameObject curInteractGameObject;

    public MeshFilter viewMeshFilter;
    private Mesh _viewMesh;
    private Enemy _enemyInfo;
    private EnemyNav _enemyNav;

    public float meshResolution;
    

    private float _viewRadius;
    private float _viewAngle;


    private bool _isDayTime;

    private void Awake()
    {
        _enemyInfo = GetComponent<Enemy>();
        _enemyNav = GetComponent<EnemyNav>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = _viewMesh;

        //_viewRadius = EnemyManager.Instance.enemyInfo.viewRadius;
        //_viewAngle = EnemyManager.Instance.enemyInfo.viewAngle;

        _viewRadius = _enemyInfo.viewRadius;
        _viewAngle = _enemyInfo.viewAngle;

    }

    void LateUpdate()
    {
        //_isDayTime = EnemyManager.Instance.enemyInfo.isDayTimeMode;
        _isDayTime = _enemyInfo.isDayTimeMode;


        // ������ ���� ǥ��
        if (_isDayTime)
        {
            DrawFieldOfView();
        }
        else 
        {
            _viewMesh.Clear();
        }


    }

    void DrawFieldOfView()
    {       
        int stepCount = Mathf.RoundToInt(_viewAngle * meshResolution);
        float stepAngleSize = _viewAngle / stepCount;
        
        List<Vector3> viewPoints = new List<Vector3>();
        
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - _viewAngle * 0.5f + stepAngleSize * i;

            // ���̾� �������� �ӽ��ڵ� - ���� ����**
            ViewCastInfo newViewCast = ViewCast(angle);
            //viewPoints.Add(newViewCast.point);
            if (!newViewCast.isHit)
            {
                //viewPoints.Add(new Vector3(newViewCast.point.x, transform.position.y, newViewCast.point.z));
                viewPoints.Add(newViewCast.point);
            }

        }
        
        int vertexCount = viewPoints.Count + 1;        
        Vector3[] vertices = new Vector3[vertexCount];        
        int[] triangles = new int[(vertexCount - 2) * 3];        
        vertices[0] = Vector3.zero;        
              
        for (int i = 0; i < vertexCount - 1; i++)
        {            
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {                
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        // ���� �׷��� ��޽����ִٸ� ����
        _viewMesh.Clear();
        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }


    // raycast ����� ViewCastInfo�� ��ȯ
    public ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        int craft = LayerMask.NameToLayer("Craft");
        int player = LayerMask.NameToLayer("Player"); ;
               

        // ��ȣ �ۿ��� �ŰԺ����� ���̾�� �߰��ѵ� �ش� ���̾��� ������ ViewCastInfo�� ������ �� �ִ�
        if (Physics.Raycast(transform.position, dir, out hit, _viewRadius, obstacleMask))
        {
            Debug.Log($"EnemyNav.cs - ViewCast() - ���� ����, {hit.collider.gameObject.layer}");
            //Debug.Log($"EnemyNav.cs - ViewCast() - ������� ��ġ, {hit.point}");
            _enemyNav.curViewCastInfo = new ViewCastInfo(true, hit.point, _viewRadius, globalAngle, hit);

            // ���ع� ����
            if (craft == hit.collider.gameObject.layer && !_enemyNav.isChase)
            {
                Debug.Log("EnemyNav.cs - ViewCast() - ��ֹ� �߰�");
                //_enemyNav.curInteractGameObject = hit.collider.gameObject;

                //_enemyNav.curHit = hit;
                _enemyNav.aiState = AIState.Chasing;
                _enemyNav.isChase = true;
                
            }
            // �÷��̾� ���� - ���̾� ���� �� �߰��Ұ�**
            if (player == hit.collider.gameObject.layer && !_enemyNav.isChase)
            { 
                Debug.Log("EnemyNav.cs - ViewCast() - �÷��̾� �߰�");
                _enemyNav.aiState = AIState.Chasing;
                _enemyNav.isChase = true;
               
            }
        }
       
        return new ViewCastInfo(false, transform.position + dir, _viewRadius, globalAngle, hit);

    }

    // �þ߰����� ��ǥȹ��
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angleInDegrees) * _viewRadius, 0, Mathf.Cos(Mathf.Deg2Rad * angleInDegrees) * _viewRadius);
    }




}
