using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Domain.OrderAggregate
{
    [Owned]
    public class Address : ValueObject
    {
        /// <summary>
        /// 街道
        /// </summary>
        [StringLength(200)]
        public string Street { get; private set; }

        /// <summary>
        /// 成是
        /// </summary>
        [StringLength(200)]
        public string City { get; private set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [StringLength(200)]
        public string ZipCode { get; private set; }

        /// <summary>
        /// 构造
        /// </summary>
        public Address()
        {
        }

        public Address(string street, string city, string zipcode)
        {
            Street = street;
            City = city;
            ZipCode = zipcode;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            yield return Street;
            yield return City;
            yield return ZipCode;
        }
    }
}