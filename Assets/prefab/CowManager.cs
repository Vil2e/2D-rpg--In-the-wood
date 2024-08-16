using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CowManager : MonoBehaviour
{
	[SerializeField] private AssetReference animalReference;
	void Start()
	{
		AsyncOperationHandle<GameObject> asyncOperationHandle = animalReference.LoadAssetAsync<GameObject>();
		asyncOperationHandle.Completed += AsyncOperationHandle_Completed;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	
	void AsyncOperationHandle_Completed(AsyncOperationHandle<GameObject> asyncOperationHandle)
	{	//確認載入成功 -> 生成物件, 否則印出錯誤訊息
		if(asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
		{
			Instantiate(asyncOperationHandle.Result);
		}else
		{
			Debug.Log("Loading Asset failed!");
		}
	}
	
}
