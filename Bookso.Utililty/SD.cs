using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookso.Utililty
{
    public static class SD   // Holds all the static details and constants
    {
        public const string Role_Individual = "Individual";
        public const string Role_Admin = "Admin";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public const string PaymentStatusDelayedPayment = "Delayed";
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved= "Approved";
        public const string PaymentStatusRejected = "Pending";
    }
}

// internal -> for classes that are used within a project (It can only be accesed within the same .net project)