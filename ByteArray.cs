/*
 * Created by SharpDevelop.
 * User: C3rebro
 * Date: 12.03.2018
 * Time: 21:02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace ByteArrayHelper
{

	public class ByteArray
	{
		public int Length { get { return Data.Length; } }
		
		public byte[] Data { get; set; }
		
		public ByteArray()
		{
			
		}
		
		public ByteArray(int length)
		{
			Data = new byte[length];
		}
		
		public ByteArray(byte[] _data)
		{
			Data = new byte[_data.Length];
			Data = _data;
		}
		
		public byte[] Or(List<byte[]> source, bool isLittleEndian = true)
		{
			
			ObservableCollection<byte[]> localCopy = new ObservableCollection<byte[]>(source);
			
			for (int i = source.Count; i > 0; i--)
			{
				foreach ( byte[] b in source)
				{
					
				}
			}
			
			return null;
			//localCopy.Aggregate((i, j) => i.MainListWords.Length > j.MainListWords.Length ? i : j)
		}
		
		public ByteArray Or(byte[] source, bool isLittleEndian = true)
		{
			
			byte[] localCopy = new byte[Data.Length];
			
			if(!isLittleEndian) // or-ing from right to left
			{
//				for (int i = localCopy.Length; i > 0; i--)
//				{
//					if((localCopy.Length - source.Length) < i)
//					{
//						Data[i] |= source[i - ];
//					}
//				}
			}
			else // the other way around
			{
				for (int i = 0; i < localCopy.Length; i++)
				{
					if(source.Length > i)
					{
						Data[i] |= source[i];
					}
					else
						break;
				}
			}
			return this;
		}
	}
	
	namespace Extensions
	{
		/// <summary>
		/// Convert byte and byte[] to and from other types
		/// </summary>
		public static class ByteConverter
		{
			/// <summary>
			/// Reverses an Array of Bytes from Little Endian to Big Endian or Vice Versa
			/// </summary>
			/// <param name="arrToReverse">The byte[] Array to be reversed</param>
			/// <returns>The byte[] Array in Reversed Order</returns>
			public static byte[] Reverse(byte[] arrToReverse)
			{
				if (BitConverter.IsLittleEndian)
					Array.Reverse(arrToReverse);
				
				return arrToReverse;
			}
			
			/// <summary>
			/// Reverses the Bit Order in a Single Byte
			/// </summary>
			/// <param name="byteToPutInReverseOrder">The byte to be reversed</param>
			/// <returns>The byte in reversed Order</returns>
			public static byte Reverse(byte byteToPutInReverseOrder)
			{
				byte byteInReversedOrder = 0;
				for (byte i = 0; i < 8; ++i)
				{
					byteInReversedOrder <<= 1;
					byteInReversedOrder |= (byte)(byteToPutInReverseOrder & 1);
					byteToPutInReverseOrder >>= 1;
				}
				return byteInReversedOrder;
			}
			
			/// <summary>
			/// Gets the Amount of bytes in a string that contains one or more bytes in the format "00" to "FF" each byte
			/// </summary>
			/// <param name="hexString">A string containing one or more bytes in the format "00" to "FF" each byte</param>
			/// <returns>The Amount of Bytes in the string</returns>
			public static int GetByteCount(string hexString)
			{
				int numHexChars = 0;
				char c;
				// remove all none A-F, 0-9, characters
				for (int i = 0; i < hexString.Length; i++)
				{
					c = hexString[i];
					if (IsHexDigit(c))
						numHexChars++;
				}
				// if odd number of characters, discard last character
				if (numHexChars % 2 != 0)
				{
					numHexChars--;
				}
				return numHexChars / 2; // 2 characters per byte
			}

			/// <summary>
			/// Converts a string with hexadecimal chars to a byte array (e.g.: "FF00FF" -> byte[3]{0xFF,0x00,0xFF})
			/// </summary>
			/// <param name="hexString">The string to convert</param>
			/// <param name="discarded">The Number of characters in the string that could not be converted to a byte</param>
			/// <returns>The Array that contains all converted values. Reliably only if <param name="discarded"/> is returned as 0</returns>
			public static byte[] GetBytes(string hexString, out int discarded)
			{
				discarded = 0;
				string newString = "";
				char c;
				// remove all none A-F, 0-9, characters
				for (int i = 0; i < hexString.Length; i++)
				{
					c = hexString[i];
					if (IsHexDigit(c))
						newString += c;
					else
						discarded++;
				}
				// if odd number of characters, discard last character
				if (newString.Length % 2 != 0)
				{
					discarded++;
					newString = newString.Substring(0, newString.Length - 1);
				}

				int byteLength = newString.Length / 2;
				byte[] bytes = new byte[byteLength];
				string hex;
				int j = 0;
				for (int i = 0; i < bytes.Length; i++)
				{
					hex = new String(new Char[] { newString[j], newString[j + 1] });
					bytes[i] = HexToByte(hex);
					j = j + 2;
				}
				return bytes;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="bytes"></param>
			/// <returns></returns>
			public static string HexToString(byte[] bytes)
			{
				string hexString = "";
				for (int i = 0; i < bytes.Length; i++)
				{
					hexString += bytes[i].ToString("X2");
				}
				return hexString;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="bytes"></param>
			/// <returns></returns>
			public static string HexToString(byte bytes)
			{
				string hexString = "";
				{
					hexString += bytes.ToString("X2");
				}
				return hexString;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="hexString"></param>
			/// <returns></returns>
			public static bool IsInHexFormat(string hexString)
			{
				bool hexFormat = true;

				foreach (char digit in hexString)
				{
					if (!IsHexDigit(digit))
					{
						hexFormat = false;
						break;
					}
				}
				return hexFormat;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="c"></param>
			/// <returns></returns>
			public static bool IsHexDigit(Char c)
			{
				int numChar;
				int numA = Convert.ToInt32('A');
				int num1 = Convert.ToInt32('0');
				c = Char.ToUpper(c);
				numChar = Convert.ToInt32(c);
				if (numChar >= numA && numChar < (numA + 6))
					return true;
				if (numChar >= num1 && numChar < (num1 + 10))
					return true;
				return false;
			}

			private static byte HexToByte(string hex)
			{
				byte newByte = 0x00;
				
				try
				{
					if (!(hex.Length > 2 || hex.Length <= 0))
					{
						newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
					}
				}
				catch
				{
					return 0x00;
				}

				return newByte;
			}
		}
	}

}