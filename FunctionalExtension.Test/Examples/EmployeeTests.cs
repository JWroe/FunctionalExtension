using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FunctionalExtension.Core;
using FunctionalExtension.Extensions;
using FunctionalExtension.Types;
using Shouldly;
using Xunit;
using static FunctionalExtension.Core.F;

namespace FunctionalExtension.Test
{
    /// <summary>
    ///     Some tests that also show example usage
    /// </summary>
    public class EmployeeTests
    {
        private static readonly Random Random = new Random();
        private const string WithWorkPermitId = nameof(WithWorkPermitId);
        private const string WithLeavingDateId = nameof(WithLeavingDateId);

        [Theory]
        [InlineData(WithWorkPermitId)]
        [InlineData(WithLeavingDateId)]
        [InlineData("wrong id")]
        public void EmployeeFGetWorkPermit(string id)
        {
            var actual = Employees.ToImmutableDictionary(employee => employee.Id, employee => employee)
                                  .GetWorkPermit(id);

            var expected = Employees.Lookup(emp => emp.Id == id).FlatMap(emp => emp.WorkPermit);

            actual.ShouldBe(expected);
        }

        private static readonly IImmutableList<Employee> Employees = ImmutableList<Employee>
                                                                     .Empty
                                                                     .Add(new Employee(WithWorkPermitId, Random.NextDateTime(), new WorkPermit(Random.NextString(), Random.NextDateTime())))
                                                                     .Add(new Employee(WithLeavingDateId, Random.NextDateTime(), Random.NextDateTime()));
    }

    public static class EmployeeF
    {
        public static Option<WorkPermit> GetWorkPermit(this IDictionary<string, Employee> people, string employeeId) => people.Lookup(employeeId).FlatMap(emp => emp.WorkPermit);
        public static double AverageYearsWorkedAtTheCompany(this IEnumerable<Employee> employees) => 0;
    }

    public class Employee
    {
        public Employee(string id, DateTime joinedOn, WorkPermit workPermit)
            : this(id,
                   joinedOn,
                   workPermit,
                   None())
        {
        }

        public Employee(string id, DateTime joinedOn, DateTime leftOn)
            : this(id,
                   joinedOn,
                   None(),
                   leftOn)
        {
        }

        public Employee(string id, DateTime joinedOn, WorkPermit workPermit, DateTime leftOn)
            : this(id,
                   joinedOn,
                   workPermit.ReturnOption(),
                   leftOn.ReturnOption())
        {
        }

        private Employee(string id, DateTime joinedOn, Option<WorkPermit> workPermit, Option<DateTime> leftOn)
        {
            Id = id;
            WorkPermit = workPermit;
            JoinedOn = joinedOn;
            LeftOn = leftOn;
        }

        public string Id { get; }
        public Option<WorkPermit> WorkPermit { get; }
        public DateTime JoinedOn { get; }
        public Option<DateTime> LeftOn { get; }
    }

    public class WorkPermit : IEquatable<WorkPermit>
    {
        public string Number { get; }
        public DateTime Expiry { get; }

        public WorkPermit(string number, DateTime expiry)
        {
            Number = number;
            Expiry = expiry;
        }

        public bool Equals(WorkPermit other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Number == other.Number && Expiry.Equals(other.Expiry);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WorkPermit)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Number.GetHashCode() * 397) ^ Expiry.GetHashCode();
            }
        }
    }
}