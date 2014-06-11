using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class DescriptionMasks {
	public const int XShift = 4;
	public const int YShift = 9;
	public const int RotShift = 14;
	public const int CoinShift = 16;
	public const int TBShift = 18;

	public const int Direction = 0xF;              //4bits
	public const int PositionX = 0x1F << XShift;   //5bits
	public const int PositionY = 0x1F << YShift;   //5bits
	public const int Rotation  = 0x3 << RotShift;  //2bits
	public const int Coin      = 0x3 << CoinShift; //2bits
	public const int TimeBoost = 0x3 << TBShift;   //2bits
}
