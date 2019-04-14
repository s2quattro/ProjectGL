using ScottGarland;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CityStage;


namespace GLCore
{
	public static class GLAPI
	{
        private const decimal unit4 = 10000000000000000;
        private const decimal unit3 = 1000000000000;
        private const decimal unit2 = 100000000;
        private const decimal unit1 = 10000;

        public static void StatExpInc(PlayerExpChangeInfo playerExpChangeInfo)
        {
            LocalStorage localStorage = LinkContainer.Instance.localStorage;

            switch (playerExpChangeInfo)
            {                        
                case PlayerExpChangeInfo.creditExp:
                    {
                        BankMetadata bankMetadata = LinkContainer.Instance.bankMetadata;

                        while ((localStorage.playerStat.GetCredit < Constants.statMax)
                            && (localStorage.playerStat.GetCreditExp >= bankMetadata.demandExpPerCredit[localStorage.playerStat.GetCredit - 1]))
                        {
                            localStorage.playerStat.ChangeExp(PlayerExpChangeInfo.creditExp, ValueChangeInfo.decrease, bankMetadata.demandExpPerCredit[localStorage.playerStat.GetCredit - 1]);
                            localStorage.playerStat.ChangeStat(PlayerStatChangeInfo.credit, ValueChangeInfo.increase, 1);

                            UIManager.Instance.exeOpenSimpleMessageBox("알림", string.Format("신용등급 {0}등급 -> {1}등급 1단계 상승", localStorage.playerStat.GetCredit - 1, localStorage.playerStat.GetCredit));
                        }
                        break;
                    }
                case PlayerExpChangeInfo.fitnessExp:
                    {
                        CharActionMetadata charActionMetadata = LinkContainer.Instance.charActionMetadata;

                        while ((localStorage.playerStat.GetFitness < Constants.statMax)
                            && (localStorage.playerStat.GetFitnessExp >= charActionMetadata.demandExpPerFitness[(int)localStorage.playerStat.GetFitness - 1]))
                        {
                            localStorage.playerStat.ChangeExp(PlayerExpChangeInfo.fitnessExp, ValueChangeInfo.decrease, charActionMetadata.demandExpPerFitness[(int)localStorage.playerStat.GetFitness - 1]);
                            localStorage.playerStat.ChangeStat(PlayerStatChangeInfo.fitness, ValueChangeInfo.increase, 1);

                            UIManager.Instance.exeOpenSimpleMessageBox("알림", string.Format("체력 {0}등급 -> {1}등급 1단계 상승", localStorage.playerStat.GetFitness - 1, localStorage.playerStat.GetFitness));
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public static int GetTimeChangeAmount(CustumDateTime custumDateTime, uint renewTime = 3600, uint maxRenewNum = 24)
        {
            int timeChangeAmount = 0;

            try
            {
                DateTime curTime = GetNetworkTime();

                // 초기화가 안돼있다면 최근시간을 현재시간으로 저장시킨다
                if ((custumDateTime.month == 0)
                        || (custumDateTime.day == 0)
                        || (custumDateTime.year == 0))
                {
                    custumDateTime.year = curTime.Year;
                    custumDateTime.month = curTime.Month;
                    custumDateTime.day = curTime.Day;
                    custumDateTime.hour = curTime.Hour;
                    custumDateTime.minute = curTime.Minute;
                    custumDateTime.second = curTime.Second;
                    
                    LinkContainer.Instance.localStorage.SaveToLocal(FileSaveType.recentTime);

                    return 0;
                }

                DateTime recentTime = new DateTime(custumDateTime.year,
                                                    custumDateTime.month,
                                                    custumDateTime.day,
                                                    custumDateTime.hour,
                                                    custumDateTime.minute,
                                                    custumDateTime.second);

                TimeSpan timeSpan = curTime - recentTime;
                
                timeChangeAmount = (int)((uint)timeSpan.TotalSeconds / renewTime);

                if (timeChangeAmount > maxRenewNum)
                {
                    timeChangeAmount = (int)maxRenewNum;
                }

                if (timeChangeAmount > 0)
                {
                    custumDateTime.year = curTime.Year;
                    custumDateTime.month = curTime.Month;
                    custumDateTime.day = curTime.Day;
                    custumDateTime.hour = curTime.Hour;
                    custumDateTime.minute = curTime.Minute;
                    custumDateTime.second = curTime.Second;
                    
                    LinkContainer.Instance.localStorage.SaveToLocal(FileSaveType.recentTime);
                }
            }
            catch
            {
                return -1;
            }

            return timeChangeAmount;
        }
        
		#region Time import from NTP 

		public static DateTime GetNetworkTime()
		{
			//default Windows time server
			const string ntpServer = "time.windows.com";

			// NTP message size - 16 bytes of the digest (RFC 2030)
			var ntpData = new byte[48];

			//Setting the Leap Indicator, Version Number and Mode values
			ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

			var addresses = Dns.GetHostEntry(ntpServer).AddressList;

			//The UDP port number assigned to NTP is 123
			var ipEndPoint = new IPEndPoint(addresses[0], 123);
			//NTP uses UDP

			using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
			{
				socket.Connect(ipEndPoint);

				//Stops code hang if NTP is blocked
				socket.ReceiveTimeout = 3000;

				socket.Send(ntpData);
				socket.Receive(ntpData);
				socket.Close();
			}

			//Offset to get to the "Transmit Timestamp" field (time at which the reply 
			//departed the server for the client, in 64-bit timestamp format."
			const byte serverReplyTime = 40;

			//Get the seconds part
			ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

			//Get the seconds fraction
			ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

			//Convert From big-endian to little-endian
			intPart = SwapEndianness(intPart);
			fractPart = SwapEndianness(fractPart);

			var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

			//**UTC** time
			var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

			return networkDateTime.ToLocalTime();
		}
		// stackoverflow.com/a/3294698/162671
		private static uint SwapEndianness(ulong x)
		{
			return (uint)(((x & 0x000000ff) << 24) +
						   ((x & 0x0000ff00) << 8) +
						   ((x & 0x00ff0000) >> 8) +
						   ((x & 0xff000000) >> 24));
		}

        #endregion

        public static string StringToWon(string value)
        {
            string strTmp;
            StringBuilder strBuilder = new StringBuilder();
            decimal curValue = Convert.ToDecimal(value);

            if (decimal.Compare(curValue, 0) <= 0)
            {
                strBuilder.AppendFormat(string.Format("0"));
            }
            else
            {
                if (decimal.Compare(curValue, unit4) > -1)
                {
                    strTmp = curValue.ToString();
                    strBuilder.AppendFormat(string.Format("{0}해 ", strTmp.Substring(0, strTmp.Length - 16)));
                    curValue -= (Convert.ToDecimal(strTmp.Substring(0, strTmp.Length - 16)) * unit4);
                }
                if (decimal.Compare(curValue, unit3) > -1)
                {
                    strTmp = curValue.ToString();
                    strBuilder.AppendFormat(string.Format("{0}경 ", strTmp.Substring(0, strTmp.Length - 12)));
                    curValue -= (Convert.ToDecimal(strTmp.Substring(0, strTmp.Length - 12)) * unit3);
                }
                if (decimal.Compare(curValue, unit2) > -1)
                {
                    strTmp = curValue.ToString();
                    strBuilder.AppendFormat(string.Format("{0}조 ", strTmp.Substring(0, strTmp.Length - 8)));
                    curValue -= (Convert.ToDecimal(strTmp.Substring(0, strTmp.Length - 8)) * unit2);
                }
                if (decimal.Compare(curValue, unit1) > -1)
                {
                    strTmp = curValue.ToString();
                    strBuilder.AppendFormat(string.Format("{0}억 ", strTmp.Substring(0, strTmp.Length - 4)));
                    curValue -= (Convert.ToDecimal(strTmp.Substring(0, strTmp.Length - 4)) * unit1);
                }
                if (decimal.Compare(curValue, 0) > 0)
                {
                    strBuilder.AppendFormat(string.Format("{0}만 ", curValue));
                }
            }

            strBuilder.AppendFormat(string.Format("원"));
            return strBuilder.ToString();
        }

        public static string DecimalToWon(decimal value)
        {
            string strTmp;
            StringBuilder strBuilder = new StringBuilder();
            decimal curValue = value;

            if (decimal.Compare(curValue, 0) <= 0)
            {
                strBuilder.AppendFormat(string.Format("0"));
            }
            else
            {
                if (decimal.Compare(curValue, unit4) > -1)
                {
                    strTmp = curValue.ToString();
                    strBuilder.AppendFormat(string.Format("{0}해 ", strTmp.Substring(0, strTmp.Length - 16)));
                    curValue -= (Convert.ToDecimal(strTmp.Substring(0, strTmp.Length - 16)) * unit4);
                }
                if (decimal.Compare(curValue, unit3) > -1)
                {
                    strTmp = curValue.ToString();
                    strBuilder.AppendFormat(string.Format("{0}경 ", strTmp.Substring(0, strTmp.Length - 12)));
                    curValue -= (Convert.ToDecimal(strTmp.Substring(0, strTmp.Length - 12)) * unit3);
                }
                if (decimal.Compare(curValue, unit2) > -1)
                {
                    strTmp = curValue.ToString();
                    strBuilder.AppendFormat(string.Format("{0}조 ", strTmp.Substring(0, strTmp.Length - 8)));
                    curValue -= (Convert.ToDecimal(strTmp.Substring(0, strTmp.Length - 8)) * unit2);
                }
                if (decimal.Compare(curValue, unit1) > -1)
                {
                    strTmp = curValue.ToString();
                    strBuilder.AppendFormat(string.Format("{0}억 ", strTmp.Substring(0, strTmp.Length - 4)));
                    curValue -= (Convert.ToDecimal(strTmp.Substring(0, strTmp.Length - 4)) * unit1);
                }
                if (decimal.Compare(curValue, 0) > 0)
                {
                    strBuilder.AppendFormat(string.Format("{0}만 ", curValue));
                }
            }

            strBuilder.AppendFormat(string.Format("원"));
            return strBuilder.ToString();
        }

        /*
		public static string MoneyToString(BigInteger value)
		{
			string strTmp;
			StringBuilder strBuilder = new StringBuilder();
			BigInteger curValue = value;

			if (BigInteger.Compare(curValue, 0) <= 0)
			{
				strBuilder.AppendFormat(string.Format("0"));
			}
			else
			{
				if (BigInteger.Compare(curValue, unit4) > -1)
				{
					strTmp = curValue.ToString();
					strBuilder.AppendFormat(string.Format("{0}해 ", strTmp.Substring(0, strTmp.Length - 16)));
					curValue -= (new BigInteger(strTmp.Substring(0, strTmp.Length - 16)) * unit4);
				}
				if (BigInteger.Compare(curValue, unit3) > -1)
				{
					strTmp = curValue.ToString();
					strBuilder.AppendFormat(string.Format("{0}경 ", strTmp.Substring(0, strTmp.Length - 12)));
					curValue -= (new BigInteger(strTmp.Substring(0, strTmp.Length - 12)) * unit3);
				}
				if (BigInteger.Compare(curValue, unit2) > -1)
				{
					strTmp = curValue.ToString();
					strBuilder.AppendFormat(string.Format("{0}조 ", strTmp.Substring(0, strTmp.Length - 8)));
					curValue -= (new BigInteger(strTmp.Substring(0, strTmp.Length - 8)) * unit2);
				}
				if (BigInteger.Compare(curValue, unit1) > -1)
				{
					strTmp = curValue.ToString();
					strBuilder.AppendFormat(string.Format("{0}억 ", strTmp.Substring(0, strTmp.Length - 4)));
					curValue -= (new BigInteger(strTmp.Substring(0, strTmp.Length - 4)) * unit1);
				}
				if (BigInteger.Compare(curValue, 0) > 0)
				{
					strBuilder.AppendFormat(string.Format("{0}만 ", curValue));
				}
			}

			strBuilder.AppendFormat(string.Format("원"));
			return strBuilder.ToString();
		}*/


        #region BigInteger Viewer
        //-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-

        // 🛠🛠🛠🛠
        public const string krLargeNumberNames = "만억조경해자양";  //"만억조경해자양구간정재극";
		public const string krCutLargeNumberNames = "억조경해자양";  //"만억조경해자양구간정재극";
		public const string usLargeNumberNames = "KMBTQ";

		/*
		/// <summary>
		///		Seperate cash string use certain unit to better view
		/// </summary>
		/// <param name="totalCash">Insert BigInteger</param>
		/// <param name="NumberInterval">Interval of division</param>
		/// <param name="unitString">Currency unit (it will not attach the unit, when set null)</param>
		/// <returns></returns>
		public static string convertCashToWon(BigInteger totalCash, int NumberInterval = 4, string unitString = " 원")
		{
			if (NumberInterval < 2)
			{
				throw new ArgumentException("Invaild a interval amount : Set interval value greater than 2");
			}
			//BitArray zeroChecker = new BitArray(NumberInterval, false);
			StringBuilder cashText = new StringBuilder(totalCash.ToString());

			#region Initializer

			//Set cut count
			int cutCount = 0;
			int tempCashLength = cashText.Length - 1;
			cutCount = tempCashLength / NumberInterval;

			if (cutCount == 0)
			{   //O Need No processing! :)

				//Attach a currency unit
				if (unitString != null)
					cashText.Append(unitString);

				return cashText.ToString();
			}

			//For reuse Local var
			tempCashLength++;

			//Clamp cut count Prevent Excess Numberical Char Count
			if (cutCount > krLargeNumberNames.Length)
				cutCount = krLargeNumberNames.Length;

			//Debug.Log(krLargeNumberNames.Length);

			#endregion

			//Cut string as Numberical Inverval
			for (int i = 0; i < cutCount; i++)
			{
				//First try
				if (i == 0)
				{
					#region Zero Filtering

					//Set Area

					//RightSide index each cut
					int rightCapIndex = tempCashLength;
					//LeftSide index each cut
					tempCashLength -= NumberInterval;

					//Search Zero left to right
					int zeroCounter = 0;
					for (int x = tempCashLength; x < rightCapIndex; x++)
					{
						if (cashText[x] == '0')
						{//Zero is checked
							zeroCounter++;
						}
						else
						{//Not Zero
							break;
						}
					}

					//Cut Zero char If counter is greater than 0
					if (zeroCounter > 0)
						cashText.Remove(tempCashLength, zeroCounter);

					#endregion
				}


				#region Check Next Area

				//Set Next Area
				//Next RightCap index each cut
				int nextRightCapIndex = tempCashLength;  //same as previousLeftIndex
														 //Next LeftSide index each cut
				tempCashLength -= NumberInterval;

				//the Final unit detected
				if (tempCashLength < 1)
				{
					//Attach an unit
					cashText.Insert(nextRightCapIndex, string.Format("{0} ", krLargeNumberNames[i]));
					//And Exit
					break;
				}

				//Search Zero left to right
				int nextZeroCounter = 0;
				for (int x = tempCashLength; x < nextRightCapIndex; x++)
				{
					if (cashText[x] == '0')
					{//Zero is checked
						nextZeroCounter++;
					}
					else
					{//Not Zero
						break;
					}
				}

				#endregion


				#region Attach Measure Sign

				//Attach an unit : whether next Area have a value
				if (nextZeroCounter < NumberInterval)
					cashText.Insert(nextRightCapIndex, string.Format("{0} ", krLargeNumberNames[i]));

				#endregion

				//Cut Zero char If counter is greater than 0
				if (nextZeroCounter > 0)
					cashText.Remove(tempCashLength, nextZeroCounter);
			}

			//Attach a currency unit
			if (unitString != null)
				cashText.Append(unitString);

			return cashText.ToString();
		}
		*/

		/// <summary>
		///		Seperate cash string use certain unit to better view
		/// </summary>
		/// <param name="cash">Insert Decimal</param>
		/// <param name="NumberInterval">Interval of division</param>
		/// <param name="unitString">Currency unit (it will not attach the unit, when set null)</param>
		/// <returns></returns>
		public static string convertCashToWon(decimal cash, int NumberInterval = 4, string unitString = " 원")
		{
			if (NumberInterval < 2)
			{
				throw new ArgumentException("Invaild a interval amount : Set interval value greater than 2");
			}

			cash = decimal.Truncate(cash);

			//BitArray zeroChecker = new BitArray(NumberInterval, false);
			StringBuilder cashText = new StringBuilder(cash.ToString());

			#region Initializer

			//Set cut count
			int cutCount = 0;
			int tempCashLength = cashText.Length - 1;
			cutCount = tempCashLength / NumberInterval;

			if (cutCount == 0)
			{   //O Need No processing! :)

				//Attach a currency unit
				if (unitString != null)
					cashText.Append(unitString);

				return cashText.ToString();
			}

			//For reuse Local var
			tempCashLength++;

			//Clamp cut count Prevent Excess Numberical Char Count
			if (cutCount > krLargeNumberNames.Length)
				cutCount = krLargeNumberNames.Length;

			//Debug.Log(krLargeNumberNames.Length);

			#endregion

			//Cut string as Numberical Inverval
			for (int i = 0; i < cutCount; i++)
			{
				//First try
				if (i == 0)
				{
					#region Zero Filtering

					//Set Area

					//RightSide index each cut
					int rightCapIndex = tempCashLength;
					//LeftSide index each cut
					tempCashLength -= NumberInterval;

					//Search Zero left to right
					int zeroCounter = 0;
					for (int x = tempCashLength; x < rightCapIndex; x++)
					{
						if (cashText[x] == '0')
						{//Zero is checked
							zeroCounter++;
						}
						else
						{//Not Zero
							break;
						}
					}

					//Cut Zero char If counter is greater than 0
					if (zeroCounter > 0)
						cashText.Remove(tempCashLength, zeroCounter);

					#endregion
				}


				#region Check Next Area

				//Set Next Area
				//Next RightCap index each cut
				int nextRightCapIndex = tempCashLength;  //same as previousLeftIndex
														 //Next LeftSide index each cut
				tempCashLength -= NumberInterval;

				//the Final unit detected
				if (tempCashLength < 1)
				{
					//Attach an unit
					cashText.Insert(nextRightCapIndex, string.Format("{0} ", krLargeNumberNames[i]));
					//And Exit
					break;
				}

				//Search Zero left to right
				int nextZeroCounter = 0;
				for (int x = tempCashLength; x < nextRightCapIndex; x++)
				{
					if (cashText[x] == '0')
					{//Zero is checked
						nextZeroCounter++;
					}
					else
					{//Not Zero
						break;
					}
				}

				#endregion


				#region Attach Measure Sign

				//Attach an unit : whether next Area have a value
				if (nextZeroCounter < NumberInterval)
					cashText.Insert(nextRightCapIndex, string.Format("{0} ", krLargeNumberNames[i]));

				#endregion

				//Cut Zero char If counter is greater than 0
				if (nextZeroCounter > 0)
					cashText.Remove(tempCashLength, nextZeroCounter);
			}

			//Attach a currency unit
			if (unitString != null)
				cashText.Append(unitString);

			return cashText.ToString();
		}
		/// <summary>
		///		Seperate cash string use certain unit to better view
		/// </summary>
		/// <param name="cash">Insert String</param>
		/// <param name="NumberInterval">Interval of division</param>
		/// <param name="unitString">Currency unit (it will not attach the unit, when set null)</param>
		/// <returns></returns>
		public static string convertCashToWon(string cash, int NumberInterval = 4, string unitString = " 원")
		{
			if (NumberInterval < 2)
			{
				throw new ArgumentException("Invaild a interval amount : Set interval value greater than 2");
			}			

			//BitArray zeroChecker = new BitArray(NumberInterval, false);
			StringBuilder cashText = new StringBuilder(cash);

			#region Initializer

			//Set cut count
			int cutCount = 0;
			int tempCashLength = cashText.Length - 1;
			cutCount = tempCashLength / NumberInterval;

			if (cutCount == 0)
			{   //O Need No processing! :)

				//Attach a currency unit
				if (unitString != null)
					cashText.Append(unitString);

				return cashText.ToString();
			}

			//For reuse Local var
			tempCashLength++;

			//Clamp cut count Prevent Excess Numberical Char Count
			if (cutCount > krLargeNumberNames.Length)
				cutCount = krLargeNumberNames.Length;

			//Debug.Log(krLargeNumberNames.Length);

			#endregion

			//Cut string as Numberical Inverval
			for (int i = 0; i < cutCount; i++)
			{
				//First try
				if (i == 0)
				{
					#region Zero Filtering

					//Set Area

					//RightSide index each cut
					int rightCapIndex = tempCashLength;
					//LeftSide index each cut
					tempCashLength -= NumberInterval;

					//Search Zero left to right
					int zeroCounter = 0;
					for (int x = tempCashLength; x < rightCapIndex; x++)
					{
						if (cashText[x] == '0')
						{//Zero is checked
							zeroCounter++;
						}
						else
						{//Not Zero
							break;
						}
					}

					//Cut Zero char If counter is greater than 0
					if (zeroCounter > 0)
						cashText.Remove(tempCashLength, zeroCounter);

					#endregion
				}


				#region Check Next Area

				//Set Next Area
				//Next RightCap index each cut
				int nextRightCapIndex = tempCashLength;  //same as previousLeftIndex
														 //Next LeftSide index each cut
				tempCashLength -= NumberInterval;

				//the Final unit detected
				if (tempCashLength < 1)
				{
					//Attach an unit
					cashText.Insert(nextRightCapIndex, string.Format("{0} ", krLargeNumberNames[i]));
					//And Exit
					break;
				}

				//Search Zero left to right
				int nextZeroCounter = 0;
				for (int x = tempCashLength; x < nextRightCapIndex; x++)
				{
					if (cashText[x] == '0')
					{//Zero is checked
						nextZeroCounter++;
					}
					else
					{//Not Zero
						break;
					}
				}

				#endregion


				#region Attach Measure Sign

				//Attach an unit : whether next Area have a value
				if (nextZeroCounter < NumberInterval)
					cashText.Insert(nextRightCapIndex, string.Format("{0} ", krLargeNumberNames[i]));

				#endregion

				//Cut Zero char If counter is greater than 0
				if (nextZeroCounter > 0)
					cashText.Remove(tempCashLength, nextZeroCounter);
			}

			//Attach a currency unit
			if (unitString != null)
				cashText.Append(unitString);

			return cashText.ToString();
		}

		/// <summary>
		///		Seperate cash string use certain unit to better view (Ten thousand Unit default)
		/// </summary>
		/// <param name="cash">Insert Decimal</param>
		/// <param name="NumberInterval">Interval of division</param>
		/// <param name="unitString">Currency unit (it will not attach the unit, when set null)</param>
		/// <returns></returns>
		public static string convertCashToTenThouWon(decimal cash, int NumberInterval = 4, string unitString = " 원")
		{
			if (NumberInterval < 2)
			{
				throw new ArgumentException("Invaild a interval amount : Set interval value greater than 2");
			}

			cash = decimal.Truncate(cash);

			// BitArray zeroChecker = new BitArray(NumberInterval, false);
			StringBuilder cashText = new StringBuilder(cash.ToString());

			#region Initializer

			// Set cut count
			int cutCount = 0;
			int tempCashLength = cashText.Length - 1;
			cutCount = tempCashLength / NumberInterval;

			if (cutCount == 0)
			{   // O Need No processing! :)

				// Attach a currency unit
				if (unitString != null)
					cashText.Append(unitString);

				return cashText.ToString();
			}

			// For reuse Local var
			tempCashLength++;

			// Clamp cut count Prevent Excess Numberical Char Count
			if (cutCount > krCutLargeNumberNames.Length)
				cutCount = krCutLargeNumberNames.Length;

			//Debug.Log(krLargeNumberNames.Length);

			#endregion

			// Cut string as Numberical Inverval
			for (int i = 0; i < cutCount; i++)
			{
				// First try
				if (i == 0)
				{
					#region Zero Filtering

					// Set Area

					// RightSide index each cut
					int rightCapIndex = tempCashLength;
					// LeftSide index each cut
					tempCashLength -= NumberInterval;

					// Search Zero left to right
					int zeroCounter = 0;
					for (int x = tempCashLength; x < rightCapIndex; x++)
					{
						if (cashText[x] == '0')
						{ // Zero is checked
							zeroCounter++;
						}
						else
						{ // Not Zero
							break;
						}
					}

					// Cut Zero char If counter is greater than 0
					if (zeroCounter > 0)
						cashText.Remove(tempCashLength, zeroCounter);

					#endregion
				}


				#region Check Next Area

				// Set Next Area
				// Next RightCap index each cut
				int nextRightCapIndex = tempCashLength;  // same as previousLeftIndex
														 // Next LeftSide index each cut
				tempCashLength -= NumberInterval;

				// the Final unit detected
				if (tempCashLength < 1)
				{
					//Attach an unit
					cashText.Insert(nextRightCapIndex, string.Format("{0} ", krCutLargeNumberNames[i]));
					//And Exit
					break;
				}

				// Search Zero left to right
				int nextZeroCounter = 0;
				for (int x = tempCashLength; x < nextRightCapIndex; x++)
				{
					if (cashText[x] == '0')
					{// Zero is checked
						nextZeroCounter++;
					}
					else
					{// Not Zero
						break;
					}
				}

				#endregion


				#region Attach Measure Sign

				// Attach an unit : whether next Area have a value
				if (nextZeroCounter < NumberInterval)
					cashText.Insert(nextRightCapIndex, string.Format("{0} ", krCutLargeNumberNames[i]));

				#endregion

				// Cut Zero char If counter is greater than 0
				if (nextZeroCounter > 0)
					cashText.Remove(tempCashLength, nextZeroCounter);
			}

			// Attach a currency unit
			if (unitString != null)
				cashText.Append(unitString);

			return cashText.ToString();
		}
		/// <summary>
		///		Seperate cash string use certain unit to better view (Ten thousand Unit default)
		/// </summary>
		/// <param name="cash">Insert String</param>
		/// <param name="NumberInterval">Interval of division</param>
		/// <param name="unitString">Currency unit (it will not attach the unit, when set null)</param>
		/// <returns></returns>
		public static string convertCashToTenThouWon(string cash, int NumberInterval = 4, string unitString = " 원")
		{
			if (NumberInterval < 2)
			{
				throw new ArgumentException("Invaild a interval amount : Set interval value greater than 2");
			}

			//BitArray zeroChecker = new BitArray(NumberInterval, false);
			StringBuilder cashText = new StringBuilder(cash);

			#region Initializer

			//Set cut count
			int cutCount = 0;
			int tempCashLength = cashText.Length - 1;
			cutCount = tempCashLength / NumberInterval;

			if (cutCount == 0)
			{   //O Need No processing! :)

				//Attach a currency unit
				if (unitString != null)
					cashText.Append(unitString);

				return cashText.ToString();
			}

			//For reuse Local var
			tempCashLength++;

			//Clamp cut count Prevent Excess Numberical Char Count
			if (cutCount > krCutLargeNumberNames.Length)
				cutCount = krCutLargeNumberNames.Length;

			//Debug.Log(krLargeNumberNames.Length);

			#endregion

			//Cut string as Numberical Inverval
			for (int i = 0; i < cutCount; i++)
			{
				//First try
				if (i == 0)
				{
					#region Zero Filtering

					//Set Area

					//RightSide index each cut
					int rightCapIndex = tempCashLength;
					//LeftSide index each cut
					tempCashLength -= NumberInterval;

					//Search Zero left to right
					int zeroCounter = 0;
					for (int x = tempCashLength; x < rightCapIndex; x++)
					{
						if (cashText[x] == '0')
						{//Zero is checked
							zeroCounter++;
						}
						else
						{//Not Zero
							break;
						}
					}

					//Cut Zero char If counter is greater than 0
					if (zeroCounter > 0)
						cashText.Remove(tempCashLength, zeroCounter);

					#endregion
				}


				#region Check Next Area

				//Set Next Area
				//Next RightCap index each cut
				int nextRightCapIndex = tempCashLength;  //same as previousLeftIndex
														 //Next LeftSide index each cut
				tempCashLength -= NumberInterval;

				//the Final unit detected
				if (tempCashLength < 1)
				{
					//Attach an unit
					cashText.Insert(nextRightCapIndex, string.Format("{0} ", krCutLargeNumberNames[i]));
					//And Exit
					break;
				}

				//Search Zero left to right
				int nextZeroCounter = 0;
				for (int x = tempCashLength; x < nextRightCapIndex; x++)
				{
					if (cashText[x] == '0')
					{//Zero is checked
						nextZeroCounter++;
					}
					else
					{//Not Zero
						break;
					}
				}

				#endregion


				#region Attach Measure Sign

				//Attach an unit : whether next Area have a value
				if (nextZeroCounter < NumberInterval)
					cashText.Insert(nextRightCapIndex, string.Format("{0} ", krCutLargeNumberNames[i]));

				#endregion

				//Cut Zero char If counter is greater than 0
				if (nextZeroCounter > 0)
					cashText.Remove(tempCashLength, nextZeroCounter);
			}

			//Attach a currency unit
			if (unitString != null)
				cashText.Append(unitString);

			return cashText.ToString();
		}

		/// <summary>
		///		View Comma-Separated value of BigInteger or view another Char-Separated value user want
		/// </summary>
		/// <param name="totalCash">Insert BigInteger</param>
		/// <param name="NumberInterval">Interval of Separating</param>
		/// <param name="delimiter">Separating character</param>
		/// <returns></returns>
		public static string viewCommaSeparatedValue(BigInteger totalCash, int NumberInterval = 4, char delimiter = ',')
		{
			if (NumberInterval < 2)
			{
				throw new ArgumentException("Invaild a interval amount : Set interval value greater than 2");
			}

			StringBuilder cashText = new StringBuilder(totalCash.ToString());

			//Set cut count
			int cutCount = 0;
			int tempCashLength = cashText.Length - 1;
			cutCount = tempCashLength / NumberInterval;

			//Reuse Local var
			tempCashLength++;

			//Insert divide Char
			for (int i = 0; i < cutCount; i++)
			{
				tempCashLength -= NumberInterval;

				cashText.Insert(tempCashLength, delimiter);
			}

			return cashText.ToString();
		}

		/// <summary>
		///		view Biggest Unit value of BigInteger
		/// </summary>
		/// <param name="totalCash">Insert BigInteger</param>
		/// <param name="NumberInterval">Interval of division</param>
		/// <param name="unitString">Currency unit (it will not attach the unit, when set null)</param>
		/// <returns></returns>
		public static string viewBiggestScaleValue(BigInteger totalCash, int NumberInterval = 4, string unitString = " 원")
		{
			if (NumberInterval < 2)
			{
				throw new ArgumentException("Invaild a interval amount : Set interval value greater than 2");
			}

			StringBuilder cashText = new StringBuilder(totalCash.ToString());

			#region Initializer

			// Set cut count			
			int tempCashLength = cashText.Length - 1;
			int cutCount = tempCashLength / NumberInterval;

			if (cutCount == 0)
			{   // O Need No processing! :)

				// Attach a currency unit
				if (unitString != null)
					cashText.Append(unitString);

				return cashText.ToString();
			}

			// For reuse Local var
			tempCashLength++;

			// Clamp cut count Prevent Excess Numberical Char Count
			if (cutCount > krLargeNumberNames.Length)
				cutCount = krLargeNumberNames.Length;

			#endregion

			// Cut all Except the biggest scale value
			int restOfOthersLength = cutCount * NumberInterval;
			int lastScaleRightCapIndex = tempCashLength - restOfOthersLength;
			cashText.Remove(lastScaleRightCapIndex, restOfOthersLength);

			// Attach Measure Sign
			cashText.Append(krLargeNumberNames[--cutCount]);

			// Attach a currency unit
			if (unitString != null)
				cashText.Append(unitString);

			return cashText.ToString();
		}

		/// <summary>
		///		view Biggest Unit value of Decimal
		/// </summary>
		/// <param name="totalCash">Insert Decimal</param>
		/// <param name="NumberInterval">Interval of division</param>
		/// <param name="unitString">Currency unit (it will not attach the unit, when set null)</param>
		/// <returns></returns>
		public static string viewBiggestScaleValue(decimal totalCash, int NumberInterval = 4, string unitString = " 원")
		{
			if (NumberInterval < 2)
			{
				throw new ArgumentException("Invaild a interval amount : Set interval value greater than 2");
			}

			StringBuilder cashText = new StringBuilder(totalCash.ToString());

			#region Initializer

			// Set cut count			
			int tempCashLength = cashText.Length - 1;
			int cutCount = tempCashLength / NumberInterval;

			if (cutCount == 0)
			{   //O Need No processing! :)

				//Attach a currency unit
				if (unitString != null)
					cashText.Append(unitString);

				return cashText.ToString();
			}

			// For reuse Local var
			tempCashLength++;

			// Clamp cut count Prevent Excess Numberical Char Count
			if (cutCount > krLargeNumberNames.Length)
				cutCount = krLargeNumberNames.Length;

			#endregion

			// Cut all Except the biggest scale value
			int restOfOthersLength = cutCount * NumberInterval;
			int lastScaleRightCapIndex = tempCashLength - restOfOthersLength;
			cashText.Remove(lastScaleRightCapIndex, restOfOthersLength);

			// Attach Measure Sign
			cashText.Append(krLargeNumberNames[--cutCount]);

			// Attach a currency unit
			if (unitString != null)
				cashText.Append(unitString);

			return cashText.ToString();
		}

		/*
		
		********************** Dummy **********************

			if(NumberInterval < 2)
			{
				throw new ArgumentException("Invaild a interval amount ");
			}

			#region Initializer		
			string rawCashText = totalCash.ToString();
			StringBuilder refinedCashText = new StringBuilder();		

			//Set cut count & remain length
			int cutCount = 0;
			int remainLength = 0;
			int tempCashLength = rawCashText.Length;
			cutCount = tempCashLength / NumberInterval;
			remainLength = tempCashLength % NumberInterval;
			if (remainLength > 0)
			{ 
				cutCount++;			
			}
			else
			{
				remainLength = NumberInterval;
			}

			//Clamp cut count Prevent Excess Numberical Char Count
			if (cutCount > krLargeNumberNames.Length)
				cutCount = krLargeNumberNames.Length;
			#endregion

			//Cut string as Numberical Inverval
			for (int i = 0; i < cutCount; i++)
			{
				if(i == 0)
				{//가장 큰 단위 부분 파싱 부
					//일정 간격 자르기
					string numberShard = rawCashText.Substring(0, remainLength);

					//Zero Remover
					int y = 0;
					foreach (char numberAtom in numberShard)
					{
						if(numberAtom == '0')
						{//0 숫자 발견시

						}
						else
						{//0이 아닌 숫자 발견시
							break;
						}

						y++;
					}
				}
				else
				{//Parse others
					rawCashText.Substring(remainLength * i, NumberInterval);
				}
			}

			//Cut All '_' char to null
			//cashText.Replace("_", null);

			//값 단위 부착 부
			refinedCashText.Append("원");

			*/
		#endregion
	}
}
