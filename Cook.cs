namespace readytogo;

internal class Cook
{
    // do you need to add variables here?
    // add the variables you need for concurrency here


    // do not add more variables after this comment.
    private readonly int id;

    public Cook(int id) // you can add more parameters if you need
    {
        this.id = id;
    }
    internal void DoWork()  // do not change the signature of this method
                            // this method is not working properly
    {
        Order? o = null;
        bool orderFound = false; 
        while (!orderFound) { 
            lock (Program.sharedOrdersLock)
            {
                if (Program.orders.Count > 0)
                { // each cook will ONLY get a dish from ONE order and prepare it
                    o = Program.orders.First(); // do not remove this line
                    Program.orders.RemoveFirst(); // do not remove this line
                    Console.WriteLine("K: Order taken by {0}, now preparing", id);  // do not remove this line
                    orderFound = true;
                }
            }
            if (!orderFound)
            {
                Thread.Sleep(new Random().Next(100, 500)); // do not remove this line
            }
        }
        if (o != null)
        {
        o.Done(); // the order is now ready
        Console.WriteLine("K: Order is: {0}", o.isReady()); // do not remove this line

        lock (Program.sharedPickupsLock){
            Program.pickups.AddFirst(o);                        // do not remove this line
            // now the client can pickup the order

            Console.WriteLine("K: Order ready");                // do not remove this line
            // each cook will terminate after preparing one order
        }
        }
    }
}
