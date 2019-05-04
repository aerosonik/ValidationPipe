using System;
using System.Collections.Generic;
using System.Text;

namespace NSV.ValidationPipe.xTests
{
    public class TestData
    {
        public static ComplexTestModel GetValidModel()
        {
            return new ComplexTestModel
            {
                Id = TestConst.Guid_1,
                Models = GetValidSimpleModels()
            };
        }

        public static ComplexTestModel GetInvalidModel()
        {
            return new ComplexTestModel
            {

            };
        }

        private static List<SimpleTestModel> GetValidSimpleModels()
        {
            var result = new List<SimpleTestModel>();

            for (int i = 0; i < 4; i++)
            {
                var item = new SimpleTestModel
                {
                    ModelType = Enum.Parse<TestModelType>(i + 1.ToString()),

                };
                result.Add(item);
            }

            return result;
        }

        private static BaseTestField GetBaseTestField(int index)
        {
            if (index > _baseTestFields.Length - 1)
                return null;

            return _baseTestFields[index];
        }
        private static BaseTestField[] _baseTestFields = new BaseTestField[]
        {
            new QuantityTestField
            {
                Count = 20,
                Code = "Code"
            },
            new PeriodTestField
            {
                From = DateTime.Now.AddDays(-5),
                To = DateTime.Now.AddDays(5),
                Code = "Code"
            },
            new DateTestField
            {
                Date = DateTime.Now,
                Code = "Code"
            },
            new MoneyTestField
            {
                Amount = 100.87M,
                Currency = Currency.RUB,
                Code = "Code"
            }
        };
    }
}
