using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace MessagePackVsProtobuf
{
    [Serializable]
    [DataContract]
    public class Person
    {
        [DataMember(Order = 0)]
        public virtual int Age { get; set; }

        [DataMember(Order = 1)]
        public virtual string FirstName { get; set; }

        [DataMember(Order = 2)]
        public virtual string LastName { get; set; }

        [DataMember(Order = 3)]
        public virtual Sex Sex { get; set; }

        [DataMember(Order = 4)]
        public virtual IDictionary<string, string> Items { get; set; }
    }

    public class Person2
    {
        public virtual int Age { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual Sex Sex { get; set; }

        public virtual IDictionary<string, string> Items { get; set; }
    }

    public enum Sex
    {
        Unknown,

        Male,

        Female,
    }

    public class Report
    {
        public string Name { get; set; }

        public string TestType { get; set; }

        public double SerializeTime { get; set; }

        public double DeserializeTime { get; set; }

        public double ReSerializeTime { get; set; }

        public long Size { get; set; }
    }

    internal class Program
    {
        private const int _iteration = 10000;

        private const int _times = 3;

        private static bool _dryRun = true;

        private static List<Report> _reports = new List<Report>();

        private const string _filename = "report.json";

        private static void Main(string[] args)
        {
            var p = new Person
            {
                Age = 99,
                FirstName = NextString(6),
                LastName = NextString(6),
                Sex = Sex.Male,
                Items = new Dictionary<string, string> {{NextString(6), NextString(6)}}
            };
            IList<Person> l = Enumerable.Range(1, 1000)
                                        .Select(
                                            x => new Person
                                            {
                                                Age = x,
                                                FirstName = NextString(6),
                                                LastName = NextString(6),
                                                Sex = Sex.Female,
                                                Items = new Dictionary<string, string> {{NextString(6), NextString(6)}}
                                            })
                                        .ToArray();

            #region protobuf-net single

            {
                _dryRun = true;
                SerializeProtobuf(p, null);
                _dryRun = false;
                var reports = new List<Report>();
                for (int i = 0; i < _times; i++)
                {
                    var report = new Report()
                    {
                        TestType = "Single Object",
                        Name = "mgravell/protobuf-net",
                    };
                    var b = SerializeProtobuf(p, report);
                    reports.Add(report);
                }

                var averageReport = new Report
                {
                    TestType = "Single Object",
                    Name = "mgravell/protobuf-net",
                    SerializeTime = reports.Average(report => report.SerializeTime),
                    DeserializeTime = reports.Average(report => report.DeserializeTime),
                    ReSerializeTime = reports.Average(report => report.ReSerializeTime),
                    Size = reports.FirstOrDefault()?.Size ?? 0
                };
                _reports.Add(averageReport);
            }

            #endregion

            #region Official MsgPack-Cli single

            {
                _dryRun = true;
                SerializeMsgPack(p, null);
                _dryRun = false;
                var reports = new List<Report>();
                for (int i = 0; i < _times; i++)
                {
                    var report = new Report()
                    {
                        TestType = "Single Object",
                        Name = "Official MsgPack-Cli",
                    };
                    var b = SerializeMsgPack(p, report);
                    reports.Add(report);
                }

                var averageReport = new Report
                {
                    TestType = "Single Object",
                    Name = "Official MsgPack-Cli",
                    SerializeTime = reports.Average(report => report.SerializeTime),
                    DeserializeTime = reports.Average(report => report.DeserializeTime),
                    ReSerializeTime = reports.Average(report => report.ReSerializeTime),
                    Size = reports.FirstOrDefault()?.Size ?? 0
                };

                _reports.Add(averageReport);
            }

            #endregion

            #region neuecc/MessagePack-CSharp single

            {
                _dryRun = true;
                SerializeMessagePack(p, null);
                _dryRun = false;
                var reports = new List<Report>();
                for (int i = 0; i < _times; i++)
                {
                    var report = new Report()
                    {
                        TestType = "Single Object",
                        Name = "neuecc/MessagePack-CSharp",
                    };
                    var b = SerializeMessagePack(p, report);
                    reports.Add(report);
                }

                var averageReport = new Report
                {
                    TestType = "Single Object",
                    Name = "neuecc/MessagePack-CSharp",
                    SerializeTime = reports.Average(report => report.SerializeTime),
                    DeserializeTime = reports.Average(report => report.DeserializeTime),
                    ReSerializeTime = reports.Average(report => report.ReSerializeTime),
                    Size = reports.FirstOrDefault()?.Size ?? 0
                };

                _reports.Add(averageReport);
            }

            #endregion

            #region Newtonsoft.Json single

            {
                _dryRun = true;
                SerializeNewtonsoftJson(p, null);
                _dryRun = false;
                var reports = new List<Report>();
                for (int i = 0; i < _times; i++)
                {
                    var report = new Report()
                    {
                        TestType = "Single Object",
                        Name = "Newtonsoft.Json",
                    };
                    var b = SerializeNewtonsoftJson(p, report);
                    reports.Add(report);
                }

                var averageReport = new Report
                {
                    TestType = "Single Object",
                    Name = "Newtonsoft.Json",
                    SerializeTime = reports.Average(report => report.SerializeTime),
                    DeserializeTime = reports.Average(report => report.DeserializeTime),
                    ReSerializeTime = reports.Average(report => report.ReSerializeTime),
                    Size = reports.FirstOrDefault()?.Size ?? 0
                };

                _reports.Add(averageReport);
            }

            #endregion

            #region protobuf-net array

            {
                _dryRun = true;
                SerializeProtobuf(l, null);
                _dryRun = false;
                var reports = new List<Report>();
                for (int i = 0; i < _times; i++)
                {
                    var report = new Report()
                    {
                        TestType = "Large Array",
                        Name = "mgravell/protobuf-net",
                    };
                    var b = SerializeProtobuf(l, report);
                    reports.Add(report);
                }

                var averageReport = new Report
                {
                    TestType = "Large Array",
                    Name = "mgravell/protobuf-net",
                    SerializeTime = reports.Average(report => report.SerializeTime),
                    DeserializeTime = reports.Average(report => report.DeserializeTime),
                    ReSerializeTime = reports.Average(report => report.ReSerializeTime),
                    Size = reports.FirstOrDefault()?.Size ?? 0
                };

                _reports.Add(averageReport);
            }

            #endregion

            #region Official MsgPack-Cli array

            {
                _dryRun = true;
                SerializeMsgPack(l, null);
                _dryRun = false;
                var reports = new List<Report>();
                for (int i = 0; i < _times; i++)
                {
                    var report = new Report()
                    {
                        TestType = "Large Array",
                        Name = "Official MsgPack-Cli",
                    };
                    var b = SerializeMsgPack(l, report);
                    reports.Add(report);
                }

                var averageReport = new Report
                {
                    TestType = "Large Array",
                    Name = "Official MsgPack-Cli",
                    SerializeTime = reports.Average(report => report.SerializeTime),
                    DeserializeTime = reports.Average(report => report.DeserializeTime),
                    ReSerializeTime = reports.Average(report => report.ReSerializeTime),
                    Size = reports.FirstOrDefault()?.Size ?? 0
                };

                _reports.Add(averageReport);
            }

            #endregion

            #region neuecc/MessagePack-CSharp array

            {
                _dryRun = true;
                SerializeMessagePack(l, null);
                _dryRun = false;
                var reports = new List<Report>();
                for (int i = 0; i < _times; i++)
                {
                    var report = new Report()
                    {
                        TestType = "Large Array",
                        Name = "neuecc/MessagePack-CSharp",
                    };
                    var b = SerializeMessagePack(l, report);
                    reports.Add(report);
                }

                var averageReport = new Report
                {
                    TestType = "Large Array",
                    Name = "neuecc/MessagePack-CSharp",
                    SerializeTime = reports.Average(report => report.SerializeTime),
                    DeserializeTime = reports.Average(report => report.DeserializeTime),
                    ReSerializeTime = reports.Average(report => report.ReSerializeTime),
                    Size = reports.FirstOrDefault()?.Size ?? 0
                };

                _reports.Add(averageReport);
            }

            #endregion

            #region Newtonsoft.Json array

            {
                _dryRun = true;
                SerializeNewtonsoftJson(l, null);
                _dryRun = false;
                var reports = new List<Report>();
                for (int i = 0; i < _times; i++)
                {
                    var report = new Report()
                    {
                        TestType = "Large Array",
                        Name = "Newtonsoft.Json",
                    };
                    var b = SerializeNewtonsoftJson(l, report);
                    reports.Add(report);
                }

                var averageReport = new Report
                {
                    TestType = "Large Array",
                    Name = "Newtonsoft.Json",
                    SerializeTime = reports.Average(report => report.SerializeTime),
                    DeserializeTime = reports.Average(report => report.DeserializeTime),
                    ReSerializeTime = reports.Average(report => report.ReSerializeTime),
                    Size = reports.FirstOrDefault()?.Size ?? 0
                };

                _reports.Add(averageReport);
            }

            #endregion

            // Without Attribute

            global::MessagePack.MessagePackSerializer.SetDefaultResolver(
                MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            var p2 = new Person2
            {
                Age = 99,
                FirstName = NextString(6),
                LastName = NextString(6),
                Sex = Sex.Male,
                Items = new Dictionary<string, string> {{NextString(6), NextString(6)}}
            };
            IList<Person2> l2 = Enumerable.Range(1, 1000)
                                          .Select(
                                              x => new Person2
                                              {
                                                  Age = x,
                                                  FirstName = NextString(6),
                                                  LastName = NextString(6),
                                                  Sex = Sex.Female,
                                                  Items = new Dictionary<string, string>
                                                  {
                                                      {NextString(6), NextString(6)}
                                                  }
                                              })
                                          .ToArray();

            #region neuecc/MessagePack-CSharp single

            {
                _dryRun = true;
                SerializeMessagePack(p2, null);
                _dryRun = false;
                var reports = new List<Report>();
                for (int i = 0; i < _times; i++)
                {
                    var report = new Report()
                    {
                        TestType = "Single Object",
                        Name = "neuecc/MessagePack-CSharp without Attribute",
                    };
                    var b = SerializeMessagePack(p2, report);
                    reports.Add(report);
                }

                var averageReport = new Report
                {
                    TestType = "Single Object",
                    Name = "neuecc/MessagePack-CSharp without Attribute",
                    SerializeTime = reports.Average(report => report.SerializeTime),
                    DeserializeTime = reports.Average(report => report.DeserializeTime),
                    ReSerializeTime = reports.Average(report => report.ReSerializeTime),
                    Size = reports.FirstOrDefault()?.Size ?? 0
                };

                _reports.Add(averageReport);
            }

            #endregion

            #region neuecc/MessagePack-CSharp array

            {
                _dryRun = true;
                SerializeMessagePack(l2, null);
                _dryRun = false;
                var reports = new List<Report>();
                for (int i = 0; i < _times; i++)
                {
                    var report = new Report()
                    {
                        TestType = "Large Array",
                        Name = "neuecc/MessagePack-CSharp without Attribute",
                    };
                    var b = SerializeMessagePack(l2, report);
                    reports.Add(report);
                }

                var averageReport = new Report
                {
                    TestType = "Large Array",
                    Name = "neuecc/MessagePack-CSharp without Attribute",
                    SerializeTime = reports.Average(report => report.SerializeTime),
                    DeserializeTime = reports.Average(report => report.DeserializeTime),
                    ReSerializeTime = reports.Average(report => report.ReSerializeTime),
                    Size = reports.FirstOrDefault()?.Size ?? 0
                };

                _reports.Add(averageReport);
            }

            #endregion

            File.WriteAllText(_filename, JsonConvert.SerializeObject(_reports));
            Console.WriteLine("Test done");
        }

        private static T SerializeProtobuf<T>(T original, Report report)
        {
            Console.WriteLine(report?.Name);

            T copy = default(T);
            MemoryStream stream = null;

            using (new Measure(Measure.MeasureType.Serialize, report))
            {
                for (int i = 0; i < _iteration; i++)
                {
                    ProtoBuf.Serializer.Serialize<T>(stream = new MemoryStream(), original);
                }
            }

            using (new Measure(Measure.MeasureType.Deserialize, report))
            {
                for (int i = 0; i < _iteration; i++)
                {
                    stream.Position = 0;
                    copy = ProtoBuf.Serializer.Deserialize<T>(stream);
                }
            }

            using (new Measure(Measure.MeasureType.ReSerialize, report))
            {
                for (int i = 0; i < _iteration; i++)
                {
                    ProtoBuf.Serializer.Serialize<T>(stream = new MemoryStream(), copy);
                }
            }

            if (!_dryRun)
            {
                Console.WriteLine(string.Format("{0,15} {1}", "Size of Binary", ToHumanReadableSize(stream.Position)));
                report.Size = stream.Position;
            }

            return copy;
        }

        private static T SerializeMsgPack<T>(T original, Report report)
        {
            Console.WriteLine(report?.Name);

            T copy = default(T);

            MemoryStream stream = null;
            using (new Measure(Measure.MeasureType.Serialize, report))
            {
                for (int i = 0; i < _iteration; i++)
                {
                    MsgPack.Serialization.MessagePackSerializer.Get<T>().Pack(stream = new MemoryStream(), original);
                }
            }

            using (new Measure(Measure.MeasureType.Deserialize, report))
            {
                for (int i = 0; i < _iteration; i++)
                {
                    stream.Position = 0;
                    copy = MsgPack.Serialization.MessagePackSerializer.Get<T>().Unpack(stream);
                }
            }

            using (new Measure(Measure.MeasureType.ReSerialize, report))
            {
                for (int i = 0; i < _iteration; i++)
                {
                    MsgPack.Serialization.MessagePackSerializer.Get<T>().Pack(stream = new MemoryStream(), copy);
                }
            }

            if (!_dryRun)
            {
                Console.WriteLine(string.Format("{0,15} {1}", "Size of Binary", ToHumanReadableSize(stream.Position)));
                report.Size = stream.Position;
            }
            return copy;
        }

        private static T SerializeMessagePack<T>(T original, Report report)
        {
            Console.WriteLine(report?.Name);

            T copy = default(T);
            MemoryStream stream = null;

            using (new Measure(Measure.MeasureType.Serialize, report))
            {
                for (int i = 0; i < _iteration; i++)
                {
                    global::MessagePack.MessagePackSerializer.Serialize(stream = new MemoryStream(), original);
                }
            }

            using (new Measure(Measure.MeasureType.Deserialize, report))
            {
                for (int i = 0; i < _iteration; i++)
                {
                    stream.Position = 0;
                    copy = global::MessagePack.MessagePackSerializer.Deserialize<T>(stream);
                }
            }

            using (new Measure(Measure.MeasureType.ReSerialize, report))
            {
                for (int i = 0; i < _iteration; i++)
                {
                    global::MessagePack.MessagePackSerializer.Serialize(stream = new MemoryStream(), copy);
                }
            }

            if (!_dryRun)
            {
                Console.WriteLine(string.Format("{0,15} {1}", "Size of Binary", ToHumanReadableSize(stream.Position)));
                report.Size = stream.Position;
            }
            return copy;
        }

        private static T SerializeNewtonsoftJson<T>(T original, Report report)
        {
            Console.WriteLine(report?.Name);

            var jsonSerializer = new JsonSerializer();
            T copy = default(T);
            MemoryStream stream = null;

            using (new Measure(Measure.MeasureType.Serialize, report))
            {
                for (int i = 0; i < _iteration; i++)
                {
                    stream = new MemoryStream();
                    using (var tw = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                    using (var jw = new JsonTextWriter(tw))
                    {
                        jsonSerializer.Serialize(jw, original);
                    }
                }
            }

            using (new Measure(Measure.MeasureType.Deserialize, report))
            {
                for (int i = 0; i < _iteration; i++)
                {
                    stream.Position = 0;
                    using (var tr = new StreamReader(stream, Encoding.UTF8, false, 1024, true))
                    using (var jr = new JsonTextReader(tr))
                    {
                        copy = jsonSerializer.Deserialize<T>(jr);
                    }
                }
            }

            using (new Measure(Measure.MeasureType.ReSerialize, report))
            {
                for (int i = 0; i < _iteration; i++)
                {
                    stream = new MemoryStream();
                    using (var tw = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                    using (var jw = new JsonTextWriter(tw))
                    {
                        jsonSerializer.Serialize(jw, copy);
                    }
                }
            }

            if (!_dryRun)
            {
                Console.WriteLine(string.Format("{0,15} {1}", "Size of Binary", ToHumanReadableSize(stream.Position)));
                report.Size = stream.Position;
            }

            return copy;
        }

        private struct Measure : IDisposable
        {
            public enum MeasureType
            {
                Serialize,

                Deserialize,

                ReSerialize
            }

            private readonly MeasureType _measureType;

            private readonly Report _report;

            private readonly Stopwatch _stopwatch;

            public Measure(MeasureType measureType, Report report)
            {
                _measureType = measureType;
                _report = report;
                _stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                if (!_dryRun)
                {
                    Console.WriteLine($"{_measureType.ToString(),15}   {_stopwatch.Elapsed.TotalMilliseconds} ms");
                    switch (_measureType)
                    {
                    case MeasureType.Serialize:
                        _report.SerializeTime = _stopwatch.Elapsed.TotalMilliseconds;
                        break;
                    case MeasureType.Deserialize:
                        _report.DeserializeTime = _stopwatch.Elapsed.TotalMilliseconds;
                        break;
                    case MeasureType.ReSerialize:
                        _report.ReSerializeTime = _stopwatch.Elapsed.TotalMilliseconds;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private static string ToHumanReadableSize(long size)
        {
            return ToHumanReadableSize(new Nullable<long>(size));
        }

        private static string ToHumanReadableSize(long? size)
        {
            if (size == null)
                return "NULL";

            double bytes = size.Value;

            if (bytes <= 1024)
                return bytes.ToString("f2") + " B";

            bytes = bytes / 1024;
            if (bytes <= 1024)
                return bytes.ToString("f2") + " KB";

            bytes = bytes / 1024;
            if (bytes <= 1024)
                return bytes.ToString("f2") + " MB";

            bytes = bytes / 1024;
            if (bytes <= 1024)
                return bytes.ToString("f2") + " GB";

            bytes = bytes / 1024;
            if (bytes <= 1024)
                return bytes.ToString("f2") + " TB";

            bytes = bytes / 1024;
            if (bytes <= 1024)
                return bytes.ToString("f2") + " PB";

            bytes = bytes / 1024;
            if (bytes <= 1024)
                return bytes.ToString("f2") + " EB";

            bytes = bytes / 1024;
            return bytes + " ZB";
        }

        /// <summary>
        /// 生成随机字符串,字典为[A-Za-z0-9]
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        public static string NextString(int length)
        {
            const string dictionary = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int dictCount = dictionary.Length;
            var builder = new StringBuilder(length);
            var bytes = new byte[length * 4];
            RandomNumberGenerator.Create().GetBytes(bytes);
            for (var i = 0; i < length; ++i)
            {
                builder.Append(dictionary[Math.Abs(BitConverter.ToInt32(bytes, i * 4)) % dictCount]);
            }

            return builder.ToString();
        }
    }
}