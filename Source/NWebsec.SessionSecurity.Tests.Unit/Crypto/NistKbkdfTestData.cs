// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace NWebsec.SessionSecurity.Tests.Unit.Crypto
{
    public class NistKbkdfTestData : IEnumerable
    {
        private static IEnumerator ReadTestDataFromFile()
        {
            using (var reader = new StreamReader("KDFCTR_gen.txt"))
            {

                var line = reader.ReadLine();
                while (line != null)
                {
                    if (!line.Equals("[PRF=HMAC_SHA256]"))
                    {
                        line = reader.ReadLine();
                        continue;
                    }
                    line = reader.ReadLine();
                    if (line == null || !line.Equals("[CTRLOCATION=BEFORE_FIXED]"))
                    {
                        line = reader.ReadLine();
                        continue;
                    }
                    line = reader.ReadLine();
                    if (line != null && line.Equals("[RLEN=8_BITS]"))
                    {
                        //We're there!
                        break;
                    }
                }
                //Now get Data
                var testData = new List<ITestCaseData>();

                line = reader.ReadLine();
                while (line != null && !line.StartsWith("["))
                {
                    if (line.Equals(String.Empty))
                    {
                        line = reader.ReadLine();
                        continue;
                    }

                    //Read a data chunk
                    var dataChunk = new HmacSha256CtrData();
                    var field = line.Split('=');
                    if (!field[0].Trim().Equals("COUNT")) throw new ApplicationException("Expected COUNT, but got: " + field[0]);
                    dataChunk.Count = Int32.Parse(field[1].Trim());
                    // ReSharper disable PossibleNullReferenceException
                    line = reader.ReadLine();
                    field = line.Split('=');
                    if (!field[0].Trim().Equals("L")) throw new ApplicationException("Expected L, but got: " + field[0]);
                    dataChunk.KeyLength = Int32.Parse(field[1].Trim());

                    line = reader.ReadLine();
                    field = line.Split('=');
                    if (!field[0].Trim().Equals("KI")) throw new ApplicationException("Expected KI, but got: " + field[0]);
                    dataChunk.Ki = field[1].Trim();

                    reader.ReadLine(); //Skip FixedInputDataByteLen 

                    line = reader.ReadLine();
                    field = line.Split('=');
                    if (!field[0].Trim().Equals("FixedInputData")) throw new ApplicationException("Expected FixedInputData, but got: " + field[0]);
                    dataChunk.Input = field[1].Trim();

                    //Skip binary rep details
                    do
                    {
                        line = reader.ReadLine();
                    } while (line.StartsWith("\t"));

                    field = line.Split('=');
                    if (!field[0].Trim().Equals("KO")) throw new ApplicationException("Expected KO , but got: " + field[0]);
                    dataChunk.Ko = field[1].Trim();

                    // ReSharper enable PossibleNullReferenceException
                    line = reader.ReadLine();
                    testData.Add(dataChunk);
                    //rince and repeat.
                }

                return testData.GetEnumerator();
            }
        }

        public IEnumerator GetEnumerator()
        {
            return ReadTestDataFromFile();
        }
    }

    public class HmacSha256CtrData : ITestCaseData
    {
        internal int Count { get; set; }
        internal int KeyLength { get; set; }
        internal string Ki { get; set; }
        internal string Input { get; set; }
        internal string Ko { get; set; }

        public object[] Arguments
        {
            get { return new object[] { KeyLength, Ki, Input, Ko }; }
        }

        public object Result { get; private set; }

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        public bool HasExpectedResult { get; private set; }

        public Type ExpectedException { get; private set; }
        public string ExpectedExceptionName { get; private set; }

        public string TestName
        {
            get { return String.Format("NIST KDF HMAC_SHA256 Ctr validation #{0:d2}",Count); }
            private set { throw new NotImplementedException(); }
        }

        public string Description { get; private set; }
        public bool Ignored { get; private set; }
        public bool Explicit { get; private set; }
        public string IgnoreReason { get; private set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}
