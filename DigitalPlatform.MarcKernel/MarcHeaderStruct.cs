using System;
using System.Diagnostics;
using System.Text;

namespace DigitalPlatform.Marc
{
	// ISO2709ANSIHEADER�ṹ����
	// ISO2709ͷ�����ṹ
	// charset: ����ANSI�ַ����洢���ߴ�̶���������DBCS/UTF-8/MARC-8����
	public class MarcHeaderStruct
	{
		byte[] reclen	= new byte[5];				// ��¼����
		byte[] status	= new byte[1];
		byte[] type		= new byte[1];
		byte[] level	= new byte[1];
		byte[] control	= new byte[1];
		byte[] reserve	= new byte[1];
		byte[] indicount	= new byte[1];			// �ֶ�ָʾ������
		byte[] subfldcodecount	= new byte[1];	// ���ֶα�ʶ������
		byte[] baseaddr	= new byte[5];			// ���ݻ���ַ
		byte[] res1		= new byte[3];
		byte[] lenoffld	= new byte[1];			// Ŀ�������ֶγ��Ȳ���
		byte[] startposoffld	= new byte[1];		// Ŀ�������ֶ���ʼλ�ò���
		byte[] impdef	= new byte[1];				// ʵ���߶��岿��
		byte[] res2		= new byte[1];

		// ����UNIMARC����ǿ�����ISO2709ͷ����
		public int ForceUNIMARCHeader()
		{
			indicount[0] = (byte)'2';
			subfldcodecount[0] = (byte)'2';
			lenoffld[0] = (byte)'4';   // Ŀ�������ֶγ��Ȳ���
			startposoffld[0] = (byte)'5'; // Ŀ�������ֶ���ʼλ�ò���

			return 0;
		}

		public static string StringValue(byte[] baValue)
		{
			Encoding encoding = Encoding.UTF8;
			return encoding.GetString(baValue);
		}

		public static int IntValue(byte[] baValue)
		{
			Encoding encoding = Encoding.UTF8;
			return Convert.ToInt32(encoding.GetString(baValue));
		}

		public static int IntValue(byte[] baValue, 
			int nStart,
			int nLength)
		{
			Encoding encoding = Encoding.UTF8;
			byte[] baTemp = new byte[nLength];
			Array.Copy(baValue, nStart, baTemp, 0, nLength);
			return Convert.ToInt32(encoding.GetString(baTemp));
		}

		// ��¼����
		public int RecLength
		{
			get
			{
				return IntValue(reclen);
			}
			set 
			{
				string strText = Convert.ToString(value);
				strText = strText.PadLeft(reclen.Length, '0');
				reclen = Encoding.UTF8.GetBytes(strText);
			}
		}

		// ��¼���� �ַ���
		public string RecLengthString
		{
			get
			{
				return StringValue(reclen);
			}
		}

		// ���ݻ���ַ
		public int BaseAddress
		{
			get
			{
				return IntValue(baseaddr);
			}
			set 
			{
				string strText = Convert.ToString(value);
				strText = strText.PadLeft(baseaddr.Length, '0');
				baseaddr = Encoding.UTF8.GetBytes(strText);
			}
		}

		// ���ݻ���ַ �ַ���
		public string BaseAddressString
		{
			get
			{
				return StringValue(baseaddr);
			}
		}

		// Ŀ�����б�ʾ�ֶγ���Ҫռ�õ��ַ���
		public int WidthOfFieldLength
		{
			get
			{
				return IntValue(lenoffld);
			}
		}

		// �ַ�����Ŀ�����б�ʾ�ֶγ���Ҫռ�õ��ַ���
		public string WidthOfFieldLengthString
		{
			get
			{
				return StringValue(lenoffld);
			}
		}

		public int WidthOfStartPositionOfField
		{
			get
			{
				return IntValue(startposoffld);
			}
		}
			
		// string�汾
		public string WidthOfStartPositionOfFieldString
		{
			get
			{
				return StringValue(startposoffld);
			}
		}

