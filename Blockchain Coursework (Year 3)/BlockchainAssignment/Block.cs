using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace BlockchainAssignment
{
    class Block
    {

        /* Block Variables */
        private DateTime timestamp; // Time of creation

        private int index; // Position of the block in the sequence of blocks
        public static int difficulty { get; set; } = 4; // An arbitrary number of 0's to proceed a hash value

        public String prevHash, // A reference pointer to the previous block
            hash, // The current blocks "identity"
            merkleRoot,  // The merkle root of all transactions in the block
            minerAddress; // Public Key (Wallet Address) of the Miner

        public List<Transaction> transactionList; // List of transactions in this block
        
        // Proof-of-work
        public long nonce; // Number used once for Proof-of-Work and mining
        static object threadLock = new object(); // object to lock threads
        public float mineTime; // time taken to mine

        // Rewards
        public double reward; // Simple fixed reward established by "Coinbase"

        /* Genesis block constructor */
        public Block()
        {
            timestamp = DateTime.Now;
            index = 0;
            transactionList = new List<Transaction>();
            hash = Mine().Item1;
            mineTime = Mine().Item2;
        }

        /* New Block constructor */
        public Block(Block lastBlock, List<Transaction> transactions, String minerAddress)
        {
            timestamp = DateTime.Now;

            index = lastBlock.index + 1;
            prevHash = lastBlock.hash;

            this.minerAddress = minerAddress; // The wallet to be credited the reward for the mining effort
            reward = 1.0; // Assign a simple fixed value reward
            transactions.Add(createRewardTransaction(transactions)); // Create and append the reward transaction
            transactionList = new List<Transaction>(transactions); // Assign provided transactions to the block

            merkleRoot = MerkleRoot(transactionList); // Calculate the merkle root of the blocks transactions
            hash = Mine().Item1; // Conduct PoW to create a hash which meets the given difficulty requirement
            mineTime = Mine().Item2;
        }

        /* Hashes the entire Block object */
        public String CreateHash()
        {
            String hash = String.Empty;
            SHA256 hasher = SHA256Managed.Create();

            /* Concatenate all of the blocks properties including nonce as to generate a new hash on each call */
            String input = timestamp.ToString() + index + prevHash + nonce + merkleRoot;

            /* Apply the hash function to the block as represented by the string "input" */
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            /* Reformat to a string */
            foreach (byte x in hashByte)
                hash += String.Format("{0:x2}", x);
            
            return hash;
        }

        // Create a Hash which satisfies the difficulty level required for PoW
        public Tuple<string,float> Mine()
        {
            nonce = 0; // Initalise the nonce
            String hash = CreateHash(); // Hash the block
            String re = new string('0', difficulty); // A string for analysing the PoW requirement

            var timer = new Stopwatch(); // timer to see how long it takes to find correct nonce
            timer.Start(); // starts the timer
            for (int threadCount = 0; threadCount < 2; threadCount++) // create threads
            {
                Thread thread = new Thread(delegate ()
                {
                    findNonce(hash, re); // uses findNonce method
                });
                thread.Start(); // start the thread
            }
            timer.Stop(); // stops the timer after correct nonce found 

            TimeSpan timeTaken = timer.Elapsed; // time taken to find correct nonce
            Debug.WriteLine("Time taken to find correct nonce:" + timeTaken); // output timeTaken to debug window

            float floatTimeSpan;
            int seconds, milliseconds;
            seconds = timeTaken.Seconds;
            milliseconds = timeTaken.Milliseconds;
            floatTimeSpan = (float)seconds + ((float)milliseconds / 1000); // convert time taken from timespan to float in seconds and milliseconds

            return Tuple.Create(hash, floatTimeSpan); // Return the hash meeting the difficulty requirement and time to mine
        }

        // increments nonce until correct one found, uses lock for threading
        public void findNonce (string hash, string re)
        {
            while (!hash.StartsWith(re)) // Check the resultant hash against the "re" string
            {
                lock (threadLock)
                {
                    nonce++; // Increment the nonce should the difficulty level not be satisfied
                    hash = CreateHash(); // Rehash with the new nonce as to generate a different hash
                }
            }
        }

        // Merkle Root Algorithm - Encodes transactions within a block into a single hash
        public static String MerkleRoot(List<Transaction> transactionList)
        {
            List<String> hashes = transactionList.Select(t => t.hash).ToList(); // Get a list of transaction hashes for "combining"
            
            // Handle Blocks with...
            if (hashes.Count == 0) // No transactions
            {
                return String.Empty;
            }
            if (hashes.Count == 1) // One transaction - hash with "self"
            {
                return HashCode.HashTools.combineHash(hashes[0], hashes[0]);
            }
            while (hashes.Count != 1) // Multiple transactions - Repeat until tree has been traversed
            {
                List<String> merkleLeaves = new List<String>(); // Keep track of current "level" of the tree

                for (int i=0; i<hashes.Count; i+=2) // Step over neighbouring pair combining each
                {
                    if (i == hashes.Count - 1)
                    {
                        merkleLeaves.Add(HashCode.HashTools.combineHash(hashes[i], hashes[i])); // Handle an odd number of leaves
                    }
                    else
                    {
                        merkleLeaves.Add(HashCode.HashTools.combineHash(hashes[i], hashes[i + 1])); // Hash neighbours leaves
                    }
                }
                hashes = merkleLeaves; // Update the working "layer"
            }
            return hashes[0]; // Return the root node
        }

        // Create reward for incentivising the mining of block
        public Transaction createRewardTransaction(List<Transaction> transactions)
        {
            double fees = transactions.Aggregate(0.0, (acc, t) => acc + t.fee); // Sum all transaction fees
            return new Transaction("Mine Rewards", minerAddress, (reward + fees), 0, ""); // Issue reward as a transaction in the new block
        }

        /* Concatenate all properties to output to the UI */
        public override string ToString()
        {
            return "[BLOCK START]"
                + "\nIndex: " + index
                + "\tTimestamp: " + timestamp
                + "\nPrevious Hash: " + prevHash
                + "\n-- PoW --"
                + "\nDifficulty Level: " + difficulty
                + "\nNonce: " + nonce
                + "\nHash: " + hash
                + "\nMineTime: " + mineTime
                + "\n-- Rewards --"
                + "\nReward: " + reward
                + "\nMiners Address: " + minerAddress
                + "\n-- " + transactionList.Count + " Transactions --"
                +"\nMerkle Root: " + merkleRoot
                + "\n" + String.Join("\n", transactionList)
                + "\n[BLOCK END]";
        }
    }
}
