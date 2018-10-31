using System;
using System.Collections;
using NUnit.Framework;

namespace HomeExercises
{
	public class NumberValidatorTests
	{
		[TestCase(-3, 2, true, TestName = "WhenPrecisionIsNegative")]
		[TestCase(0, 2, true, TestName = "WhenPrecisionIsZero")]
		[TestCase(3, -2, true, TestName = "WhenScaleIsNegative")]
		[TestCase(1, 2, true, TestName = "WhenScaleIsLarger_ThanPrecision")]
		[TestCase(2, 2, true, TestName = "WhenScaleIsSame_AsPrecision")]
		public void Constructor_ShouldFail(int precision, int scale, bool onlyPositive)
		{
			Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale, onlyPositive));
		}

		private static IEnumerable ScaleTestCases
		{
			get
			{
				yield return new TestCaseData(new NumberValidator(17, 2, true), "0.0")
					.Returns(true)
					.SetName("HasZeroScalePart");
				yield return new TestCaseData(new NumberValidator(17, 2, true), "0.4")
					.Returns(true)
					.SetName("HasNonZeroScalePart");
				yield return new TestCaseData(new NumberValidator(17, 2, true), "0.42")
					.Returns(true)
					.SetName("ScalePartLengthIsTheSameAsExpected");
			}
		}
		
		[Test, TestCaseSource(nameof(ScaleTestCases))]
		public bool IsValidNumber_TestScale(NumberValidator validator, string representation)
		{
			return validator.IsValidNumber(representation);
		}

		private static IEnumerable PrecisionTestCases
		{
			get
			{
				yield return new TestCaseData(new NumberValidator(17, 2, true), "0")
					.Returns(true)
					.SetName("HasNotFractionalPart");
				yield return new TestCaseData(new NumberValidator(2, 1, true), "10")
					.Returns(true)
					.SetName("HasLengthSameAsPrecision");
				yield return new TestCaseData(new NumberValidator(2, 1, true), "1.1")
					.Returns(true)
					.SetName("IntegerAndFractionPartLengthSameAsPrecision");
				yield return new TestCaseData(new NumberValidator(4, 2, true), "+1.23")
					.Returns(true)
					.SetName("HasFractionalPartAndPlusSign");
			}
		}
		
		[Test, TestCaseSource(nameof(PrecisionTestCases))]
		public bool IsValidNumber_TestPrecision(NumberValidator validator, string representation)
		{
			return validator.IsValidNumber(representation);
		}

		private static IEnumerable NegativeSignTestCases
		{
			get
			{
				yield return new TestCaseData(new NumberValidator(4, 2, false), "-1.23")
					.Returns(true)
					.SetName("HasFractionalPart");
				yield return new TestCaseData(new NumberValidator(4, 2, false), "-1")
					.Returns(true)
					.SetName("HasNotFractionalPart");
				yield return new TestCaseData(new NumberValidator(4, 2, false), "-001")
					.Returns(true)
					.SetName("HasZerosBetweenSignAndNonZeroNumber");
			}
		}
		
		[Test, TestCaseSource(nameof(NegativeSignTestCases))]
		public bool IsValidNumber_TestNegativeNumbers(NumberValidator validator, string representation)
		{
			return validator.IsValidNumber(representation);
		}
		
		private static IEnumerable IncorrectValidatorInputTestCases
		{
			get
			{
				yield return new TestCaseData(new NumberValidator(4, 2, true), "-1.23")
					.Returns(false)
					.SetName("WhenOnlyPositiveAndSignIsMinus");
				yield return new TestCaseData(new NumberValidator(3, 2, false), "-1.23")
					.Returns(false)
					.SetName("WhenPrecisionIsSameAsExpectedButValueIsSigned");
				yield return new TestCaseData(new NumberValidator(17, 2, true), "a.1d")
					.Returns(false)
					.SetName("WhenNumberStringHasNotOnlyNumbers");
				yield return new TestCaseData(new NumberValidator(17, 2, true), "1.230")
					.Returns(false)
					.SetName("ScaleIsLargerThanExpected");
			}
		}
		
		[Test, TestCaseSource(nameof(IncorrectValidatorInputTestCases))]
		public bool IsValidNumber_TestIncorrectValidatorInputs(NumberValidator validator, string representation)
		{
			return validator.IsValidNumber(representation);
		}
	}
}