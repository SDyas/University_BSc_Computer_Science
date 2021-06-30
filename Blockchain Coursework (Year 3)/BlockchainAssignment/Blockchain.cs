using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Blockchain
    {
        // List of block objects forming the blockchain
        public List<Block> blocks;

        // Maximum number of transactions per block
        private int transactionsPerBlock = 3;

        // List of pending transactions to be mined
        public List<Transaction> transactionPool = new List<Transaction>();
        public int transactionType = 0 ; // 0 default, 1 greedy, 2 altruist, 3 random, 4 owner

        // Default Constructor - initialises the list of blocks and generates the genesis block
        public Blockchain()
        {
            blocks = new List<Block>()
            {
                new Block() // Create and append the Genesis Block
            };
        }

        // Prints the block at the specified index to the UI
        public String GetBlockAsString(int index)
        {
            // Check if referenced block exists
            if (index >= 0 && index < blocks.Count)
                return blocks[index].ToString(); // Return block as a string
            else
                return "No such block exists";
        }

        // Retrieves the most recently appended block in the blockchain
        public Block GetLastBlock()
        {
            return blocks[blocks.Count - 1];
        }

        // Retrieves block from blockchain
        public Block GetBlock(int index)
        {
            return blocks[blocks.Count - index];
        }

        // Retrieve pending transactions and remove from pool
        public List<Transaction> GetPendingTransactions(String address) 
        {
        
            // Determine the number of transactions to retrieve dependent on the number of pending transactions and the limit specified
            int n = Math.Min(transactionsPerBlock, transactionPool.Count);

            // "Pull" transactions from the transaction list (modifying the original list)

            // default
            if (transactionType == 0)
            {
                Debug.WriteLine("Using DEFAULT Transaction Selection"); // debug line

                List<Transaction> transactions = transactionPool.GetRange(0, n); // get transactions
                transactionPool.RemoveRange(0, n); // remove from transaction pool

                return transactions; // Return the extracted transactions
            }

            // greedy (highest fee first)
            else if (transactionType == 1)
            {
                Debug.WriteLine("Using GREEDY Transaction Selection"); // debug line

                transactionPool.Sort((x, y) => y.fee.CompareTo(x.fee)); // sort the list by fee (biggest to smallest)
                List<Transaction> transactions = transactionPool.GetRange(0, n); // get transactions by highest fee
                transactionPool.RemoveRange(0, n); // remove from transaction pool

                return transactions; // Return the extracted transactions
            }

            // altruistic (longest wait time first)
            else if (transactionType == 2)
            {
                Debug.WriteLine("USING ALTRUIST Transaction Selection"); // debug line

                transactionPool.Sort((x, y) => x.timestamp.CompareTo(y.timestamp)); // sort the list by timeStamp (earliest to most recent)
                List<Transaction> transactions = transactionPool.GetRange(0, n); // get transactions by earliest creation
                transactionPool.RemoveRange(0, n); // remove from transaction pool

                return transactions; // Return the extracted transactions

            }
            
            // random (random selection of transactions)
            else if (transactionType == 3)
            {
                Debug.WriteLine("USING RANDOM Transaction Selection"); // debug line

                List<Transaction> transactions = new List<Transaction>(); // create empty list for transactions
                var random = new Random(); // random number generator
                for (int i = 0; i < transactionsPerBlock; i++) // for number of transactions per block
                {
                    int index = random.Next(transactionPool.Count); // randomly choose transaction from list
                    transactions.Add(transactionPool[index]); // add transaction to return list
                    transactionPool.Remove(transactionPool[index]); // remove from transaction pool 
                }

                return transactions; // Return the extracted transactions
            }

            // owner (transactions sending to or from the owner selected first)
            else if (transactionType == 4)
            {
                Debug.WriteLine("Using OWNER Transaction Selection"); // debug line

                for (int i = 0; i < transactionPool.Count; i++) // for each transaction in the transactionPool
                {
                    if (transactionPool[i].recipientAddress.Equals(address)) // if transaction recipient is owner
                    {
                        Transaction trans = transactionPool[i];
                        transactionPool.Remove(transactionPool[i]); // remove from transaction pool
                        transactionPool.Insert(0, trans); // reinsert at front of list
                    }
                    else if (transactionPool[i].senderAddress.Equals(address)) // else if transaction sender is owner
                    {
                        Transaction trans = transactionPool[i];
                        transactionPool.Remove(transactionPool[i]); // remove from transaction pool
                        transactionPool.Insert(0, trans); // reinsert at front of list
                    }
                }
                List<Transaction> transactions = transactionPool.GetRange(0, n); // get transactions (any owner transactions will be at the front) 
                transactionPool.RemoveRange(0, n); // remove from transaction pool

                return transactions; // Return the extracted transactions
            }
            else return null; // never returned
        }

        // Check validity of a blocks hash by recomputing the hash and comparing with the mined value
        public static bool ValidateHash(Block b)
        {
            String rehash = b.CreateHash();
            return rehash.Equals(b.hash);
        }

        // Check validity of the merkle root by recalculating the root and comparing with the mined value
        public static bool ValidateMerkleRoot(Block b)
        {
            String reMerkle = Block.MerkleRoot(b.transactionList);
            return reMerkle.Equals(b.merkleRoot);
        }

        // Check the balance associated with a wallet based on the public key
        public double GetBalance(String address)
        {
            // Accumulator value
            double balance = 0;

            // Loop through all approved transactions in order to assess account balance
            foreach(Block b in blocks)
            {
                foreach(Transaction t in b.transactionList)
                {
                    if (t.recipientAddress.Equals(address))
                    {
                        balance += t.amount; // Credit funds recieved
                    }
                    if (t.senderAddress.Equals(address))
                    {
                        balance -= (t.amount + t.fee); // Debit payments placed
                    }
                }
            }
            return balance;
        }

        // Output all blocks of the blockchain as a string
        public override string ToString()
        {
            return String.Join("\n", blocks);
        }
    }
}
