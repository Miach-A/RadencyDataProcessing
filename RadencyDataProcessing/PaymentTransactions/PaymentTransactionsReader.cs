﻿using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsReader : IPaymentTransactionsReader
    {
        public async Task<IEnumerable<IPaymentEntry>> Read(string path)
        {
            throw new NotImplementedException();
        }
    }
}
