using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;


public class Test1 : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        LuaEnv luaEnv = new LuaEnv();
        luaEnv.DoString("require 'main'");
    }

    // Update is called once per frame
    void Update() {

    }
}
