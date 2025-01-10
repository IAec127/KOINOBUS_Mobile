using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public static class PlayerProperties
{
	private const string ScoreKey = "s";
	private const string HpKey = "h";

	private static readonly Hashtable propsToSet = new Hashtable();

    // プレイヤーのスコアを取得する
    public static int GetScore(this Player player)
	{
		return (player.CustomProperties[ScoreKey] is int score) ? score : 0;
	}

	// プレイヤーのスコアを設定する
	public static void SetScore(this Player player, int score)
	{
		propsToSet[ScoreKey] = score;
		player.SetCustomProperties(propsToSet);
		propsToSet.Clear();
	}

	// プレイヤーのスコアを取得する
	public static int GetHp(this Player player)
	{
		return (player.CustomProperties[HpKey] is int hp) ? hp : 0;
	}

	// プレイヤーのスコアを設定する
	public static void SetHp(this Player player, int hp)
	{
		propsToSet[HpKey] = hp;
		player.SetCustomProperties(propsToSet);
		propsToSet.Clear();
	}

}
