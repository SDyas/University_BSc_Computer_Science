using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace BlockchainAssignment
{
    public partial class BlockchainApp : Form
    {
        // Global blockchain object
        private Blockchain blockchain;
        static float BLOCK_GENERATION_INTERVAL = 0.05f; // how often a block should be found
        static float DIFFICULTY_ADJUSTMENT_INTERVAL = 5; // how many blocks before difficulty is adjusted
        private float targetTime = BLOCK_GENERATION_INTERVAL * DIFFICULTY_ADJUSTMENT_INTERVAL; // target time to mine block

        // Default App Constructor
        public BlockchainApp()
        {
            // Initialise UI Components
            InitializeComponent();
            // Create a new blockchain 
            blockchain = new Blockchain();
            // Update UI with an initalisation message
            UpdateText("New blockchain initialised!");
        }

        /* PRINTING */
        // Helper method to update the UI with a provided message
        private void UpdateText(String text)
        {
            output.Text = text;
        }

        // Print entire blockchain to UI
        private void ReadAll_Click(object sender, EventArgs e)
        {
            UpdateText(blockchain.ToString());
        }

        // Print Block N (based on user input)
        private void PrintBlock_Click(object sender, EventArgs e)
        {
            if (Int32.TryParse(blockNo.Text, out int index))
                UpdateText(blockchain.GetBlockAsString(index));
            else
                UpdateText("Invalid Block No.");
        }

        // Print pending transactions from the transaction pool to the UI
        private void PrintPendingTransactions_Click(object sender, EventArgs e)
        {
            UpdateText(String.Join("\n", blockchain.transactionPool));
        }

        /* WALLETS */
        // Generate a new Wallet and fill the public and private key fields of the UI
        private void GenerateWallet_Click(object sender, EventArgs e)
        {
            Wallet.Wallet myNewWallet = new Wallet.Wallet(out string privKey);

            publicKey.Text = myNewWallet.publicID;
            privateKey.Text = privKey;
        }

        // Validate the keys loaded in the UI by comparing their mathematical relationship
        private void ValidateKeys_Click(object sender, EventArgs e)
        {
            if (Wallet.Wallet.ValidatePrivateKey(privateKey.Text, publicKey.Text))
                UpdateText("Keys are valid");
            else
                UpdateText("Keys are invalid");
        }

        // Check the balance of current user
        private void CheckBalance_Click(object sender, EventArgs e)
        {
            UpdateText(blockchain.GetBalance(publicKey.Text).ToString() + " Assignment Coin");
        }


        /* TRANSACTION MANAGEMENT */
        // Create a new pending transaction and add it to the transaction pool
        private void CreateTransaction_Click(object sender, EventArgs e)
        {
            Transaction transaction = new Transaction(publicKey.Text, reciever.Text, Double.Parse(amount.Text), Double.Parse(fee.Text), privateKey.Text);
            /* TODO: Validate transaction */
            blockchain.transactionPool.Add(transaction);
            UpdateText(transaction.ToString());
        }

        /* BLOCK MANAGEMENT */
        // Conduct Proof-of-work in order to mine transactions from the pool and submit a new block to the Blockchain
        private void NewBlock_Click(object sender, EventArgs e)
        {
            // Retrieve pending transactions to be added to the newly generated Block
            List<Transaction> transactions = blockchain.GetPendingTransactions(publicKey.Text);

            // Create and append the new block - requires a reference to the previous block, a set of transactions and the miners public address (For the reward to be issued)

            Block newBlock = new Block(blockchain.GetLastBlock(), transactions, publicKey.Text);
            blockchain.blocks.Add(newBlock);

            // changes difficulty according to average mining time against target mining time
            int numberBlocks = blockchain.blocks.Count; // get number of blocks in blockchain
            if (numberBlocks % DIFFICULTY_ADJUSTMENT_INTERVAL == 0) // if number of blocks added for next interval
            {
                float mineTimeSum = 0;
                for (int i = 1; i < DIFFICULTY_ADJUSTMENT_INTERVAL; i++)
                {
                    mineTimeSum += blockchain.GetBlock(i).mineTime; // get sum of mine time for last n blocks in blockchain
                }
                float avgMineTime = mineTimeSum / DIFFICULTY_ADJUSTMENT_INTERVAL; // average time for each block to mine

                if (avgMineTime < targetTime / 2) // increase difficulty by 1 as too easy
                {
                    Block.difficulty += 1;
                }
                else if (avgMineTime > targetTime * 2) // decrease difficulty by 1 as too hard
                {
                    Block.difficulty -= 1;
                }
            }
            blockchain.blocks.Add(newBlock);
            UpdateText(blockchain.ToString());

        }


        /* BLOCKCHAIN VALIDATION */
        // Validate the integrity of the state of the Blockchain
        private void Validate_Click(object sender, EventArgs e)
        {
            // CASE: Genesis Block - Check only hash as no transactions are currently present
            if(blockchain.blocks.Count == 1)
            {
                if (!Blockchain.ValidateHash(blockchain.blocks[0])) // Recompute Hash to check validity
                    UpdateText("Blockchain is invalid");
                else
                    UpdateText("Blockchain is valid");
                return;
            }

            for (int i=1; i<blockchain.blocks.Count-1; i++)
            {
                if(
                    blockchain.blocks[i].prevHash != blockchain.blocks[i - 1].hash || // Check hash "chain"
                    !Blockchain.ValidateHash(blockchain.blocks[i]) ||  // Check each blocks hash
                    !Blockchain.ValidateMerkleRoot(blockchain.blocks[i]) // Check transaction integrity using Merkle Root
                )
                {
                    UpdateText("Blockchain is invalid");
                    return;
                }
            }
            UpdateText("Blockchain is valid");
        }

        /* TRANSACTION TYPE SELECTION */ 
        
        // default (select from front of list)   
        private void defaultButton_Click(object sender, EventArgs e)
        {
            if (blockchain.transactionType != 0) // if not already set to default
            {
                blockchain.transactionType = 0; // set to default
                UpdateText("Changed Transaction Type to DEFAULT");
            }
        }

        // greedy (select highest fees first)
        private void greedyButton_Click(object sender, EventArgs e)
        {
            if (blockchain.transactionType != 1) // if not already set to greedy
            {
                blockchain.transactionType = 1; // set to greedy
                UpdateText("Changed Transaction Type to GREEDY"); // debug line
            }
        }

        // altruist (select transactions added earliest first) 
        private void altruistButton_Click(object sender, EventArgs e)
        {
            if (blockchain.transactionType != 2) // if not already set to altruist
            {
                blockchain.transactionType = 2; // set to altruist
                UpdateText("Changed Transaction Type to ALTRUIST"); // debug line
            }
        }

        // random (randomly selects from transaction pool)
        private void randomButton_Click(object sender, EventArgs e)
        {
            if (blockchain.transactionType != 3) // if not already set to random
            {
                blockchain.transactionType = 3; // set to random
                UpdateText("Changed Transaction Type to RANDOM"); // debug line
            }
        }

        // owner (selects transactions owner is sending or recieving first)
        private void ownerButton_Click(object sender, EventArgs e)
        {
            if (blockchain.transactionType != 4) // if not already set to owner
            {
                blockchain.transactionType = 4; // set to owner
                UpdateText("Changed Transaction Type to ADDRESS PREFERENCE"); // debug line
            }
        }
    }
}