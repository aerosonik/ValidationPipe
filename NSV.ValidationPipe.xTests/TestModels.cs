using System;
using System.Collections.Generic;

namespace NSV.ValidationPipe.xTests
{
    public class ComplexTestModel
    {
        public string Id { get; set; }
        public List<SimpleTestModel> Models { get; set; }
    }

    public class SimpleTestModel
    {
        public TestModelType ModelType { get; set; }
        public BaseTestField TestValue { get; set; }
        public List<ComplexTestField> ComplexFields { get; set; }

        public DateTime Created { get; set; }
        public TimeSpan Duration { get; set; }
        public string Text { get; set; }
        public byte[] Binary { get; set; }

    }

    public enum TestModelType
    {
        Unknown,
        ModelType1,
        ModelType2,
        ModelType3,
        ModelType4,
        ModelType5,
        ModelType6,
    }

    public class BaseTestField
    {
        public string Code { get; set; }
    }
    public class QuantityTestField : BaseTestField
    {
        public int Count { get; set; }
    }
    public class PeriodTestField: BaseTestField
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    public class DateTestField : BaseTestField
    {
        public DateTime Date { get; set; }
    }
    public class MoneyTestField : BaseTestField
    {
        public Decimal Amount { get; set; }
        public Currency Currency { get; set; }
    }
    public enum Currency
    {
        EUR,
        USD,
        RUB,
        JPY,
        GBR
    }

    public class ComplexTestField
    {
        public string System { get; set; }
        public List<ComplexTestSubField> Subfields { get; set; }
    }
    public class ComplexTestSubField
    {
        public string System { get; set; }
        public int? Version { get; set; }
        public string Code { get; set; }
    }

}
