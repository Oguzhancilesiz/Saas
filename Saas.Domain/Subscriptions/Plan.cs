using Saas.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Domain.Subscriptions
{
    public class Plan : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public decimal Price { get; private set; }
        public int DurationDays { get; private set; }

        private Plan() { }
        public Plan(string name, decimal price, int days)
        {
            Name = name; Price = price; DurationDays = days;
        }
    }

}