        public MarcHeaderStruct(Encoding encoding,
            byte[] baRecord)
        {
            if (baRecord.Length < 24)
            {
                throw (new ArgumentException("baRecord���ֽ�������24"));
            }

            bool bUcs2 = false;

            if (encoding != null
                && encoding.Equals(Encoding.Unicode) == true)
                bUcs2 = true;

            if (bUcs2 == true)
            {
                // �Ȱ�baRecordת��ΪANSI���͵Ļ�����
                string strRecord = encoding.GetString(baRecord);

                baRecord = Encoding.ASCII.GetBytes(strRecord);
            }

            Array.Copy(baRecord,
                0,
                reclen, 0,
                5);
            Array.Copy(baRecord,
                5,
                status, 0,
                1);
            Array.Copy(baRecord,
                5 + 1,
                type, 0,
                1);
            Array.Copy(baRecord,
                5 + 1 + 1,
                level, 0,
                1);
            Array.Copy(baRecord,
                5 + 1 + 1 + 1,
                control, 0,
                1);
            Array.Copy(baRecord,
                5 + 1 + 1 + 1 + 1,
                reserve, 0,
                1);
            Array.Copy(baRecord,
                5 + 1 + 1 + 1 + 1 + 1,
                indicount, 0,
                1);
            Array.Copy(baRecord,
                5 + 1 + 1 + 1 + 1 + 1 + 1,
                subfldcodecount, 0,
                1);
            Array.Copy(baRecord,
                5 + 1 + 1 + 1 + 1 + 1 + 1 + 1,
                baseaddr, 0,
                5);
            Array.Copy(baRecord,
                5 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 5,
                res1, 0,
                3);
            Array.Copy(baRecord,
                5 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 5 + 3,
                lenoffld, 0,
                1);
            Array.Copy(baRecord,
                5 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 5 + 3 + 1,
                startposoffld, 0,
                1);
            Array.Copy(baRecord,
                5 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 5 + 3 + 1 + 1,
                impdef, 0,
                1);
            Array.Copy(baRecord,
                5 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 5 + 3 + 1 + 1 + 1,
                res2, 0,
                1);
        }

		public MarcHeaderStruct(byte[] baRecord)
		{
			if (baRecord.Length < 24) 
			{
				throw(new Exception("baRecord���ֽ�������24"));
			}

            bool bUcs2 = false;
            if (baRecord[0] == 0
                || baRecord[1] == 0)
            {
                bUcs2 = true;
            }

            if (bUcs2 == true)
            {
                throw new Exception("Ӧ�ù��캯����Ӧ��һ���汾������֧��UCS2���뷽ʽ");
            }

			Array.Copy(baRecord,
				0,
				reclen,	0,
				5);
			Array.Copy(baRecord,
				5,
				status, 0,
				1);
			Array.Copy(baRecord,
				5 + 1,
				type, 0,
				1);
			Array.Copy(baRecord,
				5 + 1 + 1,
				level, 0,
				1);
			Array.Copy(baRecord,
				5 + 1 + 1 + 1,
				control, 0,
				1);
			Array.Copy(baRecord,
				5 + 1 + 1 + 1 + 1,
				reserve, 0,
				1);
			Array.Copy(baRecord,
				5 + 1 + 1 + 1 + 1 + 1,
				indicount, 0,
				1);
			Array.Copy(baRecord,
				5 + 1 + 1 + 1 + 1 + 1 + 1,
				subfldcodecount, 0,
				1);
			Array.Copy(baRecord,
				5 + 1 + 1 + 1 + 1 + 1 + 1 + 1,
				baseaddr, 0,
				5);
			Array.Copy(baRecord,
				5 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 5,
				res1, 0,
				3);
			Array.Copy(baRecord,
				5 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 5 + 3,
				lenoffld, 0,
				1);
			Array.Copy(baRecord,
				5 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 5 + 3 + 1,
				startposoffld, 0,
				1);
			Array.Copy(baRecord,
				5 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 5 + 3 + 1 + 1,
				impdef, 0,
				1);
			Array.Copy(baRecord,
				5 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 5 + 3 + 1 + 1 + 1,
				res2, 0,
				1);
		}

