    public interface ITransactionRunner
    {
        void Run(Action action);
    }

    public class TransactionRunner : ITransactionRunner
    {
        private readonly MyDbContext _context;

        public TransactionRunner(MyDbContext context)
        {
            _context = context;
        }

        public void Run(Action action)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    action();
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
	
	// Usages
	TransactionRunner.Run(() =>
	{
		_userRepository.Add(new User(currentUser));
	});