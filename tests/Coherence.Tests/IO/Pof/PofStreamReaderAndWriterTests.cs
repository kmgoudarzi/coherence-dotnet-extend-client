/*
 * Copyright (c) 2000, 2020, Oracle and/or its affiliates.
 *
 * Licensed under the Universal Permissive License v 1.0 as shown at
 * http://oss.oracle.com/licenses/upl.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using NUnit.Framework;
using Tangosol.Util;
using IList=System.Collections.IList;

namespace Tangosol.IO.Pof
{
    [TestFixture]
    public class PofStreamReaderAndWriterTests
    {
        [Test]
        public void TestReaderConstructor()
        {
            PofStreamReader psr = new PofStreamReader(new DataReader(new MemoryStream()), new SimplePofContext());
            psr.PofContext = new SimplePofContext();
            Console.WriteLine(psr.UserTypeId);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReaderConstructorWithException()
        {
            PofStreamReader psr = new PofStreamReader(new DataReader(new MemoryStream()), new SimplePofContext());
            psr.PofContext = null;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestReaderReadReminder()
        {
            PofStreamReader psr = new PofStreamReader(new DataReader(new MemoryStream()), new SimplePofContext());
            psr.ReadRemainder();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestReaderVersionIdWithException()
        {
            PofStreamReader psr = new PofStreamReader(new DataReader(new MemoryStream()), new SimplePofContext());
            Console.WriteLine(psr.VersionId);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWriterProperties()
        {
            PofStreamWriter psw = new PofStreamWriter(new DataWriter(new MemoryStream()), new SimplePofContext());
            psw.PofContext = new SimplePofContext();
            Console.WriteLine(psw.UserTypeId);
            psw.PofContext = null;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestWriterVersionIdWithException1()
        {
            PofStreamWriter psw = new PofStreamWriter(new DataWriter(new MemoryStream()), new SimplePofContext());
            Console.WriteLine(psw.VersionId);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestWriterVersionIdWithException2()
        {
            PofStreamWriter psw = new PofStreamWriter(new DataWriter(new MemoryStream()), new SimplePofContext());
            psw.VersionId = 3;
        }

        [Test]
        public void TestGenericCollection()
        {
            MemoryStream           buffer = new MemoryStream(1000);
            DataWriter             writer = new DataWriter(buffer);
            DataReader             reader = new DataReader(buffer);
            ConfigurablePofContext cpc    = new ConfigurablePofContext("config/include-pof-config.xml");
            GenericCollectionsType gct    = new GenericCollectionsType();

            cpc.Serialize(writer, gct);
            buffer.Position = 0;
            cpc.Deserialize(reader);
        }

        [Test]
        public void TestUTF8Serialization()
        {
            // Create a string with multi-bytes character.
            String surrogate = "abc" + Char.ConvertFromUtf32(Int32.Parse("2A601", NumberStyles.HexNumber)) + "def";

            // Safe UTF-8 encoding & decoding of string.
            Stream     stream = new MemoryStream();
            DataWriter writer = new DataWriter(stream);
            byte[]     bytes  = writer.FormatUTF(surrogate);

            Console.WriteLine("Safe UTF-8-encoded code units:");
            foreach (var utf8Byte in bytes)
                Console.Write("{0:X2} ", utf8Byte);
            Console.WriteLine();
            string s = SerializationHelper.ConvertUTF(bytes, 0, bytes.Length);
            Assert.AreEqual(s, surrogate);
        }
    }
}
