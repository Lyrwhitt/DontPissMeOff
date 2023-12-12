using System.Collections;
using UnityEngine;

public class BossShoulderMissileController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private GameObject _p1Obj;
    [SerializeField] private GameObject _p1SpawnObj;
    [SerializeField] private GameObject[] _leftMissileObj;
    [SerializeField] private GameObject[] _rightMissileObj;

    private GameObject[] _leftP1Obj;
    private GameObject[] _rightP1Obj;
    private Vector3[] _originLeftMissilePos;
    private Vector3[] _originLeftMissileLocalPos;
    private Vector3[] _originRightMissilePos;
    private Vector3[] _originRightMissileLocalPos;

    private Coroutine _fireMissileCO;
    private int _missileRowCount = 3;
    private float _percentOfSpeed = 0.003f;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        _leftP1Obj = new GameObject[_leftMissileObj.Length];
        _rightP1Obj = new GameObject[_rightMissileObj.Length];

        _originLeftMissilePos = new Vector3[_leftMissileObj.Length];
        _originLeftMissileLocalPos = new Vector3[_leftMissileObj.Length];
        _originRightMissilePos = new Vector3[_rightMissileObj.Length];
        _originRightMissileLocalPos = new Vector3[_rightMissileObj.Length];

        int share;
        int remain;

        for (int i = 0; i < _leftMissileObj.Length; i++)
        {
            share = (i + 1) / _missileRowCount;
            remain = (i + 1) % _missileRowCount;
            _originLeftMissilePos[i] = _leftMissileObj[i].transform.position;
            _originLeftMissileLocalPos[i] = _leftMissileObj[i].transform.localPosition;
            _leftP1Obj[i] = Instantiate(_p1Obj, _p1SpawnObj.transform);
            _leftP1Obj[i].transform.position = _originLeftMissilePos[i] + new Vector3(remain, (1.5f - share) * 1.5f, 0) * 4f;
        }

        for (int i = 0; i < _rightMissileObj.Length; i++)
        {
            share = (i + 1) / _missileRowCount;
            remain = (i + 1) % _missileRowCount;
            _originRightMissilePos[i] = _rightMissileObj[i].transform.position;
            _originRightMissileLocalPos[i] = _rightMissileObj[i].transform.localPosition;
            _rightP1Obj[i] = Instantiate(_p1Obj, _p1SpawnObj.transform);
            _rightP1Obj[i].transform.position = _originRightMissilePos[i] + new Vector3(-(_missileRowCount - remain), (1.5f - share) * 1.5f, 0) * 4f;
        }
    }

    public void FireMissile()
    {
        _fireMissileCO = StartCoroutine(FireMissileCO());
    }

    IEnumerator FireMissileCO()
    {
        BezierCurve bezierCurve = new BezierCurve();
        float percentToPlayer = 0f;
        Vector3 objPosition;
        Vector3 playerPosition;
        Vector3 p1;

        for (int i = 0; i < _leftMissileObj.Length; i++)
            _originLeftMissilePos[i] = _leftMissileObj[i].transform.position;

        for (int i = 0; i < _rightMissileObj.Length; i++)
            _originRightMissilePos[i] = _rightMissileObj[i].transform.position;

        while (true)
        {
            yield return null;

            percentToPlayer += _percentOfSpeed;
            playerPosition = _playerTransform.position;

            for (int i = 0; i < _leftMissileObj.Length; i++)
            {
                p1 = _leftP1Obj[i].transform.position;

                objPosition = bezierCurve.Quadratic(percentToPlayer, _originLeftMissilePos[i], p1, playerPosition);

                _leftMissileObj[i].transform.position = objPosition;
                _leftMissileObj[i].transform.LookAt(playerPosition);
            }

            for (int i = 0; i < _rightMissileObj.Length; i++)
            {
                p1 = _rightP1Obj[i].transform.position;

                objPosition = bezierCurve.Quadratic(percentToPlayer, _originRightMissilePos[i], p1, playerPosition);

                _rightMissileObj[i].transform.position = objPosition;
                _rightMissileObj[i].transform.LookAt(playerPosition);
            }

            if (percentToPlayer > 1.1f)
            {
                ResetMissilePos();
                break;
            }
        }
    }

    public void ResetMissilePos()
    {
        if (_fireMissileCO != null)
            StopCoroutine(_fireMissileCO);

        for (int i = 0; i < _leftMissileObj.Length; i++)
        {
            _leftMissileObj[i].GetComponent<BossRobotShoulderMissile>().RemoveMissile();
            _leftMissileObj[i].transform.localPosition = _originLeftMissileLocalPos[i];
            _leftMissileObj[i].transform.localRotation = Quaternion.identity;
        }

        for (int i = 0; i < _rightMissileObj.Length; i++)
        {
            _rightMissileObj[i].GetComponent<BossRobotShoulderMissile>().RemoveMissile();
            _rightMissileObj[i].transform.localPosition = _originRightMissileLocalPos[i];
            _rightMissileObj[i].transform.localRotation = Quaternion.identity;
        }
    }

    public void ExploseAll()
    {
        for (int i = 0; i < _leftMissileObj.Length; i++)
        {
            _leftMissileObj[i].GetComponent<BossRobotShoulderMissile>().Explose();
        }

        for (int i = 0; i < _rightMissileObj.Length; i++)
        {
            _rightMissileObj[i].GetComponent<BossRobotShoulderMissile>().Explose();
        }
    }
}
