using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tem.BaseCode {
    public class BaseCode : MonoBehaviour
    {
        public string GameName;
        public string GameRule;

        void Start()
        {
            // Director director = Director.getInstance();
            // director.setBaseCode(this);
            GameName = "";
            GameRule = "";
        }
    }
}
