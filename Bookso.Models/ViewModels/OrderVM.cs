using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookso.Models.ViewModels
{
	public class OrderVM
	{
		public OrderHeader OrderHeader { get; set; }
		public IEnumerable<OrderDetail> OrderDetail { get; set; }   // an order can have multiple details -> enumerable
	}
}
