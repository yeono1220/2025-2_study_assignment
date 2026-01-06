using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    UIManager MyUIManager;

    public GameObject BallPrefab;   // prefab of Ball

    // Constants for SetupBalls
    public static Vector3 StartPosition = new Vector3(0, 0, -6.35f);
    public static Quaternion StartRotation = Quaternion.Euler(0, 90, 90);
    const float BallRadius = 0.286f;
    const float RowSpacing = 0.02f;

    GameObject PlayerBall;
    GameObject CamObj;

    const float CamSpeed = 3f;

    const float MinPower = 15f;
    const float PowerCoef = 1f;

    void Awake()
    {
        // PlayerBall, CamObj, MyUIManager를 얻어온다.
        // ---------- TODO ---------- 
        PlayerBall = GameObject.Find("PlayerBall");
        CamObj = GameObject.Find("Main Camera");
        MyUIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        // -------------------- 
    }

    void Start()
    {
        SetupBalls();
    }

    // Update is called once per frame
    void Update()
    {
        // 좌클릭시 raycast하여 클릭 위치로 ShootBallTo 한다.
        // ---------- TODO ---------- 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ShootBallTo(hit.point);
            }
        }
        // -------------------- 
    }

    void LateUpdate()
    {
        CamMove();
    }

    void SetupBalls()
    {
        // 15개의 공을 삼각형 형태로 배치한다.
        // 가장 앞쪽 공의 위치는 StartPosition이며, 공의 Rotation은 StartRotation이다.
        // 각 공은 RowSpacing만큼의 간격을 가진다.
        // 각 공의 이름은 {index}이며, 아래 함수로 index에 맞는 Material을 적용시킨다.
        // Obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ball_1");
        // ---------- TODO ---------- 
        int ballIndex = 1;
        // 공의 지름 + 간격
        float spacing = (BallRadius * 2) + RowSpacing; 
        
        // 5줄 (1개, 2개, 3개, 4개, 5개)
        for (int row = 0; row < 5; row++)
        {
            // Z축 오프셋: 뒤로 갈수록(row가 증가할수록) Z값은 감소 (또는 증가 방향에 따라 다름)
            // StartPosition(-6.35)이 가장 앞쪽(플레이어와 가까운 쪽 아님, 삼각형의 꼭지점)이라고 가정
            // 플레이어(6.35) -> 꼭지점(-6.35) 방향으로 샷을 하므로, 나머지 공들은 꼭지점보다 더 뒤(-Z 방향)에 있어야 함.
            float zOffset = row * (Mathf.Sqrt(3) * spacing / 2);

            for (int col = 0; col <= row; col++)
            {
                // X축 오프셋: 해당 줄의 중심을 기준으로 배치
                float xOffset = (col - (row / 2.0f)) * spacing;

                Vector3 spawnPos = new Vector3(StartPosition.x + xOffset, StartPosition.y, StartPosition.z - zOffset);
                
                GameObject ball = Instantiate(BallPrefab, spawnPos, StartRotation);
                ball.name = ballIndex.ToString();

                MeshRenderer renderer = ball.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = Resources.Load<Material>($"Materials/ball_{ballIndex}");
                }

                ballIndex++;
            }
        }
        // -------------------- 
    }
    void CamMove()
    {
        // CamObj는 PlayerBall을 CamSpeed의 속도로 따라간다.
        // ---------- TODO ---------- 
        if (PlayerBall != null && CamObj != null)
        {
            // 카메라의 높이(y)는 유지하고 x, z만 따라감
            Vector3 targetPos = new Vector3(PlayerBall.transform.position.x, CamObj.transform.position.y, PlayerBall.transform.position.z);
            CamObj.transform.position = Vector3.Lerp(CamObj.transform.position, targetPos, CamSpeed * Time.deltaTime);
        }
        // -------------------- 
    }

    float CalcPower(Vector3 displacement)
    {
        return MinPower + displacement.magnitude * PowerCoef;
    }

    void ShootBallTo(Vector3 targetPos)
    {
        // targetPos의 위치로 공을 발사한다.
        // 힘은 CalcPower 함수로 계산하고, y축 방향 힘은 0으로 한다.
        // ForceMode.Impulse를 사용한다.
        // ---------- TODO ---------- 
        if (PlayerBall != null)
        {
            Vector3 direction = targetPos - PlayerBall.transform.position;
            direction.y = 0; // y축 힘 제거

            float power = CalcPower(direction);
            
            Rigidbody rb = PlayerBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(direction.normalized * power, ForceMode.Impulse);
            }
        }
        // -------------------- 
    }
    
    // When ball falls
    public void Fall(string ballName)
    {
        // "{ballName} falls"을 1초간 띄운다.
        // ---------- TODO ---------- 
        if (MyUIManager != null)
        {
            MyUIManager.DisplayText($"{ballName} falls", 1f);
        }
        // -------------------- 
    }
}