		public byte[] GetBytes()
		{
			byte [] baResult = null;

			baResult = ByteArray.Add(baResult, reclen);	// 5
			baResult = ByteArray.Add(baResult, status);	// 1
			baResult = ByteArray.Add(baResult, type);	// 1
			baResult = ByteArray.Add(baResult, level);	// 1
			baResult = ByteArray.Add(baResult, control);	// 1
			baResult = ByteArray.Add(baResult, reserve);	// 1
			baResult = ByteArray.Add(baResult, indicount);	// 1
			baResult = ByteArray.Add(baResult, subfldcodecount);	// 1
			baResult = ByteArray.Add(baResult, baseaddr);	// 5
			baResult = ByteArray.Add(baResult, res1);	// 3
			baResult = ByteArray.Add(baResult, lenoffld);	// 1
			baResult = ByteArray.Add(baResult, startposoffld);	// 1
			baResult = ByteArray.Add(baResult, impdef);	// 1
			baResult = ByteArray.Add(baResult, res2);	// 1

			Debug.Assert(baResult.Length == 24, "ͷ�������ݱ���Ϊ24�ַ�");
			if (baResult.Length != 24)
				throw(new Exception("MarcHeader.GetBytes() error"));

            // 2014/5/9
            // ����ͷ�������� 0 �ַ�
            for (int i = 0; i < baResult.Length; i++)
            {
                if (baResult[i] == 0)
                    baResult[i] = (byte)'*';
            }

			return baResult;
		}

        // 2015/5/10
        public MyByteList GetByteList()
        {
            MyByteList list = new MyByteList(24);

            list.AddRange(reclen);	// 5
            list.AddRange(status);	// 1
            list.AddRange(type);	// 1
            list.AddRange(level);	// 1
            list.AddRange(control);	// 1
            list.AddRange(reserve);	// 1
            list.AddRange(indicount);	// 1
            list.AddRange(subfldcodecount);	// 1
            list.AddRange(baseaddr);	// 5
            list.AddRange(res1);	// 3
            list.AddRange(lenoffld);	// 1
            list.AddRange(startposoffld);	// 1
            list.AddRange(impdef);	// 1
            list.AddRange(res2);	// 1

            Debug.Assert(list.Count == 24, "ͷ�������ݱ���Ϊ24�ַ�");
            if (list.Count != 24)
                throw (new Exception("MarcHeader.GetBytes() error"));

            // 2014/5/9
            // ����ͷ�������� 0 �ַ�
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == 0)
                    list[i] = (byte)'*';
            }

            return list;
        }
#if NO
        // 2015/5/10 �Ż�
        public byte[] GetBytes()
        {
            MyByteList list = new MyByteList(24);

            list.AddRange(reclen);	// 5
            list.AddRange(status);	// 1
            list.AddRange(type);	// 1
            list.AddRange(level);	// 1
            list.AddRange(control);	// 1
            list.AddRange(reserve);	// 1
            list.AddRange(indicount);	// 1
            list.AddRange(subfldcodecount);	// 1
            list.AddRange(baseaddr);	// 5
            list.AddRange(res1);	// 3
            list.AddRange(lenoffld);	// 1
            list.AddRange(startposoffld);	// 1
            list.AddRange(impdef);	// 1
            list.AddRange(res2);	// 1

            Debug.Assert(list.Count == 24, "ͷ�������ݱ���Ϊ24�ַ�");
            if (list.Count != 24)
                throw (new Exception("MarcHeader.GetBytes() error"));

            // 2014/5/9
            // ����ͷ�������� 0 �ַ�
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == 0)
                    list[i] = (byte)'*';
            }

            return list.GetByteArray();
        }
#endif
	}

}
