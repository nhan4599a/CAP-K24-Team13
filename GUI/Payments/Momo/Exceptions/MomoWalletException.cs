using System;

namespace GUI.Payments.Momo.Exceptions
{
    public class MomoWalletException : Exception
    {
        public string Field { get; set; }
    }
}
