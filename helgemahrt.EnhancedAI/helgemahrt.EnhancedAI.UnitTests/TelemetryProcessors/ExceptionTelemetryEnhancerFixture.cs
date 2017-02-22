using helgemahrt.EnhancedAI.TelemetryProcessors;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace helgemahrt.EnhancedAI.UnitTests.TelemetryProcessors
{
    [TestClass]
    public class ExceptionTelemetryEnhancerFixture
    {
        class CustomPropertyException : Exception
        {
            public string CustomProperty { get; set; }

            public CustomPropertyException(string message, string customProperty) : base(message)
            {
                CustomProperty = customProperty;
            }
        }

        [TestMethod]
        public void TestProcess_CustomProperty()
        {
            using (ShimsContext.Create())
            {
                // arrange
                string actual = string.Empty;
                string expected = "\"Important Property\"";
                ExceptionTelemetryEnhancer sut = new ExceptionTelemetryEnhancer(new MockTransmissionProcessor((x) =>
                {
                    ExceptionTelemetry t = x as ExceptionTelemetry;
                    if (t != null)
                    {
                        if (t.Properties.ContainsKey("CustomPropertyException.CustomProperty"))
                        {
                            actual = t.Properties["CustomPropertyException.CustomProperty"];
                        }
                    }
                }));
                CustomPropertyException ex = new CustomPropertyException("Exception Message", "Important Property");
                ExceptionTelemetry telemetry = new ExceptionTelemetry(ex);

                // act
                sut.Process(telemetry);

                // assert
                Assert.AreEqual(expected, actual);
            }
        }

        class TaskPropertyException : Exception
        {
            public Task CustomTask { get; set; }
            public TaskPropertyException(string message, Task task) : base(message)
            {
                CustomTask = task;
            }
        }

        [TestMethod]
        public void TestProcess_IgnoresTaskBaseClass()
        {
            using (ShimsContext.Create())
            {
                // arrange
                bool actual = true;
                bool expected = false;
                ExceptionTelemetryEnhancer sut = new ExceptionTelemetryEnhancer(new MockTransmissionProcessor((x) =>
                {
                    ExceptionTelemetry t = x as ExceptionTelemetry;
                    if (t != null)
                    {
                        actual = t.Properties.ContainsKey("TaskPropertyException.CustomTask");
                    }
                }));
                TaskPropertyException ex = new TaskPropertyException("Exception Message", Task.Delay(1000));
                ExceptionTelemetry telemetry = new ExceptionTelemetry(ex);

                // act
                sut.Process(telemetry);

                // assert
                Assert.AreEqual(expected, actual);
            }
        }

        class TemplateTaskPropertyException : Exception
        {
            public Task<int> CustomTask { get; set; }
            public TemplateTaskPropertyException(string message, Task<int> task) : base(message)
            {
                CustomTask = task;
            }
        }

        [TestMethod]
        public void TestProcess_IgnoresTaskTemplateClass()
        {
            using (ShimsContext.Create())
            {
                // arrange
                bool actual = true;
                bool expected = false;
                ExceptionTelemetryEnhancer sut = new ExceptionTelemetryEnhancer(new MockTransmissionProcessor((x) =>
                {
                    ExceptionTelemetry t = x as ExceptionTelemetry;
                    if (t != null)
                    {
                        actual = t.Properties.ContainsKey("TaskPropertyException.CustomTask");
                    }
                }));
                TemplateTaskPropertyException ex = new TemplateTaskPropertyException("Exception Message", new Task<int>(() => { return 5; }));
                ExceptionTelemetry telemetry = new ExceptionTelemetry(ex);

                // act
                sut.Process(telemetry);

                // assert
                Assert.AreEqual(expected, actual);
            }
        }

        class CollectionPropertyException : Exception
        {
            public List<int> CustomList { get; set; }
            public CollectionPropertyException(string message, List<int> list) : base(message)
            {
                CustomList = list;
            }
        }

        [TestMethod]
        public void TestProcess_Collection()
        {
            using (ShimsContext.Create())
            {
                // arrange
                string actual = string.Empty;
                string expected = "[1,2,3,4,5]";
                ExceptionTelemetryEnhancer sut = new ExceptionTelemetryEnhancer(new MockTransmissionProcessor((x) =>
                {
                    ExceptionTelemetry t = x as ExceptionTelemetry;
                    if (t != null)
                    {
                        if (t.Properties.ContainsKey("CollectionPropertyException.CustomList"))
                        {
                            actual = t.Properties["CollectionPropertyException.CustomList"];
                        }
                    }
                }));
                CollectionPropertyException ex = new CollectionPropertyException("Exception Message", new List<int>()
                {
                    1, 2, 3, 4, 5
                });
                ExceptionTelemetry telemetry = new ExceptionTelemetry(ex);

                // act
                sut.Process(telemetry);

                // assert
                Assert.AreEqual(expected, actual);
            }
        }

        class ArrayPropertyException : Exception
        {
            public int[] CustomArray { get; set; }
            public ArrayPropertyException(string message, int[] array) : base(message)
            {
                CustomArray = array;
            }
        }

        [TestMethod]
        public void TestProcess_Array()
        {
            using (ShimsContext.Create())
            {
                // arrange
                string actual = string.Empty;
                string expected = "[1,2,3,4,5]";
                ExceptionTelemetryEnhancer sut = new ExceptionTelemetryEnhancer(new MockTransmissionProcessor((x) =>
                {
                    ExceptionTelemetry t = x as ExceptionTelemetry;
                    if (t != null)
                    {
                        if (t.Properties.ContainsKey("ArrayPropertyException.CustomArray"))
                        {
                            actual = t.Properties["ArrayPropertyException.CustomArray"];
                        }
                    }
                }));
                ArrayPropertyException ex = new ArrayPropertyException("Exception Message", new int[]
                {
                    1, 2, 3, 4, 5
                });
                ExceptionTelemetry telemetry = new ExceptionTelemetry(ex);

                // act
                sut.Process(telemetry);

                // assert
                Assert.AreEqual(expected, actual);
            }
        }

        class BaseTaskCollectionPropertyException : Exception
        {
            public List<Task> TaskList { get; set; }
            public BaseTaskCollectionPropertyException(string message, List<Task> taskList) : base(message)
            {
                TaskList = taskList;
            }
        }

        [TestMethod]
        public void TestProcess_IgnoresBaseTaskCollection()
        {
            using (ShimsContext.Create())
            {
                // arrange
                bool actual = true;
                bool expected = false;
                ExceptionTelemetryEnhancer sut = new ExceptionTelemetryEnhancer(new MockTransmissionProcessor((x) =>
                {
                    ExceptionTelemetry t = x as ExceptionTelemetry;
                    if (t != null)
                    {
                        actual = t.Properties.ContainsKey("BaseTaskCollectionPropertyException.TaskList");
                    }
                }));
                BaseTaskCollectionPropertyException ex = new BaseTaskCollectionPropertyException("Exception Message", new List<Task>()
                {
                    Task.Delay(1000),
                    Task.Delay(1000),
                    Task.Delay(1000),
                    Task.Delay(1000)
                });
                ExceptionTelemetry telemetry = new ExceptionTelemetry(ex);

                // act
                sut.Process(telemetry);

                // assert
                Assert.AreEqual(expected, actual);
            }
        }

        class TemplateTaskCollectionPropertyException : Exception
        {
            public List<Task<int>> TaskList { get; set; }
            public TemplateTaskCollectionPropertyException(string message, List<Task<int>> taskList) : base(message)
            {
                TaskList = taskList;
            }
        }

        [TestMethod]
        public void TestProcess_IgnoresTemplateTaskCollection()
        {
            using (ShimsContext.Create())
            {
                // arrange
                bool actual = true;
                bool expected = false;
                ExceptionTelemetryEnhancer sut = new ExceptionTelemetryEnhancer(new MockTransmissionProcessor((x) =>
                {
                    ExceptionTelemetry t = x as ExceptionTelemetry;
                    if (t != null)
                    {
                        actual = t.Properties.ContainsKey("TemplateTaskCollectionPropertyException.TaskList");
                    }
                }));
                TemplateTaskCollectionPropertyException ex = new TemplateTaskCollectionPropertyException("Exception Message", new List<Task<int>>()
                {
                    new Task<int>(() => { return 5; }),
                    new Task<int>(() => { return 5; }),
                    new Task<int>(() => { return 5; }),
                    new Task<int>(() => { return 5; })
                });
                ExceptionTelemetry telemetry = new ExceptionTelemetry(ex);

                // act
                sut.Process(telemetry);

                // assert
                Assert.AreEqual(expected, actual);
            }
        }

        class BaseTaskArrayPropertyException : Exception
        {
            public Task[] TaskArray { get; set; }
            public BaseTaskArrayPropertyException(string message, Task[] taskArray) : base(message)
            {
                TaskArray = taskArray;
            }
        }

        [TestMethod]
        public void TestProcess_IgnoresBaseTaskArray()
        {
            using (ShimsContext.Create())
            {
                // arrange
                bool actual = true;
                bool expected = false;
                ExceptionTelemetryEnhancer sut = new ExceptionTelemetryEnhancer(new MockTransmissionProcessor((x) =>
                {
                    ExceptionTelemetry t = x as ExceptionTelemetry;
                    if (t != null)
                    {
                        actual = t.Properties.ContainsKey("BaseTaskArrayPropertyException.TaskArray");
                    }
                }));
                BaseTaskArrayPropertyException ex = new BaseTaskArrayPropertyException("Exception Message", new Task[]
                {
                    Task.Delay(1000),
                    Task.Delay(1000),
                    Task.Delay(1000),
                    Task.Delay(1000)
                });
                ExceptionTelemetry telemetry = new ExceptionTelemetry(ex);

                // act
                sut.Process(telemetry);

                // assert
                Assert.AreEqual(expected, actual);
            }
        }

        class TemplateTaskArrayPropertyException : Exception
        {
            public Task<int>[] TaskArray { get; set; }
            public TemplateTaskArrayPropertyException(string message, Task<int>[] taskArray) : base(message)
            {
                TaskArray = taskArray;
            }
        }

        [TestMethod]
        public void TestProcess_IgnoresTemplateTaskArray()
        {
            using (ShimsContext.Create())
            {
                // arrange
                bool actual = true;
                bool expected = false;
                ExceptionTelemetryEnhancer sut = new ExceptionTelemetryEnhancer(new MockTransmissionProcessor((x) =>
                {
                    ExceptionTelemetry t = x as ExceptionTelemetry;
                    if (t != null)
                    {
                        actual = t.Properties.ContainsKey("TemplateTaskArrayPropertyException.TaskArray");
                    }
                }));
                TemplateTaskArrayPropertyException ex = new TemplateTaskArrayPropertyException("Exception Message", new Task<int>[]
                {
                    new Task<int>(() => { return 5; }),
                    new Task<int>(() => { return 5; }),
                    new Task<int>(() => { return 5; }),
                    new Task<int>(() => { return 5; })
                });
                ExceptionTelemetry telemetry = new ExceptionTelemetry(ex);

                // act
                sut.Process(telemetry);

                // assert
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
