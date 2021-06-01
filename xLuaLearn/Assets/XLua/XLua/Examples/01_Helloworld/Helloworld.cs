/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

// 已看
using UnityEngine;
using XLua;

namespace XLuaTest {
    public class Helloworld : MonoBehaviour {
        // Use this for initialization
        void Start() {

            DoString();


        }

        private void DoString() {
            // LuaEnv
            LuaEnv luaEnv = new LuaEnv();

            // 描述
            // 执行一个代码块
            //
            // 参数值
            // chunk: Lua代码文字串
            // chunkName: 发生error时的debug显示信息中使用,指明某某代码块的某行错误
            // env: 这个代码块的环境变量
            //
            // 返回值
            // 代码块里 return 语句的返回值
            object[] returns = luaEnv.DoString("CS.UnityEngine.Debug.Log('Hello World'); return 1, 'hello';");

            foreach (var ret in returns) {
                Debug.Log(ret);
            }

            luaEnv.Dispose();

        }


        // Update is called once per frame
        void Update() {

        }
    }
}
