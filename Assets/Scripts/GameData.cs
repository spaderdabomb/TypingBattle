using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
class PlayerData
{
	public float currentRating;
}

public class GameData : MonoBehaviour
{

	public static GameData instance = null;

	public float currentRating;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		InitGame();
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
		PlayerData data = new PlayerData();

		// Data
		data.currentRating = currentRating;

		bf.Serialize(file, data);
		file.Close();
	}

	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();

			// Data
			currentRating = data.currentRating;
		}

	}

	public void ResetData()
	{
		currentRating = 1000;

		GameData.instance.Save();
	}

	void InitGame()
	{
		GameData.instance.Load();
		GameData.instance.Save();
	}


	void Update()
	{

	}
}