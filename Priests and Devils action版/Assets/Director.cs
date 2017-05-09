using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Scene;

namespace My.Director
{
    public class Director : System.Object
    {
        public static Director _instance;
        public ISenceController currentSceneController { get; set; }
        public BaseCode baseCode { get; set; }
        public bool running { get; set; }

        public static Director getInstance()
        {
            if (_instance == null) _instance = new Director();
            return _instance;
        }

        public int getFPS()
        {
            return Application.targetFrameRate;
        }

        public void setFPS(int FPS)
        {
            Application.targetFrameRate = FPS;
        }

        public void setBaseCode(BaseCode bc)
        {
            if (baseCode == null) baseCode = bc;
        }

        public BaseCode getBaseCode()
        {
            return baseCode;
        }

        public void setScene(ISenceController iscene)
        {
            currentSceneController = iscene;
        }
    }
}