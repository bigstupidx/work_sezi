using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace AssemblyCSharp
{
	public class SeZiGlobalData
	{

		private static SeZiGlobalData instance;
		public static SeZiGlobalData getMe() {
			if (instance == null) {
				instance = new SeZiGlobalData ();
			}
			return instance;
		}

		public RoomJoinResponseVo createVO;

		public void Clear() {
			if (createVO != null) {
				createVO.playerList.Clear ();
			}
			//gameStart = gameEnd = false;
			createVO = null;
            reset();
		}

		public SeZiGlobalData ()
		{
		}

        public void reset()
        {
            
    	}

		//飞的动画，统一调试这个 needCloneObj 复制的对象		targetObj目标对象	parentObj 原始父
		public void flyGoldOrPK(GameObject needCloneObj,GameObject targetObj, GameObject parentObj, 
			int min = 1, int max = 1, bool addToTarget = true, bool randomPlace = true){
			int tmpNum = Random.Range(min,max);
			needCloneObj.SetActive (false);
			Vector3 _initPosition = needCloneObj.transform.position;
			Vector3 _targetPosition = targetObj.transform.position;
			for (int i = 0; i < tmpNum; i++) {
				GameObject cloneObj = GameObject.Instantiate (needCloneObj);
				cloneObj.SetActive (true);
                cloneObj.transform.SetParent(parentObj.transform);
                cloneObj.transform.localPosition = needCloneObj.transform.localPosition;
				cloneObj.transform.localScale = needCloneObj.transform.localScale;
				float tmpx = randomPlace ? Random.Range(-20f,20f) : 0;
				float tmpy = randomPlace ? Random.Range(-20f, 20f) : 0;
                float tmpDuration = 1f - i * 0.2f;//UnityEngine.Random.Range(0.2f,1f);
				Sequence suqence=DOTween.Sequence();
				suqence.Append(cloneObj.transform.DOMove(new Vector3(_targetPosition.x+tmpx,_targetPosition.y + tmpy,_targetPosition.z),tmpDuration));
				suqence.AppendCallback(()=>{
					if (!addToTarget) {
						GameObject.Destroy(cloneObj);
					}else{
						cloneObj.transform.SetParent(targetObj.transform);
						cloneObj.transform.localScale=Vector3.one;
					}
				});
				suqence.SetDelay(0.1f);
				suqence.SetAutoKill(true);
			}
		}

        //飞的动画，统一调试这个 needCloneObj 复制的对象		targetObj目标对象	parentObj 原始父
        public void flyDingImg(GameObject needCloneObj, GameObject targetObj, GameObject parentObj )
        {
            needCloneObj.SetActive(false);
            Vector3 _initPosition = needCloneObj.transform.position;
            Vector3 _targetPosition = targetObj.transform.position;
            GameObject cloneObj = GameObject.Instantiate(needCloneObj);
            cloneObj.SetActive(true);
            cloneObj.transform.SetParent(parentObj.transform);
            cloneObj.transform.localPosition = needCloneObj.transform.localPosition;
            cloneObj.transform.localScale = needCloneObj.transform.localScale;
            float tmpDuration = 0.5f;
            Sequence suqence = DOTween.Sequence();
            suqence.Append(cloneObj.transform.DOMove(new Vector3(_targetPosition.x, _targetPosition.y, _targetPosition.z), tmpDuration));
            suqence.AppendCallback(() => {
                GameObject.Destroy(cloneObj);
            });
            suqence.SetDelay(0.1f);
            suqence.SetAutoKill(true);
        }

    }
}

