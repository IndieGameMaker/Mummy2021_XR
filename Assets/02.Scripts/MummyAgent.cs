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


    // 초기화 작업을 위해 한번만 호출
    public override void Initialize()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        targetTr = tr.parent.Find("Target")?.transform;
    }

    // 에피소드(학습단위)가 시작될때마다 호출
    public override void OnEpisodeBegin()
    {

    }

    // 주변 환경정보를 관측, 수집, 전달
    public override void CollectObservations(VectorSensor sensor)
    {

    }

    // 정책(브레인)으로 전달 받은대로 행동
    public override void OnActionReceived(ActionBuffers actions)
    {

    }

    // 개발자가 미리 테스트하기 위한 메소드 && 모방학습
    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }
}
