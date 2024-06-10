using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Raycast를 했을 때 ray가 도달하는 위치를 표현하는 struct ViewCastInfo 생성
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

    // 레이캐스트에서 장애물로 간주할 레이어를 지정하는 변수
    public LayerMask obstacleMask;
    // 레이캐스트에 감지된 상호작용할 오브젝트
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


        // 낮에만 범위 표시
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

            // 레이어 생성전의 임시코드 - 추후 수정**
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

        // 전에 그려진 뷰메쉬가있다면 리셋
        _viewMesh.Clear();
        _viewMesh.vertices = vertices;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }


    // raycast 결과를 ViewCastInfo로 반환
    public ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        int craft = LayerMask.NameToLayer("Craft");
        int player = LayerMask.NameToLayer("Player"); ;
               

        // 상호 작용할 매게변수를 레이어로 추가한뒤 해당 레이어의 정보를 ViewCastInfo로 전달할 수 있다
        if (Physics.Raycast(transform.position, dir, out hit, _viewRadius, obstacleMask))
        {
            Debug.Log($"EnemyNav.cs - ViewCast() - 감지 성공, {hit.collider.gameObject.layer}");
            //Debug.Log($"EnemyNav.cs - ViewCast() - 감지대상 위치, {hit.point}");
            _enemyNav.curViewCastInfo = new ViewCastInfo(true, hit.point, _viewRadius, globalAngle, hit);

            // 방해물 감지
            if (craft == hit.collider.gameObject.layer && !_enemyNav.isChase)
            {
                Debug.Log("EnemyNav.cs - ViewCast() - 장애물 발견");
                //_enemyNav.curInteractGameObject = hit.collider.gameObject;

                //_enemyNav.curHit = hit;
                _enemyNav.aiState = AIState.Chasing;
                _enemyNav.isChase = true;
                
            }
            // 플레이어 감지 - 레이어 설정 후 추가할것**
            if (player == hit.collider.gameObject.layer && !_enemyNav.isChase)
            { 
                Debug.Log("EnemyNav.cs - ViewCast() - 플레이어 발견");
                _enemyNav.aiState = AIState.Chasing;
                _enemyNav.isChase = true;
               
            }
        }
       
        return new ViewCastInfo(false, transform.position + dir, _viewRadius, globalAngle, hit);

    }

    // 시야각으로 좌표획득
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angleInDegrees) * _viewRadius, 0, Mathf.Cos(Mathf.Deg2Rad * angleInDegrees) * _viewRadius);
    }




}
