namespace Dirac.Singleton
{
    abstract public class Singleton<T> where T : class, new()
    {
        public Singleton()
        {
        }

        virtual public bool Initialize(params object[] args)
        {
            return true;
        }

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }
}