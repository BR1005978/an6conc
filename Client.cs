namespace readytogo;

internal class Client
{
    // do you need to add variables here?
    // add the variables you need for concurrency here

    // do not add more variables after this comment.
    private readonly int id = 0;

    public Client(int id) // you can add more parameters if you need
    {
        this.id = id;
    }


    internal void DoWork()    // this method is not working properly
    {   // feel free to change the code in this method if needed but not the signature
        // each client will take a random range nap
        Thread.Sleep(new Random().Next(100, 500)); // do not remove this line
        // each client will place an order
        Order o = new();

        lock (Program.sharedOrdersLock)
        {
            //place the order
            Program.orders.AddFirst(o);  // do not remove this line
            // for each request of the client the cooks will prepare the order

            Console.WriteLine("C: Order placed by {0}", id); // do not remove this line

        }

        bool isOrderReady = false;

        while(!isOrderReady){
            lock(Program.sharedPickupsLock){
                if (Program.pickups.Count > 0 && Program.pickups.First().isReady())
                {
                    Program.pickups.RemoveFirst(); // do not remove this line
                    isOrderReady = true;
                    Console.WriteLine("C: Order ready for {0}", id); // do not remove this line
                }
            }

            if (!isOrderReady){
                Thread.Sleep(new Random().Next(100, 500)); // do not remove this line
            }
        }
    }
}