using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

/* 에이젼트의 역할
    1. 주변 환경을 관측   (Observations)
    2. 정책에 따라서 행동 (Actions)
    3. 보상 (Reward)
*/

public class MummyAgent : Agent
{
    private Transform tr;
    private Rigidbody rb;
    private Transform targetTr;

    private Material originMt;
    public Material goodMt;
    public Material badMt;

    private Renderer floorRd;

    // 초기화 작업을 위해 한번만 호출
    public override void Initialize()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        targetTr = tr.parent.Find("Target")?.transform;
        floorRd = tr.parent.Find("Floor")?.GetComponent<MeshRenderer>();

        originMt = floorRd.material;
    }

    // 에피소드(학습단위)가 시작될때마다 호출
    public override void OnEpisodeBegin()
    {
        // Rigidbody 물리력을 초기화
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 타겟의 위치를 변경
        targetTr.localPosition = new Vector3(Random.Range(-4.0f, 4.0f),
                                             0.55f,
                                             Random.Range(-4.0f, 4.0f));

        // Agent 위치 변경
        tr.localPosition = new Vector3(Random.Range(-4.0f, 4.0f),
                                       0.05f,
                                       Random.Range(-4.0f, 4.0f));

    }

    // 주변 환경정보를 관측, 수집, 전달
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(targetTr.localPosition);  // 3 (x,y,z)
        sensor.AddObservation(tr.localPosition);        // 3
        sensor.AddObservation(rb.velocity.x);           // 1
        sensor.AddObservation(rb.velocity.z);           // 1
    }

    // 정책(브레인)으로 전달 받은대로 행동
    public override void OnActionReceived(ActionBuffers actions)
    {
        float h = actions.ContinuousActions[0]; // 좌우 화살표 키 -1.0f ~ +1.0f
        float v = actions.ContinuousActions[1]; // 상하 화살표 키 -1.0f ~ +1.0f

        Vector3 dir = (Vector3.forward * v) + (Vector3.right * h);
        rb.AddForce(dir.normalized * 50.0f);

        SetReward(-0.001f);
    }

    // 개발자가 미리 테스트하기 위한 메소드 && 모방학습
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //Input.GetAxis("Horizontal"); // -1.0f ~ 0.0f ~ +1.0f 연속적인 값 = Continues
        //Input.GetAxisRaw("Horizontal"); // -1.0f, 0.0f, +1.0f 이산 = Discrete

        var actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxis("Horizontal");
        actions[1] = Input.GetAxis("Vertical");
    }

    void OnCollisionEnter(Collision coll)
    {
        /*
            SetReward
            AddReward
        */

        if (coll.collider.CompareTag("DEAD_ZONE"))
        {
            // 잘못된 행동에 대한 마이너스 보상
            SetReward(-1.0f);
            // 학습을 종료
            EndEpisode();
        }

        if (coll.collider.CompareTag("TARGET"))
        {
            // 올바른 행동에 대한 플러스 보상
            SetReward(+1.0f);
            // 학습을 종료
            EndEpisode();
        }
    }
}
