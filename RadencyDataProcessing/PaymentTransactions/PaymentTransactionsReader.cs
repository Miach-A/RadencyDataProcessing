using RadencyDataProcessing.PaymentTransactions.Base;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsReader : PaymentTransactionsReaderBase<string, IAsyncEnumerable<IEnumerable<string>>>
    {
        public override async IAsyncEnumerable<IEnumerable<string>> ReadAsync(string path)
        {
            int chunkSize = 1000;
            int countInChunk = 0;
            StreamReader reader = new(path);
            string? data;
            List<string> chunk = new List<string>(chunkSize);

            while (true)
            {
                data = await reader.ReadLineAsync();
                if (data == null) break;
                countInChunk++;
                chunk.Add(data);

                if (countInChunk == chunkSize)
                {
                    countInChunk = 0;
                    chunk.Clear();
                    yield return chunk;
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }

        }

    }
}
