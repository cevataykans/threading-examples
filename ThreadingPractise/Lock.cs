using System;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadingPractise
{
    public class Lock : Example
    {
        public void ExecuteExample()
        {
            Console.WriteLine("Executing Lock example!");

            Task task = ExecuteLockExample();
            task.Wait();

            Console.WriteLine("Lock example executed!");
        }

        async Task ExecuteLockExample()
        {
            Account accountUnderTest = new Account(1000f);
            var tasks = new Task[100];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() => UpdateAccount(accountUnderTest));
            }
            await Task.WhenAll( tasks);
            Console.WriteLine( $"Account's balance is { accountUnderTest.GetAccountBalance()}");
        }

        void UpdateAccount( in Account account)
        {
            float[] amounts = { 0, 2, -3, 6, -2, -1, 8, -5, 11, -6 };
            foreach ( float amount in amounts)
            {
                if ( amount >= 0)
                {
                    account.Credit(amount);
                }
                else
                {
                    account.Debit(Math.Abs(amount));
                }
            }
        }

        class Account
        {
            private readonly Object balanceObject = new object();
            private float balance = 0;

            public Account( float balanceAmount)
            {
                balance = balanceAmount;
            }

            public void Credit( float amount)
            {
                if ( amount < 0)
                {
                    throw new ArgumentOutOfRangeException( nameof(amount), "The credit amount cannot be negative.");
                }

                lock ( balanceObject)
                {
                    balance += amount;
                }
            }

            public void Debit( float amount)
            {
                if ( amount < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(amount), "The debit amount cannot be negative.");
                }

                lock ( balanceObject)
                {
                    if (balance - amount >= 0)
                        balance -= amount;
                }
            }

            public float GetAccountBalance()
            {
                lock ( balanceObject)
                {
                    return balance;
                }
            }
        }
    }
}
