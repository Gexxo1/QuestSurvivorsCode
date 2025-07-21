using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BenchMarker : MonoBehaviour
{
    [Range(0f, 1000000), SerializeField] private int _iterations = 1000;
    private BenchMarkTest _benchMarkTest;

    private void Awake() {
        //_benchMarkTest = GetComponent<BenchMarkTest>();
        _benchMarkTest = FindObjectOfType<BenchMarkTest>();
        //UnityEngine.Debug.Log(_benchMarkTest);
    }

    public void RunTest() {
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();
        for(int i=0; i < _iterations; i++) {
            _benchMarkTest.PerformBenchmarkTest();
        }
        sw.Stop();

        UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");
    }
    /*
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Backslash) && _benchMarkTest != null)  RunTest();
    }
    */
    
}
